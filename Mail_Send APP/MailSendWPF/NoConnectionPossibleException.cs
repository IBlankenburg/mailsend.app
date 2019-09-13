using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MailSend
{
    class NoConnectionPossibleException:ApplicationException
    {
        public NoConnectionPossibleException(string message) : base(message) { }
        public NoConnectionPossibleException(Exception ex) : base(ex.ToString()) { }
    }
}
