using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NorthwindWeb.Infrastructure.Options
{
    public class ImageCacheOptions
    {
        public string ImageCacheFolder { get; set; }
        public int MaxCachedImages { get; set; } = 100;
        public int ImageCacheExpiration { get; set; } = 86400;
    }
}
