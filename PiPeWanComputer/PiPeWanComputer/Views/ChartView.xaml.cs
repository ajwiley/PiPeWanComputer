using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PiPeWanComputer.Views {
    /// <summary>
    /// Interaction logic for ChartView.xaml
    /// </summary>
    public partial class ChartView : UserControl {
        public Func<double, string> TimeFormatter { get; set; } = value => new DateTime((long)(value)).ToLongTimeString();
        public Func<double, string> ValueFormatter { get; set; } = value => value.ToString("F1");
        public ChartView() {
            InitializeComponent();
            CC1AxisX.LabelFormatter = TimeFormatter;
            CC1AxisY.LabelFormatter = ValueFormatter;
        }
    }
}
