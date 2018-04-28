using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace NoiseGeneratorWPF.ViewModel
{
    class MainWindowVM : INotifyPropertyChanged
    {
        public BitmapSource Bitmap { get; set; }

        private float _scale;
        public float Scale {
            get { return _scale; }
            set {
                if (_scale != value)
                {
                    _scale = value;
                    CreateBitmap();
                    NotifyPropertyChanged("Scale");
                }
            }
        }

        private int _octaves;
        public int Octaves {
            get { return _octaves; }
            set {
                if (_octaves != value)
                {
                    _octaves = value;
                    CreateBitmap();
                    NotifyPropertyChanged("Octaves");
                }
            }
        }

        private float _lacunarity;
        public float Lacunarity {
            get { return _lacunarity; }
            set {
                if (_lacunarity != value)
                {
                    _lacunarity = value;
                    CreateBitmap();
                    NotifyPropertyChanged("Lacunarity");
                }
            }
        }

        private float _persistance;
        public float Persistance {
            get { return _persistance; }
            set {
                if (_persistance != value)
                {
                    _persistance = value;
                    CreateBitmap();
                    NotifyPropertyChanged("Persistance");
                }
            }
        }

        public MainWindowVM()
        {
            _scale = 10;
            _lacunarity = 1;
            _persistance = 0.5f;
            _octaves = 1;

            CreateBitmap();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        int tester = 0;
        public void NotifyPropertyChanged(string propertyName)
        {
            tester++;
            Debug.WriteLine("Kappa" + tester);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void CreateBitmap()
        {
            Debug.WriteLine("Bitmap");
            PixelFormat pf = PixelFormats.Gray8;
            int width = 256;
            int height = 256;
            int stride = (width * pf.BitsPerPixel + 7) / 8;
            

            NoiseData data = new NoiseData()
            {
                width = width,
                height = height,
                lacunarity = Lacunarity,
                persistance = Persistance,
                scale = Scale,
                octaves = Octaves,
                offset = new System.Numerics.Vector2(0, 0)
            };

            IBitmapRenderer renderer = new BitmapRenderer();

            byte[] rawImage = renderer.GenerateNoiseMap(data, new ValueNoise());

            Bitmap = BitmapSource.Create(width, height, 96, 96, pf, null, rawImage, stride);
            NotifyPropertyChanged("Bitmap");
        }
    }
}
