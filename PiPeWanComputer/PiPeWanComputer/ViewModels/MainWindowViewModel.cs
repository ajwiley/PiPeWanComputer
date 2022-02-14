using LiveCharts;
using PiPeWanComputer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace PiPeWanComputer.ViewModels
{
    public class MainWindowViewModel : ViewModelBase, IDisposable
    {
        private DateTime? _StartTime;
        private readonly Arduino _Arduino;
        private readonly Timer _ClearTimer;
        private readonly Timer MockArduino;

        public MainWindowViewModel()
        {
            _ClearTimer = new Timer()
            {
                AutoReset = false,
                Interval = 120000
            };
            _ClearTimer.Elapsed += (obj, e) =>
            {
                TempGraphViewModel?.TempGraph.Clear();
                FlowGraphViewModel?.FlowGraph.Clear();
            };

            _Arduino = new();
            _StartTime = null;
            TempGraphViewModel = new();
            FlowGraphViewModel = new();

            _Arduino.PortDataChanged += (obj, e) =>
            {
                SparkFunSerialData data = e.Data;

                if (_StartTime == null) { _StartTime = data.Time; }

                var currentTime = (TimeSpan)(data.Time - _StartTime);

                TempGraphViewModel.NextPoint = new TemperatureDatum(data.Temperature, currentTime.TotalSeconds);

                FlowGraphViewModel.NextPoint =  new FlowDatum(data.Flow, currentTime.TotalSeconds);
            };

            _Arduino.ConnectionChanged += (obj, e) =>
            {
                var connected = e.IsConnected.ToString();
                if (connected == "False" && !_ClearTimer.Enabled)
                {
                    _ClearTimer.Start();
                }
                else if (connected == "True" && _ClearTimer.Enabled)
                {
                    _ClearTimer.Stop();
                }
            };


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

                TempGraphViewModel.NextPoint = new TemperatureDatum(mockData.Temperature, currentTime.TotalSeconds);

                FlowGraphViewModel.NextPoint = new FlowDatum(mockData.Flow, currentTime.TotalSeconds);
            };
            MockArduino.Start();
        }

        #region ViewModels

        public TempGraphViewModel TempGraphViewModel { get; }
        public FlowGraphViewModel FlowGraphViewModel { get; }

        #endregion

        public void Dispose()
        {
            _Arduino?.Dispose();
        }
    }
}
