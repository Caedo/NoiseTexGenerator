using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Numerics;

namespace NoiseGeneratorWPF
{
    class BitmapRenderer : IBitmapRenderer
    {
        public byte[] GenerateNoiseMap(NoiseData data, INoise noiseClass)
        {
            int stride = data.stride;
            byte[] noiseMap = new byte[stride * data.height];
            float[] floatMap = new float[stride * data.height];

            float halfWidth = data.width / 2f;
            float halfHeight = data.height / 2f;

            float scale = data.scale;
            if (scale <= 0)
            {
                scale = 0.0001f;
            }

            float max = float.MinValue;
            float min = float.MaxValue;

            for (int y = 0; y < data.height; y++)
            {
                for (int x = 0; x < data.width; x++)
                {

                    float amplitude = 1;
                    float frequency = 1;
                    float value = 0;
                    float range = 1;

                    for (int i = 0; i < data.octaves; i++)
                    {
                        value += noiseClass.GetValue((new Vector2(x - halfWidth, y - halfHeight) + data.offset) / scale * frequency) * amplitude;
                        amplitude *= data.persistance;
                        frequency *= data.lacunarity;

                        range += amplitude;
                    }

                    value /= range;

                    if (value > max)
                    {
                        max = value;
                    }
                    if (value < min)
                    {
                        min = value;
                    }


                    floatMap[y * data.width + x] = value;
                }
            }

            for (int y = 0; y < data.height; y++)
            {
                for (int x = 0; x < data.width; x++)
                {
                    int pos = y * data.width + x;
                    noiseMap[pos] = (byte)(MathHelper.InverseLerp(min, max, floatMap[pos]) * 255);
                }
            }

            return noiseMap;
        }
    }
}
