using Avalonia.Controls;
using Avalonia.Media.Imaging;
using System;
using System.Collections.Generic;
using System.Text;
using SixLabors.ImageSharp;
using Avalonia.Platform;
using System.Windows.Input;
using ReactiveUI;

namespace NoiseGenerator.ViewModels
{
    public class MainWindowViewModel : ReactiveObject
    {
        private WritableBitmap _bitmap;
        public WritableBitmap Bitmap {
            get { return _bitmap; }
            set {
                this.RaiseAndSetIfChanged(ref _bitmap, value);
            }
        }

        private float _frequency;
        public float Frequency {
            get { return _frequency; }
            set {
                Reset();
                this.RaiseAndSetIfChanged(ref _frequency, value);
            }
        }

        private float _scale;
        public float Scale {
            get { return _scale; }
            set {
                if (value != 0)
                {
                    Reset();
                    this.RaiseAndSetIfChanged(ref _scale, value);
                }
            }
        }

        private int _octaves;
        public int Octaves {
            get { return _octaves; }
            set {
                Reset();
                this.RaiseAndSetIfChanged(ref _octaves, value);
            }
        }

        private float _lacunarity;
        public float Lacunarity {
            get { return _lacunarity; }
            set {
                Reset();
                this.RaiseAndSetIfChanged(ref _lacunarity, value);
            }
        }

        private float _persistance;
        public float Persistance {
            get { return _persistance; }
            set {
                Reset();
                this.RaiseAndSetIfChanged(ref _persistance, value);
            }
        }

        public IEnumerable<string> NoiseList { get; set; }

        public ICommand ResetCommand { get; }

        Random _rand;

        public unsafe MainWindowViewModel()
        {
            _rand = new Random();
            ResetCommand = new DelegateCommand(Reset);

            NoiseList = new List<string>()
            {
                "Test1",
                "Test2"
            };

            Octaves = 1;
            Lacunarity = 1;
            Persistance = 0;
            Frequency = 1;
            Scale = 1;

            Reset();
        }

        public unsafe void Reset()
        {
            WritableBitmap dummy = new WritableBitmap(512, 512, PixelFormat.Bgra8888);
            _rand = new Random();
            float offsetX = _rand.Next(-100000, 100000);
            float offsetY = _rand.Next(-100000, 100000);

            using (var buf = dummy.Lock())
            {
                var ptr = (uint*)buf.Address;

                var w = dummy.PixelWidth;
                var h = dummy.PixelHeight;

                for (int y = 0; y < w; y++)
                {
                    for (int x = 0; x < h; x++)
                    {
                        float sampleX = x / Scale * Frequency + offsetX;
                        float sampleY = y / Scale * Frequency + offsetY;

                        float noiseValue = ValueNoise.GetValue(sampleX, sampleY) * 255;
                        var pixel = (uint)noiseValue + ((uint)noiseValue << 8) + ((uint)noiseValue << 16) + ((uint)255 << 24);

                        int ptrOffset = x + w * y;
                        *(ptr + ptrOffset) = pixel;
                    }
                }

                // Clear.
                //for (var i = 0; i < w * h; i++)
                //{
                //    float value = ValueNoise.GetValue(i, i * 2) * 255;

                //    var b = (byte)value;
                //    var g = (byte)value;
                //    var r = (byte)value;

                //    var pixel = b + ((uint)g << 8) + ((uint)r << 16) + ((uint)255 << 24);
                //    *(ptr + i) = pixel;
                //}
            }
            //Console.WriteLine("Repaint");
            Bitmap = dummy;  
        }
    }
}
