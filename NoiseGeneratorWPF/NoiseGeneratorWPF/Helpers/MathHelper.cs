using System.Numerics;

namespace NoiseGeneratorWPF
{
    public static class MathHelper
    {

        public static float Smooth(float t)
        {
            return t * t * t * (t * (t * 6f - 15f) + 10f);
        }

        public static float Lerp(float a, float b, float t)
        {
            return a + (b - a) * t;
        }

        public static float Dot(Vector2 g, float x, float y)
        {
            return g.X * x + g.Y * y;
        }

        public static float InverseLerp(float a, float b, float t)
        {
            return (t - a) / (b - a);
        }

        public static byte InverseLerp(byte a, byte b, byte t)
        {
            return (byte)((t - a) / (b - a));
        }
    }
}
