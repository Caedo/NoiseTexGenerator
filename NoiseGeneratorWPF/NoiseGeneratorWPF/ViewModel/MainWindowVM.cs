using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Input;
using System.Windows;

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
                    _octaves = value;
                    if (AutoUpdate)
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

        private bool _turbulence;
        public bool Turbulence {
            get { return _turbulence; }
            set {
                if (_turbulence != value)
                {
                    _turbulence = value;
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
        BackgroundWorker _worker;
        NoiseData data;

        PixelFormat _pf = PixelFormats.Gray8;


        public MainWindowVM()
        {
            _scale = 10;
            _lacunarity = 1;
            _persistance = 0.5f;
            _octaves = 1;
            _autoUpdate = true;
            _width = 256;
            _height = 256;


            _noiseDictionary = NoiseHelper.GetNoiseDictionary();
            NoiseTypes = new ObservableCollection<string>(_noiseDictionary.Select(n => n.Key));
            _selectedNoiseType = NoiseTypes[0];

            RefreshCommand = new RelayCommand(() => CreateBitmap());
            SaveCommand = new RelayCommand(() => SaveHelper.SaveBitmap(Bitmap));
            _renderer = new BitmapRenderer();

            _worker = new BackgroundWorker()
            {
                WorkerSupportsCancellation = true

            };
            _worker.DoWork += StartGenerate;
            _worker.RunWorkerCompleted += UpdateBitmap;

            Bitmap = new WriteableBitmap(Width, Height, 96, 96, _pf, null);

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
            int stride = (Width * _pf.BitsPerPixel + 7) / 8;

            data = new NoiseData()
            {
                width = Width,
                height = Height,
                stride = stride,
                lacunarity = Lacunarity,
                persistance = Persistance,
                scale = Scale,
                octaves = Octaves,
                offset = new System.Numerics.Vector2(OffsetX, OffsetY),
                turbulance = Turbulence
            };

            if (Bitmap.PixelWidth != Width || Bitmap.PixelHeight != Height)
            {
                Bitmap = new WriteableBitmap(Width, Height, 96, 96, _pf, null);
            }

            if (_worker.IsBusy)
            {
                _worker.CancelAsync();
                _worker = new BackgroundWorker()
                {
                    WorkerSupportsCancellation = true
                };
                _worker.DoWork += StartGenerate;
                _worker.RunWorkerCompleted += UpdateBitmap;
            }
            _worker.RunWorkerAsync();
        }

        void StartGenerate(object sender, DoWorkEventArgs e)
        {
            e.Result = _renderer.GenerateNoiseMap(data, _noiseDictionary[SelectedNoiseType]);
        }

        void UpdateBitmap(object sender, RunWorkerCompletedEventArgs e)
        {
            byte[] rawImage = (byte[])e.Result;
            Bitmap.Lock();
            Int32Rect rect = new Int32Rect(0, 0, Width, Height);
            Bitmap.WritePixels(rect, rawImage, data.stride, 0);
            Bitmap.AddDirtyRect(rect);
            Bitmap.Unlock();
            NotifyPropertyChanged("Bitmap");
        }

        public void Save()
        {
            SaveHelper.SaveBitmap(Bitmap);
        }
    }
}
