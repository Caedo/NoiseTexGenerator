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
    /// <summary>
    /// ViewModel for <c>MainWindow</c>. Contains all data visible on UI as well as logic for creating Bitmap. 
    /// Implements <c>INotifyPropertyChanged</c> interface.
    /// </summary>
    class MainWindowVM : INotifyPropertyChanged
    {
        /// <summary>
        /// Bitmap that is visible on the MainWindow.
        /// </summary>
        public WriteableBitmap Bitmap { get; set; }

        private float _scale;
        /// <summary>
        /// Scale of the noise function.
        /// </summary>
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
        /// <summary>
        /// Number of Octaves of fBm algorithm.
        /// </summary>
        public int Octaves {
            get { return _octaves; }
            set {
                if (_octaves != value)
                {
                    _octaves = MathHelper.Clamp(value, 1, 6);
                    if (AutoUpdate)
                        CreateBitmap();
                    NotifyPropertyChanged("Octaves");


                }
            }
        }

        private float _lacunarity;
        /// <summary>
        /// Lacunarity value of the fBm algorithm.
        /// </summary>
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
        /// <summary>
        /// Persistance value of the fBm algorithm.
        /// </summary>
        public float Persistance {
            get { return _persistance; }
            set {
                if (_persistance != value)
                {
                    _persistance = MathHelper.Clamp(value, 0f, 1f);
                    if (AutoUpdate)
                        CreateBitmap();
                    NotifyPropertyChanged("Persistance");
                }
            }
        }

        private float _offsetX;
        /// <summary>
        /// X value of the offset of the noise function.
        /// </summary>
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
        /// <summary>
        /// Y value of the offset of the noise function.
        /// </summary>
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
        /// <summary>
        /// Use turbulence method?
        /// </summary>
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
        /// <summary>
        /// Should bitmap update automaticaly when any property changed?
        /// </summary>
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
        /// <summary>
        /// With of the bitmap.
        /// </summary>
        public int Width {
            get { return _width; }
            set {
                if (_width != value)
                {
                    _width = MathHelper.Clamp(value, 1, 4096);
                    if (AutoUpdate)
                        CreateBitmap();
                    NotifyPropertyChanged("Width");
                }
            }
        }

        private int _height;
        /// <summary>
        /// Height of the bitmap.
        /// </summary>
        public int Height {
            get { return _height; }
            set {
                if (_height != value)
                {
                    _height = MathHelper.Clamp(value, 1, 4096);
                    if (AutoUpdate)
                        CreateBitmap();
                    NotifyPropertyChanged("Height");
                }
            }
        }

        /// <summary>
        /// Collection of names of noise classes. 
        /// </summary>
        public ObservableCollection<string> NoiseTypes { get; set; }

        private string _selectedNoiseType;
        /// <summary>
        /// Currently selected noise type.
        /// </summary>
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

        /// <summary>
        /// Refresh bitmap command.
        /// </summary>
        public ICommand RefreshCommand { get; set; }
        /// <summary>
        /// Save bitmap on disc command.
        /// </summary>
        public ICommand SaveCommand { get; set; }

        /// <summary>
        /// Dictionary with noise name as key and instance of INoise as value
        /// </summary>
        private Dictionary<string, INoise> _noiseDictionary;
        /// <summary>
        /// Instance of IBitmapRenderer 
        /// </summary>
        private IBitmapRenderer _renderer;
        /// <summary>
        /// BackgroundWorker used for threading
        /// </summary>
        BackgroundWorker _worker;

        /// <summary>
        /// NoiseData struct
        /// </summary>
        NoiseData _data;

        /// <summary>
        /// Current pixelformat used for creating bitmap
        /// </summary>
        PixelFormat _pf = PixelFormats.Gray8;

        /// <summary>
        /// Event that raises when property is changed. Implemented from <c>INotifyPropertyChanged</c> interface.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindowVM"/> class.
        /// </summary>
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

        /// <summary>
        /// Invokation of the PropertyChange event.
        /// </summary>
        /// <param name="propertyName">Name of property that was changed</param>
        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Create noise bitmap with currently used parameters.
        /// </summary>
        private void CreateBitmap()
        {
            //Debug.WriteLine("Bitmap");
            int stride = (Width * _pf.BitsPerPixel + 7) / 8;

            _data = new NoiseData()
            {
                width = Width,
                height = Height,
                stride = stride,
                lacunarity = Lacunarity,
                persistance = Persistance,
                scale = Scale,
                octaves = Octaves,
                offset = new System.Numerics.Vector2(OffsetX, OffsetY),
                turbulence = Turbulence
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

        /// <summary>
        /// Start generating bitmap on another thread. Called from Background Worker.
        /// </summary>
        /// <param name="sender">Object that calls this method.</param>
        /// <param name="e">Event arguments.</param>
        private void StartGenerate(object sender, DoWorkEventArgs e)
        {
            e.Result = _renderer.GenerateNoiseMap(_data, _noiseDictionary[SelectedNoiseType]);
        }


        /// <summary>
        /// Generate bitmap with result of the algorithm. Called from Background Worker.
        /// </summary>
        /// <param name="sender">Object that calls this method.</param>
        /// <param name="e">Event arguments.</param>
        private void UpdateBitmap(object sender, RunWorkerCompletedEventArgs e)
        {
            byte[] rawImage = (byte[])e.Result;
            Bitmap.Lock();
            Int32Rect rect = new Int32Rect(0, 0, Width, Height);
            Bitmap.WritePixels(rect, rawImage, _data.stride, 0);
            Bitmap.AddDirtyRect(rect);
            Bitmap.Unlock();
            NotifyPropertyChanged("Bitmap");
        }

        /// <summary>
        /// Save bitmap on disc.
        /// </summary>
        private void Save()
        {
            SaveHelper.SaveBitmap(Bitmap);
        }
    }
}
