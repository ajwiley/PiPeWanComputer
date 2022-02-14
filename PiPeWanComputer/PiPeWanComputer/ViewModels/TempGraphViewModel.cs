using LiveCharts;
using LiveCharts.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PiPeWanComputer.ViewModels
{
    public class TempGraphViewModel : ViewModelBase
    {
        #region Private Fields

        private ChartValues<TemperatureDatum> _TempGraph;
        private double _MaxTemperature;
        private double _MinTemperature;
        private string _CurrentTemp;

        #endregion

        #region Public Properties

        public ChartValues<TemperatureDatum> TempGraph { get => _TempGraph; set => SetProperty(ref _TempGraph, value); }
        public double MaxTemperature { get => _MaxTemperature; set => SetProperty(ref _MaxTemperature, value); }
        public double MinTemperature { get => _MinTemperature; set => SetProperty(ref _MinTemperature, value); }

        public TemperatureDatum NextPoint { 
            set
            {
                TempGraph.Add(value);
                CurrentTemp = value.Temperature.ToString("F2");
                if (value.Temperature > MaxTemperature)
                {
                    MaxTemperature = value.Temperature;
                }
                if (value.Temperature < MinTemperature)
                {
                    MinTemperature = value.Temperature;
                }
            } 
        }
        public string CurrentTemp { get => _CurrentTemp; set => SetProperty(ref _CurrentTemp, value); }

        #endregion

        public TempGraphViewModel()
        {
            CurrentTemp = "N/A";
            MaxTemperature = 0;
            MaxTemperature = 1;
            _TempGraph = new();

            // Tell LiveCharts how to graph a TemperatureDatum.
            // Time is graphed on the X-axis and temperature on the Y-axis.
            var TemperatureMapper = Mappers.Xy<TemperatureDatum>().X(x => x.Time).Y(x => x.Temperature);
            Charting.For<TemperatureDatum>(TemperatureMapper);
        }


    }
}
