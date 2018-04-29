using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace NoiseGeneratorWPF
{
    public struct NoiseData
    {
        public int width;
        public int height;
        public int stride;

        public float scale;
        public int octaves;
        public float persistance;
        public float lacunarity;
        public Vector2 offset;
    }
}
