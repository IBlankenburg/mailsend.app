using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MailSendWPF.Tools
{
    public class MailTypes
    {
        public enum eMailTypes { RootNode, MainHeader, MainBody, Part, PartHeader, PartBody, HeaderNameValue, HeaderName, HeaderValue};
        public enum eHeaderFields { From, To, Subject, Date, MIME_Version, Content_Type, Return_Path, Received, Message_ID, Content_Transfer_Encoding, User_Agent, X_Priority, X_MSMail_Priority, X_Unsent, X_MimeOLE, X_OriginalArrivalTime, X_Mailer  };
        public enum HeaderTypes {ContentType };
        public Dictionary<string, eHeaderFields> headerFields = new Dictionary<string, eHeaderFields>()
        {
            {"Content-Type", eHeaderFields.Content_Type},
            {"From", eHeaderFields.From},
            {"To", eHeaderFields.To},
            {"Subject", eHeaderFields.Subject},
            {"Date", eHeaderFields.Date},
            {"MIME_Version", eHeaderFields.MIME_Version},
            {"Return_Path", eHeaderFields.Return_Path},
            {"Received", eHeaderFields.Received},
            {"Message-ID", eHeaderFields.Message_ID},
            {"Content-Transfer-Encoding", eHeaderFields.Content_Transfer_Encoding},
            {"User-Agent", eHeaderFields.User_Agent},
            {"X-Priority", eHeaderFields.X_Priority},
            {"X-MSMail_Priority", eHeaderFields.X_MSMail_Priority},
            {"X-Unsent", eHeaderFields.X_Unsent},
            {"X-MimeOLE", eHeaderFields.X_MimeOLE},
            {"X-OriginalArrivalTime", eHeaderFields.X_OriginalArrivalTime},
            {"X-Mailer", eHeaderFields.Content_Type},
        };

    }
}
