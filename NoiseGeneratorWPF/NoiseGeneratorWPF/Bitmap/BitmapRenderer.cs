using System;
using System.Numerics;
using System.Diagnostics;

namespace NoiseGeneratorWPF
{
    /// <summary>
    /// Class for creating Bitmaps, implements <c>IBitmapRenderer</c> interface
    /// </summary>
    class BitmapRenderer : IBitmapRenderer
    {

        /// <summary>
        /// Create bitmap array of noise values
        /// </summary>
        /// <param name="data"><c>NoiseData</c> struct with data about noise generation</param>
        /// <param name="noiseObject"><c>INoise</c> interface that provides noise function</param>
        /// <returns>Returns array of bytes with pixel values</returns>
        public byte[] GenerateNoiseMap(NoiseData data, INoise noiseObject)
        {
#if DEBUG
            Stopwatch watch = Stopwatch.StartNew();
            watch.Start();
#endif
            int stride = data.stride;
            byte[] noiseMap = new byte[stride * data.height];

            float[] floatMap = new float[stride * data.height];



            float scale = data.scale;
            float halfWidth = scale / 2f;
            float halfHeight = scale / 2f;

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

                    float sampleX = MathHelper.Lerp(0, scale, (float)(x) / data.width) - halfWidth;
                    float sampleY = MathHelper.Lerp(0, scale, (float)(y) / data.height) - halfHeight;

                    for (int i = 0; i < data.octaves; i++)
                    {

                        //System.Diagnostics.Debug.WriteLine($"{sampleX} {sampleY}");

                        float noiseValue = noiseObject.GetValue((new Vector2(sampleX, sampleY)) * frequency + data.offset);

                        if (data.turbulence)
                        {
                            noiseValue = Math.Abs(noiseValue);
                        }

                        value += noiseValue * amplitude;
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
                    //noiseMap[y * data.width + x] = (byte)((value * 0.5f + 0.5f) * 255);
                }

            }
            //System.Diagnostics.Debug.WriteLine($"Min: {min} Max: {max}");
            for (int y = 0; y < data.height; y++)
            {
                for (int x = 0; x < data.width; x++)
                {
                    int pos = y * data.width + x;
                    noiseMap[pos] = (byte)(MathHelper.InverseLerp(min, max, floatMap[pos]) * 255);
                }
            }
#if DEBUG
            watch.Stop();
            Debug.WriteLine(watch.ElapsedMilliseconds);
#endif
            return noiseMap;
        }
    }
}
