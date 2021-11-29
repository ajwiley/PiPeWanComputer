using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using LiveCharts;

namespace PiPeWanComputer {
    public class BoundProperties : INotifyPropertyChanged {
        #region "Serial information"

        // The COM port for the SparkFun Pro RF
        private string _RFIDComPort = "";
        public string RFIDComPort {
            get => _RFIDComPort;
            set {
                _RFIDComPort = value;
                OnPropertyChanged();
            }
        }


        // Information we get from the SparkFun Pro RF
        private string _SerialData = "";
        public string SerialData {
            get => _SerialData;
            set {
                _SerialData = value;
                OnPropertyChanged();
            }
        }

        // Temperature from SparkFun
        private double _Temperature;
        public double Temperature {
            get => _Temperature;
            set {
                _Temperature = value;
                Temp = value.ToString("F2");
                OnPropertyChanged();
            }
        }

        // Temperature we want to display (formatted nicely)
        private string _Temp = "";
        public string Temp {
            get => _Temp;
            set {
                _Temp = value;
                OnPropertyChanged();
            }
        }

        // Flow from SparkFun
        private double _FlowRate;
        public double FlowRate {
            get => _FlowRate;
            set {
                _FlowRate = value;
                Flow = value.ToString("F2");
                OnPropertyChanged();
            }
        }

        // Flow we want to display (formatted nicely)
        private string _Flow = "";
        public string Flow {
            get => _Flow;
            set {
                _Flow = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region "LiveCharts graph data"

        // Temperature graph values
        private ChartValues<TemperatureGraph> _TempGraph = new();
        public ChartValues<TemperatureGraph> TempGraph {
            get => _TempGraph;
            set {
                _TempGraph = value;
                OnPropertyChanged();
            }
        }

        // Flow graph values
        private ChartValues<FlowGraph> _FlowGraph = new();
        public ChartValues<FlowGraph> FlowGraph {
            get => _FlowGraph;
            set {
                _FlowGraph = value;
                OnPropertyChanged();
            }
        }

        #endregion

        public BoundProperties() { }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string PropertyName = "") {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }
    }
}
