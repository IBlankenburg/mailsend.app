using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MailSend
{
    class AttributeNotSetException:ApplicationException
    {
        public AttributeNotSetException(string message) : base(message) { }
    }
}
