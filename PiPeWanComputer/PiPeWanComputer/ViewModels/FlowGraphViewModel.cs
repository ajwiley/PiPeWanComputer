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
        private double _MaxFlow;
        private double _MinFlow;
        private string _CurrentFlow;

        #endregion

        #region Public Properties

        public double MaxFlow { get => _MaxFlow; set => SetProperty(ref _MaxFlow, value); }
        public double MinFlow { get => _MinFlow; set => SetProperty(ref _MinFlow, value); }
        public ChartValues<FlowDatum> FlowGraph { get => _FlowGraph; set => SetProperty(ref _FlowGraph, value); }
        public FlowDatum NextPoint
        {
            set
            {
                FlowGraph.Add(value);
                CurrentFlow = value.Flow.ToString("F2");
                if (value.Flow > MaxFlow)
                {
                    MaxFlow = value.Flow;
                }
                if (value.Flow < MinFlow)
                {
                    MinFlow = value.Flow;
                }
            }
        }
        public string CurrentFlow { get => _CurrentFlow; set => SetProperty(ref _CurrentFlow, value); } 

        #endregion

        public FlowGraphViewModel()
        {
            CurrentFlow = "N/A";
            MinFlow = 0;
            MaxFlow = 1;
            _FlowGraph = new();

            // Tell LiveCharts how to graph a FlowDatum.
            // Time is graphed on the X-axis and flow on the Y-axis.
            var FlowMapper = Mappers.Xy<FlowDatum>().X(x => x.Time).Y(x => x.Flow);
            Charting.For<FlowDatum>(FlowMapper);
        }
    }
}
