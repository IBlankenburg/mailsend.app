using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MailSendWPF
{
    class TestMail
    {
        public static string TestHeaderFrom = "From: \"Pilot\" <pilot@internal-dom2.local>\r\n";
        public static string TestHeaderTo = "To: \"Pilot\" <pilot@internal-dom2.local>\r\n";
        public static string TestHeader1 = "Subject: MailSend TestMail\r\n";
        //public static string SubjectEncodingStart = "Subject: =?iso-8859-1?B?";
        //public static string SubjectEncodingEnd = "=?=\r\n";
        public static string TestHeader2 = "MIME-Version: 1.0\r\n";
        public static string TestHeader3 = "Content-Type: text/plain; \r\n format=flowed; \r\n	charset=\"utf-8\"; \r\n	reply-type=original\r\n";
        public static string TestHeader4 = "Content-Transfer-Encoding: 8bit\r\n";
        public static string TestMailNewLine = "\r\n";

        public static string TestBody = "This is a MailSend Testmail";

        public static string WholeTestMail = TestHeaderFrom + TestHeaderTo + TestHeader1 + TestHeader2 + TestHeader3 + TestHeader4 + TestMailNewLine + TestBody;
        /*
        public static string CreateMail()
        {
            //byte[] testbyte = Encoding.UTF8.GetBytes(TestHeader1);
            
            

        }
         */ 
    }
}
