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

        #endregion

        #region Public Properties

        public ChartValues<TemperatureDatum> TempGraph { get => _TempGraph; set => SetProperty(ref _TempGraph, value); }

        #endregion

        public TempGraphViewModel()
        {
            _TempGraph = new();
            // Tell LiveCharts how to graph a TemperatureDatum point.
            // Time is graphed on the X-axis and temperature on the Y-axis.
            var TemperatureMapper = Mappers.Xy<TemperatureDatum>().X(x => x.Time).Y(x => x.Temperature);
            Charting.For<TemperatureDatum>(TemperatureMapper);
        }
    }
}
