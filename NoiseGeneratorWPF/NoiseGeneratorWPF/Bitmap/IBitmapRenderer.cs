namespace NoiseGeneratorWPF
{
    /// <summary>
    /// Interface that provides <c>GenerateNoiseData</c> method
    /// </summary>
    public interface IBitmapRenderer
    {
        /// <summary>
        /// Create bitmap array of noise values
        /// </summary>
        /// <param name="data"><c>NoiseData</c> struct with data about noise generation</param>
        /// <param name="noiseObject"><c>INoise</c> interface that provides noise function</param>
        /// <returns>Returns array of bytes with pixel values</returns>
        byte[] GenerateNoiseMap(NoiseData data, INoise noiseObject);
    }
}
