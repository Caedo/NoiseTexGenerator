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
            PixelFormat pf = PixelFormats.Gray8;
            int width = data.width;
            int height = data.height;
            int stride = (width * pf.BitsPerPixel + 7) / 8;
            byte[] noiseMap = new byte[stride * height];

            float scale = data.scale;
            if (scale <= 0)
            {
                scale = 0.0001f;
            }

            for (int y = 0; y < data.height; y++)
            {
                for (int x = 0; x < width; x++)
                {

                    float amplitude = 1;
                    float frequency = 1;
                    float value = 0;
                    float range = 1;

                    for (int i = 0; i < data.octaves; i++)
                    {
                        value += noiseClass.GetValue(new Vector2(x, y) / scale * frequency) * amplitude;
                        amplitude *= data.persistance;
                        frequency *= data.lacunarity;

                        range += amplitude;
                    }

                    value /= range;

                    noiseMap[y * width + x] = (byte)(value * 255);
                }
            }

            return noiseMap;
        }
    }
}
