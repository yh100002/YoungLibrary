using System;
using Microsoft.Extensions.Logging;

namespace PerformanceLogger
{
    public interface IOptions
    {
        IOptions WithLogLevel(LogLevel logLevel);
        IOptions WithFormat(string format);
    }
}
