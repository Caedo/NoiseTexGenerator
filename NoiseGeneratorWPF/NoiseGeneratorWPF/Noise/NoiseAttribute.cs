using System;

namespace NoiseGeneratorWPF
{
    [AttributeUsage(AttributeTargets.Class)]
    public class NoiseAttribute : Attribute
    {
        public string Name { get; }

        public NoiseAttribute(string name)
        {
            Name = name;
        }
    }
}
