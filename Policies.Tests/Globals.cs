﻿using Serilog;
using Serilog.Extensions.Logging;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace Policies.Tests;

public static class Globals
{
    public static readonly ILogger Logger = new SerilogLoggerFactory(
        new LoggerConfiguration().MinimumLevel.Debug()
            .WriteTo.NUnitOutput()
            .CreateLogger()).CreateLogger("TestLogger");
}