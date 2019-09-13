using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MailSendWPF.Server
{
    class SmtpException : ApplicationException
    {
         public SmtpException(string message) : base(message) { }
         public SmtpException(string message, Exception e) : base(message, e) { }
    }
}