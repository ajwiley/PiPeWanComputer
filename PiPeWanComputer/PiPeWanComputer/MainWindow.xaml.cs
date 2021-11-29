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

namespace PiPeWanComputer {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        private static SerialPort _Port = new();
        private static BoundProperties _BoundProperties = new BoundProperties();
        private static DispatcherTimer GraphTimer = new DispatcherTimer();
        private DateTime StartTime = DateTime.Now;

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

            //Map backend values to the frontend for the temperature graph
            var TempMapper = Mappers.Xy<TemperatureGraph>().X(x => x.Time).Y(x => x.Temp);
            Charting.For<TemperatureGraph>(TempMapper);

            //Map backend values to the frontend for the flow graph
            var FlowMapper = Mappers.Xy<FlowGraph>().X(x => x.Time).Y(x => x.Flow);
            Charting.For<FlowGraph>(FlowMapper);

            GraphTimer.Interval = TimeSpan.FromSeconds(5);
            GraphTimer.Tick += UpdateTempGraph;
            GraphTimer.Start();
        }

        /// <summary>
        /// Update the temperature graph with the newest values
        /// </summary>
        private void UpdateTempGraph(object? sender, EventArgs e) {
            double CurrentTime = (DateTime.Now - StartTime).TotalSeconds;
            _BoundProperties.TempGraph.Add(new TemperatureGraph(_BoundProperties.Temperature, CurrentTime));
            _BoundProperties.FlowGraph.Add(new FlowGraph(_BoundProperties.FlowRate, CurrentTime));
        }

        /// <summary>
        /// Serial 
        /// Receive data from the SparkFun Pro RF
        /// </summary>
        private static void NewSerialData(object sender, SerialDataReceivedEventArgs e) {
            Thread.Sleep(50);
            _BoundProperties.SerialData = _Port.ReadExisting(); // Read in the info

            // Parse the information
            string[] InfoSplit = _BoundProperties.SerialData.Split("\n");
            double Temp = Convert.ToDouble(InfoSplit[0].Split(":")[1].Trim().Trim('F'));
            _BoundProperties.Temperature = Temp;
            double Flow = Convert.ToDouble(InfoSplit[1].Split(" ")[0]);
            _BoundProperties.FlowRate = Flow;
        }
    }
}
