using Avalonia;
using Avalonia.Controls;
using Avalonia.Diagnostics;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using NoiseGenerator.ViewModels;
using System;
using System.Linq;

namespace NoiseGenerator.Views
{
    public class MainWindow : Window
    {

        Avalonia.Controls.Image _noiseIm;
        Button _resetButton;

        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif

        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);

            _noiseIm = this.Get<Image>("NoiseImage");
            _resetButton = this.Get<Button>("ResetButton");

            _resetButton.Click += RepaintImage;

            Console.WriteLine("Start");
        }

        void RepaintImage(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Repaint");
            //_noiseIm.InvalidateVisual();
        }

        private void Image_PointerMoved(object sender, PointerEventArgs e)
        {

        }
    }
}
