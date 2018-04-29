﻿using System;
using System.Collections.Generic;
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

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            vm.CreateBitmap();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            vm.Save();
        }
    }
}
