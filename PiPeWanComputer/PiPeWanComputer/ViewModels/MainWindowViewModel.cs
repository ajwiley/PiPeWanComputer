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
using PiPeWanComputer.Helper_Classes;
using PiPeWanComputer.SQL_Stuff;

namespace PiPeWanComputer.ViewModels
{
    public class MainWindowViewModel : ViewModelBase, IDisposable
    {
        private readonly Arduino _Arduino;
        private readonly Timer _ConnectionLostTimer;
        private DateTime LastSent;
        private bool SentWarning = false;

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

            List<NodeData> nodeDatas = PipeDB.SelectNodeData(1);
            TemperatureChartViewModel = new(Type.Temperature, nodeDatas);
            FlowChartViewModel = new(Type.Flow, nodeDatas);

            _Arduino.PortDataChanged += (obj, e) =>
            {
                SparkFunSerialData data = e.Data;

                TemperatureChartViewModel.NextPoint = new ObservablePoint(DateTime.Now.Ticks, data.Temperature);

                FlowChartViewModel.NextPoint =  new ObservablePoint(DateTime.Now.Ticks, data.Flow);

                if (data.Flow <= 1 && SentWarning == false) {
                    if (EmailAron) {
                        Email.SendWarning("awiley.dev@gmail.com", data.Temperature, data.Flow);
                    }
                    if (EmailAlex) {
                        Email.SendWarning("rossillonalex@gmail.com", data.Temperature, data.Flow);
                    }
                    if (emailEdgar) {
                        Email.SendWarning("", data.Temperature, data.Flow);
                    }
                    if (emailMo) {
                        Email.SendWarning("", data.Temperature, data.Flow);
                    }

                    SentWarning = true;
                }
                else if (data.Flow >= 5) {
                    SentWarning = false;
                }
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

            _Arduino.Start();
        }

        #region ViewModels
        public ChartViewModel TemperatureChartViewModel { get; }
        public ChartViewModel FlowChartViewModel { get; }
        #endregion

        public void Dispose()
        {
            _Arduino?.Dispose();
        }

        private bool emailAron;
        public bool EmailAron { get => emailAron; set => SetProperty(ref emailAron, value); }

        private bool emailAlex;
        public bool EmailAlex { get => emailAlex; set => SetProperty(ref emailAlex, value); }

        private bool emailEdgar;
        public bool EmailEdgar { get => emailEdgar; set => SetProperty(ref emailEdgar, value); }

        private bool emailMo;
        public bool EmailMo { get => emailMo; set => SetProperty(ref emailMo, value); }
    }
}
