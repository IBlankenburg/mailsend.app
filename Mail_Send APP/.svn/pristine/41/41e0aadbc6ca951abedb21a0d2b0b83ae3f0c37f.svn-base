using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using MailSendWPF.Interfaces;

namespace MailSendWPF.Server
{
    public class SelfMAPI:IServer
    {

        public SelfMAPI()
        {
        }

        public override ulong FillQueue(List<string> dirs, bool recursive, List<string> filters, ulong connections, bool endlessSending, string key)
        {
            throw new NotImplementedException();
        }

        public override void SetMimeAndSend()
        {
            throw new NotImplementedException();
        }

        public override void SetKeyManagerAndSend()
        {
            throw new NotImplementedException();
        }

        public override void AboardThreads()
        {
            throw new NotImplementedException();
        }

        public override void RequestStop()
        {
            throw new NotImplementedException();
        }

        public override void SetKeyManager(MailSend.Constants.KeyManager keyManager)
        {
            throw new NotImplementedException();
        }

        public override event IServer.StatusMessageHandler StatusMessageEvent;

        public override event IServer.RequestSendHandler RequestSendEvent;

        public override event IServer.RequestSendFailedHandler RequestSendFailedEvent;

        public override event IServer.MailsendHandler MailsendEvent;

        public override event IServer.MaxMailsToSendHandler MaxMailsToSendEvent;

        public override event IServer.SendMailEndsHandler SendMailEndsEvent;

        public override event IServer.BeforeMailsentHandler BeforeMailsentEvent;

        public override void OnStatusMessage(string message, string key)
        {
            throw new NotImplementedException();
        }

        public override void OnMailSent(MailSend.MessageWrapper mail, MailSend.MailState mailState)
        {
            throw new NotImplementedException();
        }

        public override void OnMaxMailsToSend(MailSend.MailState mailState)
        {
            throw new NotImplementedException();
        }

        public override void OnSendMailEnds(MailSend.MailState sendMailEndsStatus)
        {
            throw new NotImplementedException();
        }

        public override void OnBeforeMailSent(MailSend.MessageWrapper mail, MailSend.MailState mailState)
        {
            throw new NotImplementedException();
        }

        public override void OnRequestSent(MailSend.MessageWrapper mail, MailSend.MailState mailState)
        {
            throw new NotImplementedException();
        }

        public override void OnRequestSentFailed(MailSend.MessageWrapper mail, MailSend.MailState mailState)
        {
            throw new NotImplementedException();
        }
    }
}
