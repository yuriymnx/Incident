using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.FileProviders;

namespace Incident.Core.Configuration;

public class AppConfigurationSource<T> : JsonConfigurationSource where T : class
{
    private readonly string _configFileName;

    public AppConfigurationSource(string configFileName)
    {
        _configFileName = configFileName;
        Path = _configFileName;
        FileProvider = new PhysicalFileProvider(AppConfigurationProvider.ConfigDirectory);

    }

    private AppConfigurationProvider<T>? _provider;

    public override IConfigurationProvider Build(IConfigurationBuilder builder)
    {
        _provider = new AppConfigurationProvider<T>(_configFileName, this);
        return _provider;
    }

    public async Task Save(T settings)
    {
        if (_provider == null)
            throw new InvalidOperationException("AppConfigurationProvider не инициализирован");
        await _provider.Save(settings);
    }

}