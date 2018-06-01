using System;
using System.Collections.Generic;
using System.Reflection;

namespace NoiseGeneratorWPF
{
    /// <summary>
    /// Static class that contains function to work with Noise classes
    /// </summary>
    public static class NoiseHelper
    {
        /// <summary>
        /// Get all classes with <c>Noise</c> attribute using reflections.
        /// </summary>
        /// <returns>Returns dictionary created using Name property of Noise Attribute as key and instance of class with that attribute as value.</returns>
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
