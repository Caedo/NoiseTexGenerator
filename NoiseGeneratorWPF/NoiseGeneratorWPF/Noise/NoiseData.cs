using System.Numerics;

namespace NoiseGeneratorWPF
{
    /// <summary>
    /// Struct that contains noise and texture parameters
    /// </summary>
    public struct NoiseData
    {
        /// <summary>
        /// Width of the bitmap
        /// </summary>
        public int width;
        /// <summary>
        /// Height of the bitmap
        /// </summary>
        public int height;
        /// <summary>
        /// Stride of the Bitmap
        /// </summary>
        public int stride;

        /// <summary>
        /// Scale of the noise texture
        /// </summary>
        public float scale;
        /// <summary>
        /// Number of octaves of the fBm algorithm
        /// </summary>
        public int octaves;
        /// <summary>
        /// Persistance value of the fBm algorithm
        /// </summary>
        public float persistance;
        /// <summary>
        /// Lacunarity value of the fBm algorithm
        /// </summary>
        public float lacunarity;
        /// <summary>
        /// Offset of the noise texture
        /// </summary>
        public Vector2 offset;
        /// <summary>
        /// Turbulence is active if true
        /// </summary>
        public bool turbulence;
    }
}
