using System;
using Avalonia;
using Avalonia.Logging.Serilog;
using NoiseGenerator.ViewModels;
using NoiseGenerator.Views;

namespace NoiseGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            BuildAvaloniaApp().Start<MainWindow>(() => new MainWindowViewModel());
        }

        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .UseReactiveUI()
                .LogToDebug();
    }
}
