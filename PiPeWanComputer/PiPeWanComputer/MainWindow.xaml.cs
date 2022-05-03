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
            PipeDB.CreateDB();

            var nodes = PipeDB.SelectAllNodes();
            var users = PipeDB.SelectAllUsers();
            var nodedata = PipeDB.SelectAllNodesData();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
            MainWindowViewModel.Dispose();
        }
    }
}
