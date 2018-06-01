using System.Numerics;

namespace NoiseGeneratorWPF
{
    /// <summary>
    /// Static class with some methematical functions that Standard Library doesn't have
    /// </summary>
    public static class MathHelper
    {
        /// <summary>
        /// Smooth function, fifth degree polynomial: 6t^5 - 15t^4 + 10t^3
        /// </summary>
        /// <param name="t">Parameter of the polynomial</param>
        /// <returns>Returns evaluation of the smooth function</returns>
        /// <remarks>Function doesn't clamp t  value. If t  value is grater than 1 or lower than 0 it is extrapolation</remarks>
        public static float Smooth(float t)
        {
            return t * t * t * (t * (t * 6f - 15f) + 10f);
        }

        /// <summary>
        /// Linear interpolation beetween a and b by t
        /// </summary>
        /// <param name="a">Start Value</param>
        /// <param name="b">End Value</param>
        /// <param name="t">The interpolation value between 0 and 1</param>
        /// <returns>Returns interpolation result</returns>
        /// <remarks>Function doesn't clamp t value. If t value is grater than 1 or lower than 0 it is extrapolation</remarks>
        public static float Lerp(float a, float b, float t)
        {
            return a + (b - a) * t;
        }

        /// <summary>
        /// Dot product between vector g and vector created with x and y
        /// </summary>
        /// <param name="g">First vector of the product</param>
        /// <param name="x">x value of the second vector</param>
        /// <param name="y">y value of the second vector</param>
        /// <returns>Returns flaot value of the dot product between vector</returns>
        public static float Dot(Vector2 g, float x, float y)
        {
            return g.X * x + g.Y * y;
        }

        /// <summary>
        /// Inverse linear interpolation beetween a and b by t
        /// </summary>
        /// <param name="a">Start float Value</param>
        /// <param name="b">End flaot Value</param>
        /// <param name="t">The interpolation float value</param>
        /// <returns>Returns float value between 0 and 1</returns>
        /// <remarks>Function doesn't clamp t value. If t value is grater than a or lower than b it is extrapolation</remarks>
        public static float InverseLerp(float a, float b, float t)
        {
            return (t - a) / (b - a);
        }

        /// <summary>
        /// Inverse linear interpolation beetween a and b by t
        /// </summary>
        /// <param name="a">Start byte Value</param>
        /// <param name="b">End byte Value</param>
        /// <param name="t">The byte interpolation value</param>
        /// <returns>Returns byte value between 0 and 1</returns>
        /// <remarks>Function doesn't clamp t value. If t value is grater than a or lower than b it is extrapolation</remarks>
        public static byte InverseLerp(byte a, byte b, byte t)
        {
            return (byte)((t - a) / (b - a));
        }
    }
}
