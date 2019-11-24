using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NorthwindWeb.Models.Api
{
    public class ImageApiModel
    {
        public IFormFile File { get; set; }
    }
}
