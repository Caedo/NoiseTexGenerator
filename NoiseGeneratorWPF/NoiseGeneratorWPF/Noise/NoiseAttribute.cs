using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
