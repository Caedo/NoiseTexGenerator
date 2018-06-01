using System.Numerics;

namespace NoiseGeneratorWPF
{
    /// <summary>
    /// Interface that provides <c>GetValue</c> method. Classes that implements <c>INoise</c> interface
    /// and have <c>Noise</c> Attribute are recognized as noise clasess and presented on UI.
    /// </summary>
    public interface INoise
    {
        /// <summary>
        /// Get value of the noise function at the specific position 
        /// </summary>
        /// <param name="position">Position of the noise sample.</param>
        /// <returns>Returns noise sample of the given position. The returned value is between -1 and 1.</returns>
        float GetValue(Vector2 position);
    }
}
