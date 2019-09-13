using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MailSendWPF.Server
{
    using System;

    internal class ReplyConstants
    {
        private ReplyConstants()
        { }

        public static readonly string SYSTEM_STATUS = "211";
        public static readonly string HELP_MSG = "214";
        public static readonly string HELO_REPLY = "220";
        public static readonly string QUIT = "221";
        public static readonly string AUTH_SUCCESSFUL = "235";
        public static readonly string OK = "250";
        public static readonly string NOT_LOCAL_WILL_FORWARD = "251";
        public static readonly string SERVER_CHALLENGE = "334";
        public static readonly string START_INPUT = "354";
        public static readonly string SERVICE_NOT_AVAILABLE = "421";
        public static readonly string MAILBOX_BUSY = "450";
        public static readonly string ERROR_PROCESSING = "451";
        public static readonly string INSUFFICIENT_STORAGE = "452";
        public static readonly string UNKNOWN = "500";
        public static readonly string SYNTAX_ERROR = "501";
        public static readonly string CMD_NOT_IMPLEMENTED = "502";
        public static readonly string BAD_SEQUENCE = "503";
        public static readonly string NOT_IMPLEMENTED = "504";
        public static readonly string SECURITY_ERROR = "505";
        public static readonly string ACTION_NOT_TAKEN = "550";
        public static readonly string NOT_LOCAL_PLEASE_FORWARD = "551";
        public static readonly string EXCEEDED_STORAGE_ALLOWANCE = "552";
        public static readonly string MAILBOX_NAME_NOT_ALLOWED = "553";
        public static readonly string TRANSACTION_FAILED = "554";

        public static readonly string PIPELINING = "PIPELINING";

    }
}
