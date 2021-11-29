using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media;

namespace PiPeWanComputer {
    public class BoundProperties : INotifyPropertyChanged {
        private string _SerialData;
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

        private double _Temperature;
        public double Temperature {
            get => _Temperature;
            set {
                _Temperature = value;
                OnPropertyChanged();
            }
        }

        public BoundProperties() {
            SerialData = "Other thing";
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string PropertyName = "") {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }
    }
}
