using System.Windows;

namespace NoiseGeneratorWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// ViewModel
        /// </summary>
        ViewModel.MainWindowVM vm;

        /// <summary>
        /// Standard constructor
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            vm = new ViewModel.MainWindowVM();
            this.DataContext = vm;
        }
    }
}
