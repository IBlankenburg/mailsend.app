using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MailSend
{
    class QueueEmptyException:ApplicationException
    {
        public QueueEmptyException(string message):base(message)
        {
        }

    }
}
