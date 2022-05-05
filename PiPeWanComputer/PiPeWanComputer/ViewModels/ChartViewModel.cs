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

        #region Fields
        string _XAxisTitle;
        ChartValues<ObservablePoint> _ChartValues;
        double _MinY;
        double _MaxY;
        string _Current;
        string _LastClicked;
        #endregion

        #region Public Properties
        public string YAxisTitle { get; }
        /// <summary>
        /// The X-Axis will always be time, but its units should change to fit the scale
        /// </summary>
        public string XAxisTitle { get => _XAxisTitle; set => SetProperty(ref _XAxisTitle, value); }
        public ChartValues<ObservablePoint> ChartValues { get => _ChartValues; set => SetProperty(ref _ChartValues, value); }
        public double MinY { get => _MinY; set => SetProperty(ref _MinY, value); }
        public double MaxY { get => _MaxY; set => SetProperty(ref _MaxY, value); }
        public string CurrentY { get => _Current; set => SetProperty(ref _Current, value); }
        public ObservablePoint NextPoint
        {
            set {
                ChartValues.Add(value);
                CurrentY = value.Y.ToString("F2");
                MaxY = value.Y > MaxY ? value.Y : MaxY;
                MinY = value.Y < MinY ? value.Y : MinY;
            }
        }
        #endregion

        public ICommand UpdateChartRange { get; private set; }

        public ChartViewModel(Type type, List<NodeData> nodeDatas)
        {
            List<ObservablePoint> observablePoints = new();

            if (type.Equals(Type.Temperature))
            {
                YAxisTitle = "Temperature (F)";

                foreach (var nd in nodeDatas) {
                    observablePoints.Add(new ObservablePoint(nd.TimeStamp.Ticks, nd.Temperature));
                }
            }
            else if (type.Equals(Type.Flow))
            {
                YAxisTitle = "Flow (ml/hr)";
                
                foreach (var nd in nodeDatas) {
                    observablePoints.Add(new ObservablePoint(nd.TimeStamp.Ticks, nd.Flow));
                }               
            }

            ChartValues = new(observablePoints);

            _XAxisTitle = "Time (Seconds)";
            _Current = "0";
            _MinY = 0;
            _MaxY = 1;
            _LastClicked = "";
                        

            // Using constructor without canExecute because I want it to always be true (and therefore the buttons always enabled).
            // Defining an anonymous action parameter because this code won't be used anywhere else.
            UpdateChartRange = new RelayCommand(obj => {
                switch (obj as string)
                {
                    case "All":
                        // TODO set Min to the oldest record
                        break;
                    case "YTD":
                        Console.WriteLine("YTD");
                        break;
                    case "Month":
                        Console.WriteLine("Month");
                        break;
                    case "Week":
                        Console.WriteLine("Week");
                        break;
                    case "Day":
                        Console.WriteLine("Day");
                        break;
                }
            });

            // TODO: Create a CanExecute function which returns false if the same button was the last one selected
            UpdateChartRange = new RelayCommand(UpdateChartRange_Execute, UpdateChartRange_CanExecute);
        }

        private bool UpdateChartRange_CanExecute(object obj)
        {
            return obj as string != _LastClicked;
        }

        private void UpdateChartRange_Execute(object obj)
        {
            string content = obj as string;
            if (content != null)
            {
                _LastClicked = content;
                switch (content)
                {
                    case "All":
                        // TODO set Min to the oldest record
                        break;
                    case "YTD":
                        Console.WriteLine("YTD");
                        break;
                    case "Month":
                        Console.WriteLine("Month");
                        break;
                    case "Week":
                        Console.WriteLine("Week");
                        break;
                    case "Day":
                        Console.WriteLine("Day");
                        break;
                }
            }
        }
    }
}
