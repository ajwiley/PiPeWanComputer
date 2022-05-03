using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net;

namespace PiPeWanComputer.Helper_Classes {
    public class Email {
        #region "Static variables"

        private static readonly string Password = "Pipew@n1";
        public static readonly string Username = "pipewanwsu@gmail.com";
        public static readonly string From = Username;
        private static readonly string Host = "smtp.gmail.com";
        private static readonly int Port = 587;

        #endregion

        #region "Properties"

        public string To { get; set; } = "";
        public string Subject { get; set; } = "";
        public string Body { get; set; } = "";

        #endregion

        public Email() { }

        /// <summary>
        /// Send a warning to the building owner
        /// </summary>
        public void SendWarning(string To, double WaterTemp, double FlowRate) {
            Subject = "Warning! Failure imminent! " + DateTime.Now.ToString("MM/dd/yyyy hh:mm tt");

            Body = $"Dear, {To}"
                + "\n\nIt appears that your pipe is going to leak soon."
                + $"\nThe current water temperature is: {WaterTemp}F"
                + $"\nThe current flow rate is: {FlowRate}(units)"
                + "\nWe reccomend that you turn off your water ASAP!"
                + "\n\nSincerely,\nPiPeWan";

            Send(To, Subject, Body);
        }

        public static void Send(string To, string Subject, string Body) {
            var Client = new SmtpClient(Host, Port) {
                Credentials = new NetworkCredential(Username, Password),
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false
            };

            MailMessage Message = new();
            Message.From = new(From);
            Message.To.Add(To);
            Message.Subject = Subject;
            Message.Body = Body;

            Client.Send(From, To, Subject, Body);
        }
    }
}
