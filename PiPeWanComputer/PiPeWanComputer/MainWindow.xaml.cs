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
using PiPeWanComputer.SQL_Stuff;
using PiPeWanComputer.Helper_Classes;
using MaterialDesignThemes.Wpf;

namespace PiPeWanComputer {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindowViewModel MainWindowViewModel { get; }

        public MainWindow() {
            InitializeComponent();
            MainWindowViewModel = new();
            DataContext = MainWindowViewModel;
        }

        private void toggleTheme(object sender, RoutedEventArgs e) {
            PaletteHelper paletteHelper = new();
            ITheme theme = paletteHelper.GetTheme();

            if (theme.GetBaseTheme() == BaseTheme.Dark) {
                theme.SetBaseTheme(Theme.Light);
            }
            else {
                theme.SetBaseTheme(Theme.Dark);
            }

            paletteHelper.SetTheme(theme);
        }

        private void ExitApp(object sender, RoutedEventArgs e) {
            MainWindowViewModel.Dispose();
            Application.Current.Shutdown();
        }
    }
}
