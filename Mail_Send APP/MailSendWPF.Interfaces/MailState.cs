using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MailSend
{
    public class MailState
    {
        private ulong m_wholeMailsToSend = 0;

        public ulong WholeMailsToSend
        {
            get { return m_wholeMailsToSend; }
            set { m_wholeMailsToSend = value; }
        }
        private ulong m_roundOfMailsToSend = 1;

        public ulong RoundOfMailsToSend
        {
            get { return m_roundOfMailsToSend; }
            set { m_roundOfMailsToSend = value; }
        }
        private ulong m_countWholeMailsSend = 0;

        public ulong CountWholeMailsSend
        {
            get { return m_countWholeMailsSend; }
            set { m_countWholeMailsSend = value; }
        }
        private ulong m_errorsWhileSendingMails = 0;

        public ulong ErrorsWhileSendingMails
        {
            get { return m_errorsWhileSendingMails; }
            set { m_errorsWhileSendingMails = value; }
        }

        //Anzahl der erfolgreich gesendeten Mails
        public ulong MailsSent
        {
            get { return CountWholeMailsSend - ErrorsWhileSendingMails; }
        }
        public static List<MessageWrapper> errorMails = new List<MessageWrapper>();

        private string id = String.Empty;

        public string Id
        {
            get { return id; }
            set { id = value; }
        }
        private bool endlessSending = false;

        public bool EndlessSending
        {
            get { return endlessSending; }
            set { endlessSending = value; }
        }

        private List<Exception> ex = new List<Exception>();

        public List<Exception> Ex
        {
            get { return ex; }
            set { ex = value; }
        }

    }
}
