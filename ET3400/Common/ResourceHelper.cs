using System.IO;
using System.Reflection;

namespace ET3400.Common
{
    class ResourceHelper
    {
        public static Stream GetEmbeddedResourceStream(Assembly assembly, string resourceName)
        {
            resourceName = FormatResourceName(assembly, resourceName);
            return assembly.GetManifestResourceStream(resourceName);
        }        

        public static string GetEmbeddedResource(Assembly assembly, string resourceName)
        {
            resourceName = FormatResourceName(assembly, resourceName);
            using (Stream resourceStream = assembly.GetManifestResourceStream(resourceName))
            {
                if (resourceStream == null)
                    return null;

                using (StreamReader reader = new StreamReader(resourceStream))
                {
                    return reader.ReadToEnd();
                }
            }
        }        
        private static string FormatResourceName(Assembly assembly, string resourceName)
        {
            return assembly.GetName().Name + "." + resourceName.Replace(" ", "_")
                       .Replace("\\", ".")
                       .Replace("/", ".");
        }
    }
}