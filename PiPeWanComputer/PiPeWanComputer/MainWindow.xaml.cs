using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.IO.Ports;
using System.Threading;
using LiveCharts;
using LiveCharts.Configurations;
using System.Diagnostics;
using System.Windows.Threading;
using System.Text.RegularExpressions;
using Microsoft.Win32;
using PiPeWanComputer.ViewModels;
using PiPeWanComputer.Models;

namespace PiPeWanComputer {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        private readonly MainWindowViewModel _MainWindowViewModel;

        public MainWindow() {
            InitializeComponent();
            _MainWindowViewModel = new();
            DataContext = _MainWindowViewModel;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _MainWindowViewModel.Dispose();
        }
    }
}
