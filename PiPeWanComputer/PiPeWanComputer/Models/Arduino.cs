using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace PiPeWanComputer.Models {
    public class Arduino : IDisposable {
        #region Fields
        private const string _SparkFun_VID = "1B4F";
        private const string _SparkFun_PID = "3ABA";
        private SerialPort? _Port;
        private bool _Connected;
        private System.Timers.Timer _TryConnect;
        #endregion

        #region EventHandlers
        public event EventHandler<ConnectionChangedEventArgs>? ConnectionChanged;
        public event EventHandler<PortDataChangedEventArgs>? PortDataChanged;
        #endregion

        public Arduino() {
            _TryConnect = new() {
                AutoReset = false,
                Interval = 5000
            };
            _TryConnect.Elapsed += _TryConnect_Elapsed;
        }

        #region Events
        private void _TryConnect_Elapsed(object? sender, ElapsedEventArgs e) {
            _Port?.Dispose();

            try {
                _Port = new SerialPort() {
                    PortName = ComPortNames(_SparkFun_VID, _SparkFun_PID),
                    BaudRate = 9600
                };
                _Port.Open();
                _Port.DataReceived += Port_DataReceived;
                _Port.ErrorReceived += Port_ErrorReceived;
                SetConnected(true);
            }
            catch {
                _TryConnect.Start();
                SetConnected(false);
            }
        }

        private void Port_ErrorReceived(object sender, SerialErrorReceivedEventArgs e) {
            SetConnected(false);
            if (_TryConnect.Enabled == false) {
                _TryConnect.Start();
            }
        }

        private void Port_DataReceived(object sender, SerialDataReceivedEventArgs e) {
            // Wait arbitrary time for the full message to arrive in the serial buffer
            Thread.Sleep(50);

            string? serialData = _Port?.ReadExisting();

            if (String.IsNullOrEmpty(serialData)) { return; }

            // Parse the information
            string[] infoSplit = serialData.Split(",");

            // Get the temperature
            if (double.TryParse(infoSplit[0].Trim(), out double temperature)) {
                // Get the flow
                if (double.TryParse(infoSplit[1].Trim(), out double flow)) {
                    var portData = new SparkFunSerialData(temperature, flow);
                    PortDataChanged?.Invoke(this, new PortDataChangedEventArgs(portData));
                }
            }
        }
        #endregion

        #region Methods
        public void Start() {
            try {
                _Port = new SerialPort() {
                    PortName = ComPortNames(_SparkFun_VID, _SparkFun_PID),
                    BaudRate = 9600
                };
                _Port.Open();
                _Port.DataReceived += Port_DataReceived;
                _Port.ErrorReceived += Port_ErrorReceived;
                SetConnected(true);
            }
            catch {
                _TryConnect.Start();
                SetConnected(false);
            }
        }

        /// <summary>
        /// Check if Connected needs to be updated and invokes ConnectionChanged if needed
        /// </summary>
        /// <param name="value"></param>
        private void SetConnected(bool value) {
            if (_Connected != value) {
                _Connected = value;
                ConnectionChanged?.Invoke(this, new ConnectionChangedEventArgs(_Connected));
            }
        }

        /// <summary>
        /// Get the COM port for the SparkFun Pro RF
        /// </summary>
        private static string ComPortNames(string VID, string PID) {
            // Information we are looking for from the com port
            string pattern = string.Format("^VID_{0}.PID_{1}", VID, PID);
            Regex _RegexPattern = new(pattern, RegexOptions.IgnoreCase);

            List<string> comports = new();
            RegistryKey RegisterKey1 = Registry.LocalMachine;
            RegistryKey RegisterKey2 = RegisterKey1.OpenSubKey("SYSTEM\\CurrentControlSet\\Enum");

            // Go through each registry
            foreach (string SubKey3 in RegisterKey2.GetSubKeyNames()) {
                RegistryKey RegisterKey3 = RegisterKey2.OpenSubKey(SubKey3);

                // Find a match of the registry with what we have
                foreach (string s in RegisterKey3.GetSubKeyNames()) {
                    if (_RegexPattern.Match(s).Success) {
                        RegistryKey RegisterKey4 = RegisterKey3.OpenSubKey(s);

                        foreach (string s2 in RegisterKey4.GetSubKeyNames()) {
                            RegistryKey RegisterKey5 = RegisterKey4.OpenSubKey(s2);
                            RegistryKey RegisterKey6 = RegisterKey5.OpenSubKey("Device Parameters");
                            comports.Add((string)RegisterKey6.GetValue("PortName"));
                        }
                    }
                }
            }

            // Find the COM port currently in use
            if (comports.Count > 0) {
                foreach (string str in SerialPort.GetPortNames()) {
                    if (comports.Contains(str)) {
                        return str;
                    }
                }
            }

            return "";
        }
        #endregion

        public void Dispose() {
            _TryConnect?.Dispose();
            _Port?.Dispose();
        }
    }


    public class ConnectionChangedEventArgs : EventArgs {
        public bool Connected { get; }

        public ConnectionChangedEventArgs(bool connected) {
            Connected = connected;
        }
    }

    public class PortDataChangedEventArgs : EventArgs {
        public SparkFunSerialData Data { get; }

        public PortDataChangedEventArgs(SparkFunSerialData data) {
            Data = data;
        }
    }
}
