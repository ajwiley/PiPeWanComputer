using LiveCharts;
using LiveCharts.Defaults;
using PiPeWanComputer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;

namespace PiPeWanComputer.ViewModels
{
    public class MainWindowViewModel : ViewModelBase, IDisposable
    {
        private DateTime? _StartTime;
        private readonly Arduino _Arduino;
        private readonly Timer _ConnectionLostTimer;
        private readonly Timer MockArduino;

        public MainWindowViewModel()
        {
            _ConnectionLostTimer = new Timer()
            {
                AutoReset = false,
                Interval = 120000
            };
            _ConnectionLostTimer.Elapsed += (obj, e) =>
            {
                MessageBox.Show("Lost connection to Node");
            };

            _Arduino = new();
            _StartTime = null;
            TemperatureChartViewModel = new(Type.Temperature);
            FlowChartViewModel = new(Type.Flow);

            _Arduino.PortDataChanged += (obj, e) =>
            {
                SparkFunSerialData data = e.Data;

                if (_StartTime == null) { _StartTime = data.Time; }

                var currentTime = (TimeSpan)(data.Time - _StartTime);

                TemperatureChartViewModel.NextPoint = new ObservablePoint(data.Temperature, currentTime.TotalSeconds);

                FlowChartViewModel.NextPoint =  new ObservablePoint(data.Flow, currentTime.TotalSeconds);
            };
            _Arduino.ConnectionChanged += (obj, e) =>
            {
                bool connected = e.Connected;

                if (!connected && !_ConnectionLostTimer.Enabled)
                {
                    _ConnectionLostTimer.Start();
                }
                else if (connected && _ConnectionLostTimer.Enabled)
                {
                    _ConnectionLostTimer.Stop();
                }
            };

            // Testing Purposes Only
            //_Arduino.Start();
            MockArduino = new Timer()
            {
                AutoReset = true,
                Interval = 500
            };
            MockArduino.Elapsed += (obj, e) =>
            {
                if (_StartTime == null) { _StartTime = DateTime.Now; }

                var rand = new Random();
                var mockTemp = rand.NextDouble() * (80 - (-32)) + (-32);
                var mockFlow = rand.NextDouble() * (500 - 100) + 100;

                var mockData = new SparkFunSerialData(mockTemp, mockFlow);

                var currentTime = (TimeSpan)(DateTime.Now - _StartTime);

                TemperatureChartViewModel.NextPoint = new ObservablePoint(currentTime.TotalSeconds, mockData.Temperature);

                FlowChartViewModel.NextPoint = new ObservablePoint(currentTime.TotalSeconds, mockData.Flow);
            };
            MockArduino.Start();
        }

        #region ViewModels
        public ChartViewModel TemperatureChartViewModel { get; }
        public ChartViewModel FlowChartViewModel { get; }
        #endregion

        public void Dispose()
        {
            _Arduino?.Dispose();
        }
    }
}
