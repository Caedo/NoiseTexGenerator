using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ShaderTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public BitmapSource Img { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            int width = 1024;
            int height = 1024;

            PixelFormat pf = PixelFormats.Gray8;
            int stride = (width * pf.BitsPerPixel + 7) / 8;

            byte[] bytes = new byte[stride * height];

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    bytes[y * width + x] = 0;
                }
            }

            Img = BitmapSource.Create(width, height, 96, 96, pf, null, bytes, stride);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            PngBitmapEncoder encoder = new PngBitmapEncoder();


            RenderTargetBitmap target = new RenderTargetBitmap(1024, 1024, 96, 96, PixelFormats.Pbgra32);
            target.Render(renderImage);
            BitmapFrame frame = BitmapFrame.Create(target);
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
