using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using NorthwindWeb.Infrastructure.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NorthwindWeb.Filters
{
    public class LogActionFilter : IAsyncActionFilter
    {
        private readonly ILogger<LogActionFilter> _logger;
        private readonly LogActionOptions _options;

        public LogActionFilter(ILogger<LogActionFilter> logger, IOptionsSnapshot<LogActionOptions> options)
        {
            _logger = logger;
            _options = options.Value;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var controllerActionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;
            var actionName = controllerActionDescriptor.ActionName;
            var controllerName = controllerActionDescriptor.ControllerName;

            StringBuilder logString = new StringBuilder();
            if (_options.LogParameters)
            {
                logString = context.ActionArguments.Aggregate(logString, (current, p) =>
                    current.Append(p.Key).Append(":")
                        .Append(JsonConvert.SerializeObject(p.Value))
                        .Append(Environment.NewLine));
                _logger.LogInformation("Action {actionName} in controller {controllerName} start, arguments: {arguments}", actionName, controllerName, logString);
            }
            else
            {
                _logger.LogInformation("Action {actionName} in controller {controllerName} start", actionName, controllerName);
            }

            await next();

            if (_options.LogParameters)
            {
                _logger.LogInformation("Action {actionName} in controller {controllerName} end, arguments: {arguments}", actionName, controllerName, logString);
            }
            else
            {
                _logger.LogInformation("Action {actionName} in controller {controllerName} end", actionName, controllerName);
            }
        }
    }
}
