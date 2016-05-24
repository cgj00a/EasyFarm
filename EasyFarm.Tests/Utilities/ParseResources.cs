using System;
using System.IO;
using System.Reflection;

namespace EasyFarm.Tests.Utilities
{
    internal static class ParseResources
    {
        public static string GetResource(string fileName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            if (assembly == null) throw new InvalidOperationException();
            var assemblyName = assembly.GetName().Name;
            string resourcePath = $"{assemblyName}.Resources.{fileName}";

            using (var stream = assembly.GetManifestResourceStream(resourcePath))
            {
                if (stream == null)
                    throw new Exception($"Could not locate file '{fileName}' in assembly '{assemblyName}'");

                using (var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}
