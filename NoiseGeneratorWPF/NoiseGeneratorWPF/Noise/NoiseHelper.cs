using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NoiseGeneratorWPF
{
    public static class NoiseHelper
    {
        public static Dictionary<string, INoise> GetNoiseDictionary()
        {
            Dictionary<string, INoise> dictionary = new Dictionary<string, INoise>();
            var assembly = Assembly.GetExecutingAssembly();
            foreach (var type in assembly.GetTypes())
            {
                var att = type.GetCustomAttribute(typeof(NoiseAttribute), true);
                if (att != null && type.GetInterface("INoise") != null)
                {
                    dictionary.Add((att as NoiseAttribute).Name, Activator.CreateInstance(type) as INoise);
                }
            }

            return dictionary;
        }
    }
}
