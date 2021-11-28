using System;
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
using System.Net.Sockets;
using System.IO.Ports;
using System.Threading;

namespace PiPeWanComputer {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        private static SerialPort _Port = new();
        private static BoundProperties _BoundProperties = new BoundProperties();

        public MainWindow() {
            InitializeComponent();
            DataContext = _BoundProperties;

            _Port.Dispose();

            try {
                _Port = new SerialPort() {
                    PortName = "COM3",
                    BaudRate = 9600
                };
            }
            catch {
                MessageBox.Show("Could not connect to Arduino", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            try {
                _Port.DataReceived += NewSerialData;
                _Port.Open();
            }
            catch {
                MessageBox.Show("Could not open the port", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void NewSerialData(object sender, SerialDataReceivedEventArgs e) {
            Thread.Sleep(50);
            _BoundProperties.SerialData = _Port.ReadExisting();
            string[] InfoSplit = _BoundProperties.SerialData.Split("\n");
            double Temp = Convert.ToDouble(InfoSplit[0].Split(":")[1].Trim().Trim('F'));
            double Flow = Convert.ToDouble(InfoSplit[1].Split(" ")[0]);
            _BoundProperties.SerialData = "The temperature is: " + Temp + "F\n" + "The flow rate is: " + Flow + " L/H";
        }
    }
}
