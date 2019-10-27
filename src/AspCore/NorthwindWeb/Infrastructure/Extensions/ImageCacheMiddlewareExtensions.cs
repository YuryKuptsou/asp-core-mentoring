using Microsoft.AspNetCore.Builder;
using NorthwindWeb.Infrastructure.Middleware;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace NorthwindWeb.Infrastructure.Extensions
{
    public static class ImageCacheMiddlewareExtensions
    {
        public static IApplicationBuilder UseImageCache(this IApplicationBuilder builder, string cacheFolder,
            int maxCachedImages = 100, int cacheExpiration = 86400)
        {
            if (maxCachedImages < 0)
            {
                throw new ArgumentException();
            }
            if (cacheExpiration < 0)
            {
                throw new ArgumentException();
            }
            if (string.IsNullOrEmpty(cacheFolder))
            {
                throw new ArgumentException();
            }
            if (!Directory.Exists(cacheFolder))
            {
                Directory.CreateDirectory(cacheFolder);
            }

            return builder.UseMiddleware<ImageCacheMiddleware>(cacheFolder, maxCachedImages, cacheExpiration);
        }
    }
}
