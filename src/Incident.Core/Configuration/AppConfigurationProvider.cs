using Incident.Core.Configuration.Interfaces;
using Incident.Core.Utils;
using Microsoft.Extensions.Configuration.Json;
using Newtonsoft.Json;
using NLog;
using System.Text;

namespace Incident.Core.Configuration;

public abstract class AppConfigurationProvider : JsonConfigurationProvider
{
    protected AppConfigurationProvider(string configFilePath, JsonConfigurationSource source) : base(source)
    {
        ConfigFilePath = configFilePath;
    }

    public static string ConfigDirectory => Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        Common.Constants.AppDataSubfolder);

    protected string ConfigFilePath { get; }

    protected string GetDefaultConfigText() => AssemblyResource.GetResourceText(ConfigFilePath);

    protected static Logger Logger { get; } = LogManager.GetCurrentClassLogger();
}

public sealed class AppConfigurationProvider<T> : AppConfigurationProvider where T : class
{
    public AppConfigurationProvider(string configFilePath, JsonConfigurationSource source) : base(configFilePath, source)
    {
    }

    public override void Load()
    {
        try
        {
            var configDir = ConfigDirectory;
            if (!Directory.Exists(configDir))
            {
                Directory.CreateDirectory(configDir);
            }

            var filePath = Path.Combine(configDir, ConfigFilePath);
            var isValid = File.Exists(filePath);
            var defaultConfigText = GetDefaultConfigText();

            if (!isValid)
            {
                using var writer = File.CreateText(filePath);
                writer.Write(defaultConfigText);
            }
            else
            {
                var defaultSettings = TryParse(defaultConfigText) ??
                                      throw new InvalidOperationException("Ошибка парсинга стандартной конфигурации");
                var currentSettings = TryRead(filePath);
                if (defaultSettings is IConfigVersion dcv)
                {
                    var csv = (currentSettings as IConfigVersion)?.Version;
                    if (dcv.Version.Equals(csv, StringComparison.OrdinalIgnoreCase))
                    {
                        File.Copy(filePath, $"{filePath}.bak", true);
                        using var writer = File.CreateText(filePath);
                        writer.Write(defaultConfigText);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Logger.Error(ex, $"Ошибка при загрузке класса {nameof(T)} из файла");
        }
        
        base.Load();
    }

    internal async Task Save(T settings)
    {
        try
        {
            var configDir = ConfigDirectory;
            if (!Directory.Exists(configDir))
                Directory.CreateDirectory(configDir);
            var filePath = Path.Combine(configDir, ConfigFilePath);
            await File.WriteAllTextAsync(filePath, JsonConvert.SerializeObject(settings, Formatting.Indented),
                Encoding.UTF8);
        }
        catch (Exception ex)
        {
            Logger.Error(ex, $"Ошибка при сохранении класса {nameof(T)} в файл");
        }
    }

    private static T? TryParse(string json)
    {
        try
        {
            return JsonConvert.DeserializeObject<T>(json);
        }
        catch (Exception e)
        {
            Logger.Error(e);
            return null;
        }
    }
    private static T? TryRead(string filePath)
    {
        using var reader = File.OpenText(filePath);
        return TryParse(reader.ReadToEnd());
    }
}