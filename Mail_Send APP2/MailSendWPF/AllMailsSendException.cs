using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MailSend
{
    class AllMailsSendException: ApplicationException
    {
        public AllMailsSendException(string message) : base(message) { }
    }
}
