using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace NoiseGeneratorWPF
{
    public static class SaveHelper
    {
        public static void SaveBitmap(BitmapSource bitmap)
        {
            PngBitmapEncoder encoder = new PngBitmapEncoder();
            BitmapFrame frame = BitmapFrame.Create(bitmap);
            encoder.Frames.Add(frame);

            using(var stream = File.Create("test.png"))
            {
                encoder.Save(stream);
            }
        }
    }
}
