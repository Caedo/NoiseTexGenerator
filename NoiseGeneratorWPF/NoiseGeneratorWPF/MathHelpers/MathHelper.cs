using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            return a * (1 - t) + b * (t);
        }
    }
}
