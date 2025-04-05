using Microsoft.Extensions.Options;

namespace Incident.Core.Configuration;

public class UserConfiguration : IUserConfiguration
{
    private readonly IOptions<UserSettings> _settings;
    private readonly AppConfigurationSource<UserSettings> _configurationSource;
    public UserConfiguration(IOptions<UserSettings> settings, AppConfigurationSource<UserSettings> configurationSource)
    {
        _settings = settings;
        _configurationSource = configurationSource;
    }
    public string UserName
    {
        get => _settings.Value.UserName;
        set => _settings.Value.UserName = value;

    }

    public async Task Save() => await _configurationSource.Save(_settings.Value);
}
