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

        public WritableBitmap Bitmap { get; }
        public int Octaves { get; set; }
        public float Lacunarity { get; set; }
        public float Persistance { get; set; }

        public IEnumerable<string> NoiseList { get; set; }

        public ICommand ResetCommand { get; }

        Random _rand;

        public unsafe MainWindowViewModel()
        {
            _rand = new Random();
            Bitmap = new WritableBitmap(100, 100, PixelFormat.Bgra8888);

            ResetCommand = new DelegateCommand(Reset);

            NoiseList = new List<string>()
            {
                "Test1",
                "Test2"
            };

            Reset();
        }

        public unsafe void Reset()
        {
            _rand = new Random();
            using (var buf = Bitmap.Lock())
            {
                var ptr = (uint*)buf.Address;

                var w = Bitmap.PixelWidth;
                var h = Bitmap.PixelHeight;

                // Clear.
                for (var i = 0; i < w * (h - 1); i++)
                {
                    var b = (byte) _rand.Next(0, 255);
                    var g = (byte)_rand.Next(0, 255);
                    var r = (byte)_rand.Next(0, 255);

                    var pixel = b + ((uint)g << 8) + ((uint)r << 16) + ((uint)255 << 24);
                    *(ptr + i) = pixel;
                }

                // Draw bottom line.
                for (var i = w * (h - 1); i < w * h; i++)
                {
                    *(ptr + i) = uint.MaxValue;
                }
            }

            Console.WriteLine("Repaint VM");
            //this.
        }
    }
}
