using System;
namespace MailSendWPF
{
    interface IClient
    {
        void AboardThreads();
        string AddHeaderUnparsed(string sHeader, MailSend.MessageWrapper msg, bool deleteMessageID = true);
        string AddHeaderUnparsedObsolete(string sMsg);
        MailSend.MessageWrapper AddMailAttributes(MailSend.MessageWrapper msg);
        void AddToTxtFileStr(string fileName, string str, bool atTop = false, bool fDel = false, bool replace = false, string oldValue = null, string newValue = null, bool caseSensitive = false);
        string AddToTxtString(string sMailMessage, string str, bool atTop = false, bool fDel = false, bool replace = false, string oldValue = null, string newValue = null, bool caseSensitive = false);
        void Connect(object threadContext);
        void CountWholeMailToSendPlus1();
        ulong FillQueue(System.Collections.Generic.List<string> dirs, bool recursive, System.Collections.Generic.List<string> filters, ulong connections, bool endlessSending);
        ulong GetCountWholeMailToSend();
        string GetFileContents(string file_name);
        MailSend.MessageWrapper GetMessage();
        bool IsMessageToSend(MailSend.MessageWrapper msg);
        bool IsMessageToSendWithoutCount(MailSend.MessageWrapper msg);
        System.Collections.Generic.Queue<MailSend.MessageWrapper> Mail_Queue { get; set; }
        //event MailSend.Client.MailsendHandler MailsendEvent;
        //event MailSend.Client.MaxMailsToSendHandler MaxMailsToSendEvent;
        bool ParseMessage(MailSend.MessageWrapper msg);
        string ReplaceMessageID(string sMailMessage, string oldValue, string newValue, bool caseSensitive = false);
        void RequestStop();
        void SendingErrorHandling(ref MailSend.MessageWrapper msg, Exception ex, object threadContext, ref bool sendError, ref int countTriesToSendMail, System.IO.Stream msgStream);
        void SendingErrorHandlingNew(ref MailSend.MessageWrapper msg, Exception ex, object threadContext, System.IO.Stream msgStream);
        void SendingErrorHandlingWithRetrie(ref MailSend.MessageWrapper msg, Exception ex, object threadContext, ref bool sendError, ref int countTriesToSendMail, int triesToSendMail, System.IO.Stream msgStream);
        //event MailSend.Client.SendMailEndsHandler SendMailEndsEvent;
        void SendMessage(object threadContext);
        void SendMessageNew(object threadContext);
        void SetMimeAndSend();
        void StartSend(object threadContext);
        void StartSendNew(object threadContext);
    }
}
