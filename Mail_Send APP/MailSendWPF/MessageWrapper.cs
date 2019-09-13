using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MailSend
{
    public class MessageWrapper
    {
        public MessageWrapper(string _messagePath)
        {
            MessagePath = _messagePath;
        }
        private string messagePath = "";

        public string MessagePath
        {
            get { return messagePath; }
            set { messagePath = value; }
        }
        private ulong anzGesendet = 0;

        public ulong AnzGesendet
        {
            get { return anzGesendet; }
            set { anzGesendet = value; }
        }
        //private LumiSoft.Net.Mail.Mail_Message mailMsg = null;
        /*
        public LumiSoft.Net.Mail.Mail_Message MailMsg
        {
            get { return mailMsg; }
            set { mailMsg = value; }
        }
         */ 
        private string mailMsgString = null;

        public string MailMsgString
        {
            get { return mailMsgString; }
            set { mailMsgString = value; }
        }

        private byte[] mailMsgByte = null;

        public byte[] MailMsgByte
        {
            get { return mailMsgByte; }
            set { mailMsgByte = value; }
        }

        private int bodyPosition = 0;

        public int BodyPosition
        {
            get { return bodyPosition; }
            set { bodyPosition = value; }
        }
        private ulong mailID = 0;

        public ulong MailID
        {
            get { return mailID; }
            set { mailID = value; }
        }

        private bool msgCached = false;

        public bool MsgCached
        {
            get { return msgCached; }
            set { msgCached = value; }
        }

        public bool CacheMSG(long maxSizeToCache, long msgSize)
        {
            if (/*mailMsg == null &&*/ String.IsNullOrEmpty(MailMsgString)) return false;
            if (msgSize <= maxSizeToCache)
            {
                msgCached = true;
            }
            else
            {
                msgCached = false;
            }
            return msgCached;
           
        }
        private ulong sendErrors = 0;

        public ulong SendErrors
        {
            get { return sendErrors; }
            set { sendErrors = value; }
        }

        private long msgSize = 0;

        public long MsgSize
        {
            get { return msgSize; }
            set { msgSize = value; }
        }

        public ulong TriedToSend
        {
            get { return anzGesendet + sendErrors; }
        }

        private bool parsed = true;

        public bool Parsed
        {
            get { return parsed; }
            set { parsed = value; }
        }
        private string temp = String.Empty;

        public string Temp
        {
            get { return temp; }
            set { temp = value; }
        }
        private bool corrupt = false;

        public bool Corrupt
        {
            get { return corrupt; }
            set { corrupt = value; }
        }
        private bool sendOnce = false;

        public bool SendOnce
        {
            get { return sendOnce; }
            set { sendOnce = value; }
        }
        private List<Exception> ex = new List<Exception>();

        public List<Exception> Ex
        {
            get { return ex; }
            set { ex = value; }
        }
        private int codePage = -1;

        public int CodePage
        {
            get { return codePage; }
            set { codePage = value; }
        }

        private bool isTestMessage = false;

        public bool IsTestMessage
        {
            get { return isTestMessage; }
            set { isTestMessage = value; }
        }

        

    }
}
