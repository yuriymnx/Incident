using Incident.Common;

namespace Incident.Core.Configuration.Application;

public class AppSettings
{
    public string LogLevel { get; set; } = Constants.DefaultLogLevel;
}