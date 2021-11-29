using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PiPeWanComputer {
    public class BoundProperties : INotifyPropertyChanged {
        private string _SerialData = "";
        public string SerialData {
            get => _SerialData;
            set {
                _SerialData = value;
                OnPropertyChanged();
            }
        }

        private LiveCharts.ChartValues<TemperatureGraph> _TempGraph = new LiveCharts.ChartValues<TemperatureGraph>();
        public LiveCharts.ChartValues<TemperatureGraph> TempGraph {
            get => _TempGraph;
            set {
                _TempGraph = value;
                OnPropertyChanged();
            }
        }

        private LiveCharts.ChartValues<FlowGraph> _FlowGraph = new LiveCharts.ChartValues<FlowGraph>();
        public LiveCharts.ChartValues<FlowGraph> FlowGraph {
            get => _FlowGraph;
            set {
                _FlowGraph = value;
                OnPropertyChanged();
            }
        }

        private double _Temperature;
        public double Temperature {
            get => _Temperature;
            set {
                _Temperature = value;
                Temp = value.ToString("F2");
                OnPropertyChanged();
            }
        }

        private string _Temp = "";
        public string Temp {
            get => _Temp;
            set {
                _Temp = value;
                OnPropertyChanged();
            }
        }

        private double _FlowRate;
        public double FlowRate {
            get => _FlowRate;
            set {
                _FlowRate = value;
                Flow = value.ToString("F2");
                OnPropertyChanged();
            }
        }

        private string _Flow = "";
        public string Flow {
            get => _Flow;
            set {
                _Flow = value;
                OnPropertyChanged();
            }
        }

        public BoundProperties() { }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string PropertyName = "") {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }
    }
}
