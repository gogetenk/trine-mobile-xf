using Microsoft.Extensions.Logging;
using Prism.Logging;
using System;

namespace Trine.Mobile.Components.Logging
{
    public class PrismLoggerWrapper<T> : PrismLoggerWrapper, ILogger<T>, Microsoft.Extensions.Logging.ILogger
    {
        public PrismLoggerWrapper(Prism.Logging.ILogger logger)
            : base(logger)
        {
        }
    }

    public class PrismLoggerWrapper : Microsoft.Extensions.Logging.ILogger
    {
        private readonly ILoggerFacade _logger;

        public PrismLoggerWrapper(ILoggerFacade logger)
        {
            _logger = logger;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            var prismLogLevel = ConvertLogLevel(logLevel);
            if (!IsEnabled(prismLogLevel)) return;
            var message = formatter.Invoke(state, exception);
            _logger.Log(message, prismLogLevel, Priority.None);
        }

        private Category ConvertLogLevel(LogLevel logLevel)
        {
            switch (logLevel)
            {
                case LogLevel.Trace:
                    return Category.Debug;
                case LogLevel.Debug:
                    return Category.Debug;
                case LogLevel.Information:
                    return Category.Info;
                case LogLevel.Warning:
                    return Category.Warn;
                case LogLevel.Error:
                    return Category.Exception;
                case LogLevel.Critical:
                    return Category.Exception;
                case LogLevel.None:
                    return Category.Debug;
                default:
                    throw new ArgumentOutOfRangeException(nameof(logLevel), logLevel, null);
            }
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return IsEnabled(ConvertLogLevel(logLevel));
        }

        private bool IsEnabled(Category logLevel)
        {
            return true;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            throw new NotImplementedException();
        }
    }

}
