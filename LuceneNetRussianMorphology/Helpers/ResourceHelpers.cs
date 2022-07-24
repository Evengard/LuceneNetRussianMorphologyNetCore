using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LuceneNetRussianMorphology.Helpers
{
    public static class ResourceHelpers
    {
        public static Stream? GetResource(string name)
        {
            var assembly = typeof(ResourceHelpers).Assembly;
            var fullname = assembly.GetManifestResourceNames().First(n => n.EndsWith($".{name}"));
            return assembly.GetManifestResourceStream(fullname);
        }

        public static Stream? GetResource<T>(string name)
        {
            var assembly = typeof(T).Assembly;
            var fullname = assembly.GetManifestResourceNames().First(n => n.EndsWith($".{name}"));
            return assembly.GetManifestResourceStream(fullname);
        }
    }
}
