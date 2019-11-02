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
        private ActionExecutingContext _context;

        private readonly string _start = "start";
        private readonly string _end = "end";

        private readonly ILogger<LogActionFilter> _logger;
        private readonly LogActionOptions _options;

        public LogActionFilter(ILogger<LogActionFilter> logger, IOptionsSnapshot<LogActionOptions> options)
        {
            _logger = logger;
            _options = options.Value;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            _context = context;

            LogAction(_start);
            await next();
            LogAction(_end);
        }

        private string GetActionArguments()
        {
            StringBuilder arguments = new StringBuilder();
            arguments = _context.ActionArguments.Aggregate(arguments, (current, p) =>
                    current.Append(p.Key).Append(":")
                        .Append(JsonConvert.SerializeObject(p.Value))
                        .Append(Environment.NewLine));

            return arguments.ToString();
        }

        private void LogAction(string actionType)
        {
            var controllerActionDescriptor = _context.ActionDescriptor as ControllerActionDescriptor;
            var actionName = controllerActionDescriptor.ActionName;
            var controllerName = controllerActionDescriptor.ControllerName;

            if (_options.LogParameters)
            {
                var arguments = GetActionArguments();
                _logger.LogInformation("Action {actionName} in controller {controllerName} {actionType}, arguments: {arguments}",
                    actionName, controllerName, actionType, arguments);
            }
            else
            {
                _logger.LogInformation("Action {actionName} in controller {controllerName} {actionType}",
                    actionName, controllerName, actionType);
            }
        }

    }
}
