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
using LiveCharts;
using LiveCharts.Configurations;
using System.Diagnostics;

namespace PiPeWanComputer {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        private static SerialPort _Port = new();
        private static BoundProperties _BoundProperties = new BoundProperties();
        private Thread TempThread;
        Stopwatch StopWatch = new();
        bool RunGraph = true;
        private DateTime start = DateTime.Now;

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

            //lets instead plot elapsed milliseconds and value
            var mapper = Mappers.Xy<TemperatureGraph>().X(x => x.Time).Y(x => x.Temp);
            //save the mapper globally         
            Charting.For<TemperatureGraph>(mapper);

            TempThread = new Thread(UpdateTempGraph);
            TempThread.Start();
        }

        /// <summary>
        /// Update the temperature graph with the newest values
        /// </summary>
        private void UpdateTempGraph() {
            StopWatch.Start();
            while (RunGraph) {
                Thread.Sleep(1000);
                double time = (DateTime.Now - start).TotalSeconds;
                _BoundProperties.TempGraph.Add(new TemperatureGraph(_BoundProperties.Temperature, time));
            }
            StopWatch.Stop();
        }

        Func<double, string> formatFunc = (x) => String.Format("{0.00}", x);

        /// <summary>
        /// Serial 
        /// Receive data from the SparkFun Pro RF
        /// </summary>
        private void NewSerialData(object sender, SerialDataReceivedEventArgs e) {
            Thread.Sleep(50);
            _BoundProperties.SerialData = _Port.ReadExisting(); // Read in the info

            // Parse the information
            string[] InfoSplit = _BoundProperties.SerialData.Split("\n");
            double Temp = Convert.ToDouble(InfoSplit[0].Split(":")[1].Trim().Trim('F'));
            _BoundProperties.Temperature = Temp;
            double Flow = Convert.ToDouble(InfoSplit[1].Split(" ")[0]);

            // Output the info
            _BoundProperties.SerialData = "The temperature is: " + Temp + "F\n" + "The flow rate is: " + Flow + " L/H";
        }

        /// <summary>
        /// Stop threads when closing the program
        /// </summary>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
            RunGraph = false;
            TempThread.Join();
        }
    }
}
