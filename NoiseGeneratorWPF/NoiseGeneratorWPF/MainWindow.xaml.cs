using System.Windows;

namespace NoiseGeneratorWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ViewModel.MainWindowVM vm;
        public MainWindow()
        {
            InitializeComponent();
            vm = new ViewModel.MainWindowVM();
            this.DataContext = vm;
        }
    }
}
