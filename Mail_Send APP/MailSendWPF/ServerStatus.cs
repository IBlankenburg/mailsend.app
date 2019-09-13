using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MailSend;
using MailSendWPF.Server;
using MailSendWPF.Interfaces;
using MailSendWPF.Configuration;

namespace MailSendWPF
{
    class ServerStatus
    {
        private bool run;

        public bool Run
        {
            get { return run; }
            set { run = value; }
        }

        private string guid = String.Empty;

        public string Guid
        {
            get { return guid; }
            set { guid = value; }
        }


        private ServerSchema server = null;

        public ServerSchema Server
        {
            get { return server; }
            set { server = value; }
        }
        private IServer serverImpl = null;

        public IServer ServerImpl
        {
            get { return serverImpl; }
            set { serverImpl = value; }
        }
        private DateTime startTime;

        public DateTime StartTime
        {
            get { return startTime; }
            set { startTime = value; }
        }
        private DateTime endTime;

        public DateTime EndTime
        {
            get { return endTime; }
            set { endTime = value; }
        }
    }
}
