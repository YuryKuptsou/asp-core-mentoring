using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace NorthwindWeb.Controllers
{
    [Route("Error")]
    public class ErrorController : Controller
    {
        public IActionResult Error()
        {
            var exceptionHandlerFeature = HttpContext.Features.Get<IExceptionHandlerFeature>();

            return View(exceptionHandlerFeature.Error);
        }
    }
}