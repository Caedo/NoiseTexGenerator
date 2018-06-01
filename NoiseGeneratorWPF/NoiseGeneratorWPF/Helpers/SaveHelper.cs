using Microsoft.Win32;
using System.IO;
using System.Windows.Media.Imaging;

namespace NoiseGeneratorWPF
{
    /// <summary>
    /// Static class with function helpful with saving bitmap
    /// </summary>
    public static class SaveHelper
    {
        /// <summary>
        /// Save bitmap on disk as .png file
        /// </summary>
        /// <param name="bitmap"><c>BitmapSource</c> that has to be saved on disk</param>
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
