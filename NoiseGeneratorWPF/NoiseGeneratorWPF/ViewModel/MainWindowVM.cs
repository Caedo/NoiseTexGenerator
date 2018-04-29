using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Reflection;

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

        private float _offsetX;
        public float OffsetX {
            get { return _offsetX; }
            set {
                if (_offsetX != value)
                {
                    _offsetX = value;
                    CreateBitmap();
                    NotifyPropertyChanged("OffsetX");
                }
            }
        }

        private float _offsetY;
        public float OffsetY {
            get { return _offsetY; }
            set {
                if (_offsetY != value)
                {
                    _offsetY = value;
                    CreateBitmap();
                    NotifyPropertyChanged("OffsetY");
                }
            }
        }

        public ObservableCollection<string> NoiseTypes { get; set; }

        private string _selectedNoiseType;
        public string SelectedNoiseType {
            get { return _selectedNoiseType; }
            set {
                if (_selectedNoiseType != value)
                {
                    _selectedNoiseType = value;
                    CreateBitmap();
                    NotifyPropertyChanged("SelectedNoiseType");
                }
            }
        }

        private Dictionary<string, INoise> _noiseDictionary;

        public MainWindowVM()
        {
            _scale = 10;
            _lacunarity = 1;
            _persistance = 0.5f;
            _octaves = 1;


            _noiseDictionary = NoiseHelper.GetNoiseDictionary();
            NoiseTypes = new ObservableCollection<string>(_noiseDictionary.Select(n => n.Key));
            SelectedNoiseType = NoiseTypes[0];

            CreateBitmap();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            Debug.WriteLine(SelectedNoiseType);
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
                stride = stride,
                lacunarity = Lacunarity,
                persistance = Persistance,
                scale = Scale,
                octaves = Octaves,
                offset = new System.Numerics.Vector2(OffsetX, OffsetY)
            };

            IBitmapRenderer renderer = new BitmapRenderer();

            byte[] rawImage = renderer.GenerateNoiseMap(data, _noiseDictionary[SelectedNoiseType]);

            Bitmap = BitmapSource.Create(width, height, 96, 96, pf, null, rawImage, stride);
            NotifyPropertyChanged("Bitmap");
        }

        public void Save()
        {
            SaveHelper.SaveBitmap(Bitmap);
        }
    }
}
