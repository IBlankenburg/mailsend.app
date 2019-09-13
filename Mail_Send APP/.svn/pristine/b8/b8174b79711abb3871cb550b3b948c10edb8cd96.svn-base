using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MailSend;
using MailsendWPF;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;

namespace MailSendWPF.Server
{
     [InheritedExport]
    public abstract class IServer
    {
        public abstract ulong FillQueue(List<string> dirs, bool recursive, List<string> filters, ulong connections, bool endlessSending, string key);
        public abstract void SetMimeAndSend();
        public abstract void SetKeyManagerAndSend();
        public abstract void AboardThreads();
        public abstract void RequestStop();
        public abstract void SetKeyManager(MailSend.Constants.KeyManager keyManager);

        public delegate void MailsendHandler(MessageWrapper msg, MailState mailState);
        public delegate void RequestSendHandler(MessageWrapper msg, MailState mailState);
        public delegate void RequestSendFailedHandler(MessageWrapper msg, MailState mailState);
        public delegate void StatusMessageHandler(string message, string key);

        public abstract event StatusMessageHandler StatusMessageEvent;
        public abstract event RequestSendHandler RequestSendEvent;
        public abstract event RequestSendFailedHandler RequestSendFailedEvent;

        public abstract event MailsendHandler MailsendEvent;

        public delegate void MaxMailsToSendHandler(MailState mailState);
        public abstract event MaxMailsToSendHandler MaxMailsToSendEvent;

        public delegate void SendMailEndsHandler(MailState sendMailEndsStatus);
        public abstract event SendMailEndsHandler SendMailEndsEvent;

        
        public delegate void BeforeMailsentHandler(MessageWrapper msg, MailState mailState);
        public abstract event BeforeMailsentHandler BeforeMailsentEvent;

        public abstract void OnStatusMessage(string message, string key);
        public abstract void OnMailSent(MessageWrapper mail, MailState mailState);
        public abstract void OnMaxMailsToSend(MailState mailState);
        public abstract void OnSendMailEnds(MailState sendMailEndsStatus);
        public abstract void OnBeforeMailSent(MessageWrapper mail, MailState mailState);
        public abstract void OnRequestSent(MessageWrapper mail, MailState mailState);
        public abstract void OnRequestSentFailed(MessageWrapper mail, MailState mailState);
       
    }
}
