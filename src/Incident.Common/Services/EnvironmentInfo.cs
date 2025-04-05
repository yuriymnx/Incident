using Incident.Common.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace Incident.Common.Services;

internal class EnvironmentInfo : IEnvironmentInfo
{
    private readonly ILogger<EnvironmentInfo> _logger;
    public EnvironmentInfo(ILogger<EnvironmentInfo> logger)
    {
        _logger = logger;
    }

#if Linux
    private const bool IsLinux = true;
#else
    private const bool IsLinux = false;
#endif

#if Windows
    private const bool IsWindows = true;
#else
    private const bool IsWindows = false;
#endif

#if OSX
    private const bool IsOSX = true;
#else
    private const bool IsOSX = false;
#endif

#if DEBUG
    private const string Configuration = "DEBUG";
#else
    private const string Configuration = "RELEASE";
#endif


#if TEST_BUILD
    private const bool IsTestBuild = true;
#else
    private const bool IsTestBuild = false;
#endif


    public void Dump()
    {
        var callerAssemblyName = Assembly.GetExecutingAssembly().GetName().Name ?? "undefined";
        _logger.LogDebug("Assembly: {0}, configuration = {1}, is windows = {2}, is linus = {3}, is OSX = {4}, is test build = {5}",
            callerAssemblyName,
            Configuration,
            IsWindows,
            IsLinux,
            IsOSX,
            IsTestBuild);
    }
}