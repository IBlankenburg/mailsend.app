using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MailSendWPF.UserControls
{
    public class LogMessage
    {
        private string message = String.Empty;

        public string Message
        {
            get { return message; }
            set { message = value; }
        }
        public LogMessage(string message)
        {
            this.Message = message;
        }
    }
}
