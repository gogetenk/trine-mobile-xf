using Microsoft.Extensions.Logging;
using Prism.Logging;
using System;
using System.Collections.Generic;

namespace Trine.Mobile.Bootstrapper.Wrappers
{
    public class PrismLoggerWrapper : Microsoft.Extensions.Logging.ILogger
    {
        private readonly Prism.Logging.ILogger _logger;

        public PrismLoggerWrapper(Prism.Logging.ILogger logger)
        {
            _logger = logger;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            Category category = ConvertLogLevel(logLevel);
            if (IsEnabled(logLevel) && IsEnabled(category))
            {
                string message = formatter(state, exception);
                _logger.Log(message, category, Priority.None);
                if (exception != null)
                {
                    _logger.Log(exception, category, new Dictionary<string, string>());
                }
            }
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
                    throw new ArgumentOutOfRangeException("logLevel", logLevel, null);
            }
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            if (logLevel != LogLevel.None)
            {
                return IsEnabled(ConvertLogLevel(logLevel));
            }
            return false;
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
