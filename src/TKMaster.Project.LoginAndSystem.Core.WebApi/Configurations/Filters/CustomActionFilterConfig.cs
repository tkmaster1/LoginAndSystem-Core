using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using Npgsql;
using Microsoft.Data.SqlClient;
using TKMaster.Project.Common.Domain.Interfaces;
using TKMaster.Project.LoginAndSystem.Core.Domain.Notifications;

namespace TKMaster.Project.LoginAndSystem.Core.WebApi.Configurations.Filters
{
    public class CustomActionFilterConfig : IActionFilter, IOrderedFilter
    {
        #region Properties

        public int Order => int.MaxValue - 10;
        protected readonly DomainNotificationHandler _notifications;
        private readonly ILogger<CustomActionFilterConfig> _logger;
        private readonly ILoggerFactory _loggerFactory;

        #endregion

        #region Constructor

        public CustomActionFilterConfig(INotificationHandler<DomainNotification> notifications,
            ILogger<CustomActionFilterConfig> logger,
            ILoggerFactory loggerFactory)
        {
            _notifications = (DomainNotificationHandler)notifications;
            _logger = logger;
            _loggerFactory = loggerFactory;
        }

        #endregion

        #region Methods

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception != null)
            {
                if (context.Exception is SqlException || context.Exception is NpgsqlException)
                {
                    _logger.LogError($"[{DateTime.Now.ToString()}] [ERROR] - [Database]{context.Exception?.Message}" ?? context.Exception?.StackTrace ?? "");
                }
                else
                {
                    _logger.LogError($"[{DateTime.Now.ToString()}] [ERROR] - [Sistema]{context.Exception?.Message}" ?? context.Exception?.StackTrace ?? "");
                }

                _notifications.Handler(new DomainNotification("500", context.Exception.Message ?? context.Exception.StackTrace));

                var objErros = new
                {
                    key = "Error",
                    value = context.Exception.Message
                };

                context.Result = new ObjectResult(new
                {
                    Success = false,
                    Errors = objErros
                })
                {
                    StatusCode = 500
                };

                context.ExceptionHandled = true;
            }
        }

        public void OnActionExecuting(ActionExecutingContext context) { }

        #endregion
    }
}
