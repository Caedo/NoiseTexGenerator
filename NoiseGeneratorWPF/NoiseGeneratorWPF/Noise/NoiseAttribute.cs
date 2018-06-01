using System;

namespace NoiseGeneratorWPF
{
    /// <summary>
    /// Classes that implements <c>INoise</c> interface and have <c>Noise</c> Attribute are recognized as noise clasess and presented on UI.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class NoiseAttribute : Attribute
    {
        /// <summary>
        /// Name of the noise class
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="NoiseAttribute"/> class.
        /// </summary>
        /// <param name="name">Name of the noise class</param>
        public NoiseAttribute(string name)
        {
            Name = name;
        }
    }
}
