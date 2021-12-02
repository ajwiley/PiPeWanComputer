using System.Windows;

namespace PiPeWanComputer {
    /// <summary>
    /// Information to pass into a MessageBox thread
    /// </summary>
    public class MessageInfo {
        public string Message { get; set; }
        public string Header { get; set; }
        public MessageBoxButton Button { get; set; }
        public MessageBoxImage Image { get; set; }

        public MessageInfo(string m, string h, MessageBoxButton b, MessageBoxImage i) {
            Message = m;
            Header = h;
            Button = b;
            Image = i;
        }
    }
}
