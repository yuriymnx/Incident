using System.Reflection;

namespace Incident.Core.Utils;

public static class AssemblyResource
{
    public static Stream GetResourceStream(string resourceFileName)
    {
        var assembly = Assembly.GetCallingAssembly();
        var resourceName = assembly.GetManifestResourceNames().Single(x => x.EndsWith(resourceFileName));
        return assembly.GetManifestResourceStream(resourceName) ?? throw new InvalidOperationException($"Ресурс с именем {resourceName} не найден");
    }

    public static string GetResourceText(string resourceFileName)
    {
        using var stream = GetResourceStream(resourceFileName);
        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }
}