using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MailSend;

namespace MailSendWPF.Tools
{
    public class MailTree<T>:MailNode<T>
    {
        public MailNode<object> headerNode = null;
        public MailNode<object> bodyNode = null;
        
        public MailTree() { }
        //public MailTree() { }
        
        
    }
}
