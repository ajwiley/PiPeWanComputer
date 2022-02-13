using LiveCharts;
using LiveCharts.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PiPeWanComputer.ViewModels
{
    public class FlowGraphViewModel : ViewModelBase
    {
        #region Private Fields

        private ChartValues<FlowDatum> _FlowGraph;

        #endregion

        #region Public Properties

        public ChartValues<FlowDatum> FlowGraph { get => _FlowGraph; set => SetProperty(ref _FlowGraph, value); }

        #endregion

        public FlowGraphViewModel()
        {
            _FlowGraph = new();

            // Tell LiveCharts how to graph a FlowDatum point.
            // Time is graphed on the X-axis and flow on the Y-axis.
            var TemperatureMapper = Mappers.Xy<FlowDatum>().X(x => x.Time).Y(x => x.Flow);
            Charting.For<TemperatureDatum>(TemperatureMapper);
        }
    }
}
