﻿using System;
using System.Numerics;

namespace NoiseGeneratorWPF
{
    /// <summary>
    /// Perlin noise implementation using static hash table and gradient table. Implements <c>INoise Interface</c>
    /// </summary>
    [Noise("Perlin Noise")]
    class PerlinNoise : INoise
    {
        /// <summary>
        /// Hash table used for generating noise
        /// </summary>
        private static readonly int[] hash =
        {
        151,160,137, 91, 90, 15,131, 13,201, 95, 96, 53,194,233,  7,225,
        140, 36,103, 30, 69,142,  8, 99, 37,240, 21, 10, 23,190,  6,148,
        247,120,234, 75,  0, 26,197, 62, 94,252,219,203,117, 35, 11, 32,
         57,177, 33, 88,237,149, 56, 87,174, 20,125,136,171,168, 68,175,
         74,165, 71,134,139, 48, 27,166, 77,146,158,231, 83,111,229,122,
         60,211,133,230,220,105, 92, 41, 55, 46,245, 40,244,102,143, 54,
         65, 25, 63,161,  1,216, 80, 73,209, 76,132,187,208, 89, 18,169,
        200,196,135,130,116,188,159, 86,164,100,109,198,173,186,  3, 64,
         52,217,226,250,124,123,  5,202, 38,147,118,126,255, 82, 85,212,
        207,206, 59,227, 47, 16, 58, 17,182,189, 28, 42,223,183,170,213,
        119,248,152,  2, 44,154,163, 70,221,153,101,155,167, 43,172,  9,
        129, 22, 39,253, 19, 98,108,110, 79,113,224,232,178,185,112,104,
        218,246, 97,228,251, 34,242,193,238,210,144, 12,191,179,162,241,
         81, 51,145,235,249, 14,239,107, 49,192,214, 31,181,199,106,157,
        184, 84,204,176,115,121, 50, 45,127,  4,150,254,138,236,205, 93,
        222,114, 67, 29, 24, 72,243,141,128,195, 78, 66,215, 61,156,180,

        151,160,137, 91, 90, 15,131, 13,201, 95, 96, 53,194,233,  7,225,
        140, 36,103, 30, 69,142,  8, 99, 37,240, 21, 10, 23,190,  6,148,
        247,120,234, 75,  0, 26,197, 62, 94,252,219,203,117, 35, 11, 32,
         57,177, 33, 88,237,149, 56, 87,174, 20,125,136,171,168, 68,175,
         74,165, 71,134,139, 48, 27,166, 77,146,158,231, 83,111,229,122,
         60,211,133,230,220,105, 92, 41, 55, 46,245, 40,244,102,143, 54,
         65, 25, 63,161,  1,216, 80, 73,209, 76,132,187,208, 89, 18,169,
        200,196,135,130,116,188,159, 86,164,100,109,198,173,186,  3, 64,
         52,217,226,250,124,123,  5,202, 38,147,118,126,255, 82, 85,212,
        207,206, 59,227, 47, 16, 58, 17,182,189, 28, 42,223,183,170,213,
        119,248,152,  2, 44,154,163, 70,221,153,101,155,167, 43,172,  9,
        129, 22, 39,253, 19, 98,108,110, 79,113,224,232,178,185,112,104,
        218,246, 97,228,251, 34,242,193,238,210,144, 12,191,179,162,241,
         81, 51,145,235,249, 14,239,107, 49,192,214, 31,181,199,106,157,
        184, 84,204,176,115,121, 50, 45,127,  4,150,254,138,236,205, 93,
        222,114, 67, 29, 24, 72,243,141,128,195, 78, 66,215, 61,156,180
        };

        /// <summary>
        /// Mask for the hash table size (faster modulo)
        /// </summary>
        private static readonly int hashMask = 255;

        /// <summary>
        /// Array of gradient directions used for generating noise
        /// </summary>
        private static readonly Vector2[] gradients2D =
        {
        new Vector2( 1f, 0f),
        new Vector2(-1f, 0f),
        new Vector2( 0f, 1f),
        new Vector2( 0f,-1f),
        Vector2.Normalize(new Vector2( 1f, 1f)),
        Vector2.Normalize(new Vector2(-1f, 1f)),
        Vector2.Normalize(new Vector2( 1f,-1f)),
        Vector2.Normalize(new Vector2(-1f,-1f))
        };

        /// <summary>
        /// Mask for the gradient table size (faster modulo)
        /// </summary>
        private const int gradientsMask2D = 7;

        /// <summary>
        /// Value of the square root of 2
        /// </summary>
        private static readonly float sqr2 = (float)Math.Sqrt(2);

        /// <summary>
        /// Get value of the Perlin noise function at the specific position 
        /// </summary>
        /// <param name="position">Position of the noise sample.</param>
        /// <returns>Returns PErlin noise sample of the given position. The returned value is between -1 and 1.</returns>
        public float GetValue(Vector2 position)
        {
            int ix0 = (int)Math.Floor(position.X);
            int iy0 = (int)Math.Floor(position.Y);

            float tx0 = position.X - ix0;
            float ty0 = position.Y - iy0;
            float tx1 = tx0 - 1f;
            float ty1 = ty0 - 1f;
            ix0 &= hashMask;
            iy0 &= hashMask;
            int ix1 = ix0 + 1;
            int iy1 = iy0 + 1;

            int h0 = hash[ix0];
            int h1 = hash[ix1];
            Vector2 g00 = gradients2D[hash[h0 + iy0] & gradientsMask2D];
            Vector2 g10 = gradients2D[hash[h1 + iy0] & gradientsMask2D];
            Vector2 g01 = gradients2D[hash[h0 + iy1] & gradientsMask2D];
            Vector2 g11 = gradients2D[hash[h1 + iy1] & gradientsMask2D];

            float v00 = MathHelper.Dot(g00, tx0, ty0);
            float v10 = MathHelper.Dot(g10, tx1, ty0);
            float v01 = MathHelper.Dot(g01, tx0, ty1);
            float v11 = MathHelper.Dot(g11, tx1, ty1);

            float tx = MathHelper.Smooth(tx0);
            float ty = MathHelper.Smooth(ty0);
            return MathHelper.Lerp(MathHelper.Lerp(v00, v10, tx), MathHelper.Lerp(v01, v11, tx), ty) * sqr2;
        }
    }
}
