using LiveCharts;
using LiveCharts.Defaults;
using PiPeWanComputer.Helper_Classes;
using PiPeWanComputer.SQL_Stuff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PiPeWanComputer.ViewModels {
    public class ChartViewModel : ViewModelBase {

        #region Private Fields
        ChartValues<ObservablePoint> _ChartValues;
        string _CurrentY;
        double _MinX;
        string _LastClicked;
        #endregion

        #region Public Properties
        public string YAxisTitle { get; }
        public string XAxisTitle { get; }
        public ChartValues<ObservablePoint> ChartValues { get => _ChartValues; set => SetProperty(ref _ChartValues, value); }
        public string CurrentY { get => _CurrentY; set => SetProperty(ref _CurrentY, value); }
        public ObservablePoint NextPoint {
            set {
                ChartValues.Add(value);
                CurrentY = value.Y.ToString("F2");
            }
        }
        public double MinX { get => _MinX; set => SetProperty(ref _MinX, value); }
        #endregion

        public ICommand UpdateChartRange { get; private set; }

        public ChartViewModel(Type type, List<NodeData> nodeDatas) {
            List<ObservablePoint> observablePoints = new();

            if (type.Equals(Type.Temperature)) {
                YAxisTitle = "Temperature (F)";

                // TESTING ONLY
                /*{
                    observablePoints.Add(new ObservablePoint(637872988690000000, nodeDatas[0].Temperature));
                    observablePoints.Add(new ObservablePoint(637872988770000000, nodeDatas[0].Temperature + 1));
                    observablePoints.Add(new ObservablePoint(637872988810000000, nodeDatas[0].Temperature + 2));
                    observablePoints.Add(new ObservablePoint(637872988860000000, 1000));
                }*/

                foreach (var nd in nodeDatas) {
                    observablePoints.Add(new ObservablePoint(nd.TimeStamp.Ticks, nd.Temperature));
                }
            }
            else if (type.Equals(Type.Flow)) {
                YAxisTitle = "Flow (ml/hr)";

                foreach (var nd in nodeDatas) {
                    observablePoints.Add(new ObservablePoint(nd.TimeStamp.Ticks, nd.Flow));
                }
            }
            ChartValues = new(observablePoints);

            XAxisTitle = "Time (Seconds)";
            CurrentY = "0";
            _LastClicked = "All";

            // All
            // First() Works because SelectNodeData.sql sorts the NodeData in ascending order so the oldest nodedata is added first
            MinX = ChartValues.Count == 0 ? 0 : ChartValues.First().X; //MinX = ChartValues.Min(o => o.X);

            UpdateChartRange = new RelayCommand(UpdateChartRange_Execute, UpdateChartRange_CanExecute);
        }

        private bool UpdateChartRange_CanExecute(object obj) => obj as string != _LastClicked;

        private void UpdateChartRange_Execute(object obj) {
            string content = obj as string;
            if (content != null) {
                _LastClicked = content;
                switch (content) {
                    case "All":
                        // First() Works because SelectNodeData.sql sorts the NodeData in ascending order so the oldest nodedata is added first
                        MinX = ChartValues.Count == 0 ? 0 : ChartValues.First().X; //MinX = ChartValues.Min(o => o.X);
                        break;
                    case "Hour":
                        MinX = DateTime.Now.Ticks - 36000000000;
                        break;
                    case "Minute":
                        MinX = DateTime.Now.Ticks - 600000000;
                        break;
                }
            }
        }
    }
}
