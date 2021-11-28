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

        public BoundProperties() {
            SerialData = "Other thing";
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string PropertyName = "") {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }
    }
}
