using System.IO;
using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Utilities.Collections;
using Serilog;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

class Build : NukeBuild
{
    public static int Main() => Execute<Build>(x => x.Compile);

    [Solution] readonly Solution Solution;

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    [Parameter("API key for pushing to NuGet (optional)")] readonly string NuGetApiKey;

    AbsolutePath ArtifactsDirectory => RootDirectory / "artifacts";

    
    Target Clean => _ => _
        .Before(Restore)
        .Executes(() =>
        {
            Log.Information("Cleaning old build output...");
            if (Directory.Exists(ArtifactsDirectory))
                Directory.Delete(ArtifactsDirectory, recursive: true);

            var binObjDirs = Directory.GetDirectories(RootDirectory, "bin", SearchOption.AllDirectories)
                .Concat(Directory.GetDirectories(RootDirectory, "obj", SearchOption.AllDirectories));

            foreach (var dir in binObjDirs)
            {
                Directory.Delete(dir, recursive: true);
            }
        });

    Target Restore => _ => _
        .Executes(() =>
        {
            Log.Information("Restoring solution...");
            DotNetRestore(s => s
                .SetProjectFile(Solution));
        });

    Target Compile => _ => _
        .DependsOn(Restore)
        .Executes(() =>
        {
            Log.Information("Building solution...");
            DotNetBuild(s => s
                .SetProjectFile(Solution)
                .SetConfiguration(Configuration)
                .EnableNoRestore()
                .SetOutputDirectory(ArtifactsDirectory));
        });
}
