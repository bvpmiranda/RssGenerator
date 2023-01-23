using System;

namespace RssGenerator.Infrastructure
{
    public interface ILogger
    {
        void Debug(string message, params object[] objects);
        void Error(string message, params object[] objects);
        void Exception(string requestName, Exception exception);
        void Info(string message, params object[] objects);
        void Warning(string message, params object[] objects);
    }
}