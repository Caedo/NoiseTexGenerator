﻿using Microsoft.Win32;
using System.IO;
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

            SaveFileDialog dialog = new SaveFileDialog
            {
                Filter = "PNG File (*.png) |*.png"
            };

            if (dialog.ShowDialog() == true)
            {

                using (var stream = File.Create(dialog.FileName))
                {
                    encoder.Save(stream);
                }
            }
        }
    }
}
