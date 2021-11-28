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
using System.Net.Sockets;
using System.IO.Ports;


namespace PiPeWanComputer {    
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        private static SerialPort _Port = new();
        
        public MainWindow() {
            InitializeComponent();
            _Port.Dispose();

            try {
                _Port = new SerialPort() {
                    PortName = "COM1",
                    BaudRate = 9600
                };
            }
            catch {
                Console.WriteLine("Could not connect to the arduino");
            }
        }
    }
}
