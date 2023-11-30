using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Il2CppInterop.Runtime.InteropTypes.Arrays;

namespace HaxxToyBox.Utilities;

public static class AssetBundleManager
{
    public static AssetBundle Load(string name)
    {
        return Load(Assembly.GetCallingAssembly(), name);
    }

    private static bool TryFindFile(Assembly assembly, string fileName, [NotNullWhen(true)] out string path)
    {
        var pluginDirectoryPath = Path.GetDirectoryName(assembly.Location);
        if (pluginDirectoryPath != null) {
            var filePath = Path.Combine(pluginDirectoryPath, fileName);
            if (File.Exists(filePath)) {
                path = filePath;
                return true;
            }
        }

        path = null;
        return false;
    }

    private static bool TryLoadResource(Assembly assembly, string fileName, [NotNullWhen(true)] out Il2CppStructArray<byte> data)
    {
        var resourceName = assembly.GetManifestResourceNames().SingleOrDefault(n => n.EndsWith(fileName, StringComparison.Ordinal));
        if (resourceName != null) {
            using var stream = assembly.GetManifestResourceStream(resourceName) ?? throw new InvalidOperationException("Resource stream was null");

            var length = (int)stream.Length;
            data = new Il2CppStructArray<byte>(length);
            //if (stream.Read(data.ToSpan()) < length) throw new IOException("Failed to read in full");

            return true;
        }

        data = null;
        return false;
    }

    public static AssetBundle Load(Assembly assembly, string name)
    {
        var fileName = "toybox";

        if (TryFindFile(assembly, fileName, out var filePath)) {
            return AssetBundle.LoadFromFile(filePath);
        }

        if (TryLoadResource(assembly, fileName, out var data)) {
            return AssetBundle.LoadFromMemory(data);
        }

        throw new AssetBundleNotFoundException(name);
    }
}

public class AssetBundleNotFoundException : IOException
{
    internal AssetBundleNotFoundException(string name) : base("Couldn't find an assetbundle named " + name)
    {
    }
}
