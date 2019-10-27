using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace NorthwindWeb.Infrastructure.Middleware
{
    public class ImageCacheMiddleware
    {

        private ConcurrentDictionary<string, (string path, string contentType)> _cachedImages = 
            new ConcurrentDictionary<string, (string path, string contentType)>();
        private DateTime _startExpiration = DateTime.Now;
        private readonly string _contextTypePrefix = "image";
        

        private readonly RequestDelegate _next;
        private readonly string _cacheFolder;
        private readonly int _maxCachedImages;
        private readonly int _cacheExpiration;

        public ImageCacheMiddleware(RequestDelegate next, string cacheFolder,
            int maxCachedImages, int cacheExpiration)
        {
            _next = next;
            _cacheFolder = cacheFolder;
            _maxCachedImages = maxCachedImages;
            _cacheExpiration = cacheExpiration;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (_startExpiration.AddSeconds(_cacheExpiration) < DateTime.Now)
            {
                foreach (var file in new DirectoryInfo(_cacheFolder).GetFiles())
                {
                    file.Delete();
                }
                _cachedImages.Clear();
            }
            

            var path = context.Request.Path;
            if (_cachedImages.TryGetValue(path, out var value))
            {
                context.Response.ContentType = value.contentType;
                var image = File.ReadAllBytes(value.path);
                await context.Response.Body.WriteAsync(image, 0, image.Length);

                return;
            }

            if (_cachedImages.Count >= _maxCachedImages)
            {
                await _next(context);

                return;
            }

            var originalBodyStream = context.Response.Body;

            using (var responseBody = new MemoryStream())
            {
                context.Response.Body = responseBody;

                await _next(context);

                if (context.Response.ContentType != null &&
                    context.Response.ContentType.StartsWith(_contextTypePrefix))
                {
                    _startExpiration = DateTime.Now;
                    var route = context.GetRouteData();
                    if (route.Values.TryGetValue("id", out var routeId))
                    {
                        var imagePath = $"{_cacheFolder}/{routeId}";
                        await CreateFile(context.Response.Body, imagePath);
                        _cachedImages[path] = (imagePath, context.Response.ContentType);
                    }
                }
                context.Response.Body.Seek(0, SeekOrigin.Begin);
                await responseBody.CopyToAsync(originalBodyStream);
            }
        }

        private async Task CreateFile(Stream stream, string name)
        {
            using (var source = File.Create(name))
            {
                stream.Seek(0, SeekOrigin.Begin);
                await stream.CopyToAsync(source);
            }
        }
    }
}
