using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MailSendWPF.UserControls
{
    class StringItem
    {
        public StringItem(string item)
        {
            m_Item = item;
        }
        private String m_Item = String.Empty;

        public String Item
        {
            get { return m_Item; }
            set { m_Item = value; }
        }
    }
}
