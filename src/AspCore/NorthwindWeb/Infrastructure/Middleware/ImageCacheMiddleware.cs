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

        private ConcurrentDictionary<string, (string path, string contentType, DateTime expiration)> _cachedImages = 
            new ConcurrentDictionary<string, (string path, string contentType, DateTime expiration)>();
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
            var path = context.Request.Path;
            if (_cachedImages.TryGetValue(path, out var value))
            {
                if (DateTime.Now > value.expiration)
                {
                    _cachedImages.TryRemove(path, out _);
                }
                else
                {
                    context.Response.ContentType = value.contentType;
                    var image = FileHelper.GetBytesFromFile(value.path);
                    await context.Response.Body.WriteAsync(image, 0, image.Length);

                    return;
                }
            }

            if (_cachedImages.Count >= _maxCachedImages)
            {
                await _next(context);
            }
            else
            {
                var originalBodyStream = context.Response.Body;

                using (var responseBody = new MemoryStream())
                {
                    context.Response.Body = responseBody;

                    await _next(context);

                    await CacheIfImage(context, path);

                    context.Response.Body.Seek(0, SeekOrigin.Begin);
                    await responseBody.CopyToAsync(originalBodyStream);
                }
            }
        }

        private async Task CacheIfImage(HttpContext context, PathString path)
        {
            if (context.Response.ContentType != null &&
                        context.Response.ContentType.StartsWith(_contextTypePrefix))
            {
                var route = context.GetRouteData();
                if (route.Values.TryGetValue("id", out var routeId))
                {
                    var imagePath = $"{_cacheFolder}/{routeId}";
                    await FileHelper.CreateFile(context.Response.Body, imagePath);
                    _cachedImages[path] = (imagePath, context.Response.ContentType, DateTime.Now.AddSeconds(_cacheExpiration));
                }
            }
        }
    }
}
