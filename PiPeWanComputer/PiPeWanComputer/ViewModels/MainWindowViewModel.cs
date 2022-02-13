using LiveCharts;
using PiPeWanComputer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PiPeWanComputer.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private DateTime? _StartTime;
        private readonly Arduino _Arduino;
        private string _Connected;

        public string IsConnected { get => _Connected; set => SetProperty(ref _Connected, value); }


        public MainWindowViewModel(Arduino auduino)
        {
            _Arduino = auduino;
            _StartTime = null;
            TempGraphViewModel = new();
            FlowGraphViewModel = new();

            _Arduino.PortDataChanged += (obj, e) =>
            {
                if (_StartTime == null) { _StartTime = DateTime.Now; }

                SparkFunSerialData data = e.Data;

                var currentTime = (TimeSpan)(DateTime.Now - _StartTime);

                TempGraphViewModel.TempGraph.Add(
                    new TemperatureDatum(data.Temperature, currentTime.TotalSeconds)
                );

                FlowGraphViewModel.FlowGraph.Add(
                    new FlowDatum(data.Flow, currentTime.TotalSeconds)
                );
            };

            _Arduino.ConnectionChanged += (obj, e) => IsConnected = e.IsConnected.ToString();
        }

        #region ViewModels

        public TempGraphViewModel TempGraphViewModel { get; }
        public FlowGraphViewModel FlowGraphViewModel { get; }

        #endregion
    }
}
