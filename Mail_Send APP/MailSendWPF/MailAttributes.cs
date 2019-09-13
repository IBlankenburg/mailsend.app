using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MailSend
{
   public class MailAttributes
    {
        public enum HeaderPosition {begin, end, none};
        public enum AttributeInsertMethod {addOnce,addAlways, change, extent};
        public struct SSubject
        {
            public HeaderPosition position;
            public string subjectstring;
        }

        //Some headers are added only one time, other headers adding everytime
        public struct SHeader
        {
            public string name;
            public string value;
            public AttributeInsertMethod addHeaderOnce;
            public HeaderPosition position;
        }

        private List<SHeader> m_headers = new List<SHeader>();

        public List<SHeader> Headers
        {
            get { return m_headers; }
            set { m_headers = value; }
        }
        private SSubject m_subject = new SSubject();

        public SSubject Subject
        {
            get { return m_subject; }
            set { m_subject = value; }
        } 
    }
}
