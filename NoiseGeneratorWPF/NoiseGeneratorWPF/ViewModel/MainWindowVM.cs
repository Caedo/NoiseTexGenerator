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
using System.Windows.Input;

namespace NoiseGeneratorWPF.ViewModel
{
    class MainWindowVM : INotifyPropertyChanged
    {
        public WriteableBitmap Bitmap { get; set; }

        private float _scale;
        public float Scale {
            get { return _scale; }
            set {
                if (_scale != value)
                {
                    _scale = value;
                    if (AutoUpdate)
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
                    //Stopwatch watch = Stopwatch.StartNew();
                    //watch.Start();

                    _octaves = value;
                    if (AutoUpdate)
                        CreateBitmap();
                    //watch.Stop();
                    //Debug.WriteLine(watch.ElapsedMilliseconds);
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
                    if (AutoUpdate)
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
                    if (AutoUpdate)
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
                    if (AutoUpdate)
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
                    if (AutoUpdate)
                        CreateBitmap();
                    NotifyPropertyChanged("OffsetY");
                }
            }
        }

        private bool _turbulance;
        public bool Turbulance {
            get { return _turbulance; }
            set {
                if (_turbulance != value)
                {
                    _turbulance = value;
                    if (AutoUpdate)
                        CreateBitmap();
                    NotifyPropertyChanged("Turbulance");
                }
            }
        }

        private bool _autoUpdate;
        public bool AutoUpdate {
            get { return _autoUpdate; }
            set {
                if (_autoUpdate != value)
                {
                    _autoUpdate = value;
                    if (AutoUpdate)
                        CreateBitmap();
                    NotifyPropertyChanged("AutoUpdate");
                }
            }
        }

        private int _width;
        public int Width {
            get { return _width; }
            set {
                if (_width != value)
                {
                    _width = value;
                    if (AutoUpdate)
                        CreateBitmap();
                    NotifyPropertyChanged("Width");
                }
            }
        }

        private int _height;
        public int Height {
            get { return _height; }
            set {
                if (_height != value)
                {
                    _height = value;
                    if (AutoUpdate)
                        CreateBitmap();
                    NotifyPropertyChanged("Height");
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

        public ICommand RefreshCommand { get; set; }
        public ICommand SaveCommand { get; set; }

        private Dictionary<string, INoise> _noiseDictionary;
        private IBitmapRenderer _renderer;


        public MainWindowVM()
        {
            _scale = 10;
            _lacunarity = 1;
            _persistance = 0.5f;
            _octaves = 1;
            _autoUpdate = true;
            _width = 1024;
            _height = 1024;


            _noiseDictionary = NoiseHelper.GetNoiseDictionary();
            NoiseTypes = new ObservableCollection<string>(_noiseDictionary.Select(n => n.Key));
            _selectedNoiseType = NoiseTypes[0];

            RefreshCommand = new RelayCommand(() => CreateBitmap());
            SaveCommand = new RelayCommand(() => SaveHelper.SaveBitmap(Bitmap));
            _renderer = new BitmapRenderer();

            CreateBitmap();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void CreateBitmap()
        {

            //Debug.WriteLine("Bitmap");
            PixelFormat pf = PixelFormats.Gray8;
            int stride = (Width * pf.BitsPerPixel + 7) / 8;


            NoiseData data = new NoiseData()
            {
                width = Width,
                height = Height,
                stride = stride,
                lacunarity = Lacunarity,
                persistance = Persistance,
                scale = Scale,
                octaves = Octaves,
                offset = new System.Numerics.Vector2(OffsetX, OffsetY),
                turbulance = Turbulance
            };


            //Bitmap = BitmapSource.Create(Width, Height, 96, 96, pf, null, rawImage, stride);
            if (Bitmap == null)
            {
                Bitmap = new WriteableBitmap(Width, Height, 96, 96, pf, null);
            }

            if (Bitmap.PixelWidth != Width || Bitmap.PixelHeight != Height)
            {
                Bitmap = new WriteableBitmap(Width, Height, 96, 96, pf, null);
            }
            
            byte[] rawImage = _renderer.GenerateNoiseMap(data, _noiseDictionary[SelectedNoiseType]);

            
            Stopwatch watch = Stopwatch.StartNew();
            watch.Start();
            Bitmap.Lock();
            unsafe
            {
                byte* pBackBuffer = (byte*)Bitmap.BackBuffer;
                var w = Bitmap.PixelWidth;
                var h = Bitmap.PixelHeight;

                for (int y = 0; y < h; y++)
                {
                    for (int x = 0; x < w; x++)
                    {
                        int ptrOffset = x + w * y;
                        *(pBackBuffer + ptrOffset) = rawImage[ptrOffset];
                    }
                }
            }
            Bitmap.AddDirtyRect(new System.Windows.Int32Rect(0, 0, Bitmap.PixelWidth, Bitmap.PixelWidth));
            Bitmap.Unlock();
            
            NotifyPropertyChanged("Bitmap");

            watch.Stop();
            Debug.WriteLine(watch.ElapsedMilliseconds);
        }

        public void Save()
        {
            SaveHelper.SaveBitmap(Bitmap);
        }
    }
}
