using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using LumiSoft.Net.Mime;
using LumiSoft.Net.MIME;
using LumiSoft.Net.Mail;
using LumiSoft.Net.SMTP.Client;
using System.Threading;
using System.Net;
using LumiSoft.Net;
using href.Utils;
using System.Runtime.Serialization.Formatters.Binary;
using MailSendWPF.Server;

namespace MailSend
{
    class Client:IServer, MailSendWPF.IClient
    {

        private SMTP_Client[] m_smtpClient = null;
        private bool m_running = false;
        private bool m_useSmartHost = true;
        private IPAddress m_senderIPAddress = null;
        private Queue<MessageWrapper> mail_Queue = new Queue<MessageWrapper>();
        private Dictionary<ulong, ulong> synchroniseCountMails = null;
        private object semSychroniseCountMails = new object();
        private Thread[] mailThreads = null;
        //private IPAddress[] m_smartHostIPAddresses = null;
        private IPAddress m_smartHostIPAddress = null;
        private int m_smartHostPort = 25;
        private bool m_smartHostUseSSL = false;
        private string m_host = "";
        private string m_smartHostName = "";
        private string m_smartHostPassword = "";
        private string m_smtpFrom = "";
        private string[] m_smtpTo;
        private bool m_endlessSending = false;
        private MailState m_mailState = new MailState();
        private bool m_anonymousAuthentication = false;
        private string m_Id = String.Empty;
        private bool m_sendOriginal = false;
        private bool m_newMessageID = false;
        private bool m_addRecipientToMail = false;
        private bool m_addSenderToMail = false;
        private bool m_addHeader = false;
        private bool m_codePageEnabled = false;
        private int m_codePage = 0;

        private MailAttributes m_mailAttributes = new MailAttributes();
        private object m_dequeueLock = new object();
        private object semOnMailSend = new object();
        bool m_recursive = false;
        ManualResetEvent[] doneEvents = null;

        private int m_numberConnections = 1;
        private long m_cacheMailsUntilInByte = 50000;

        //Zeit die zwischen dem senden zweier Mails gewartet werden muss
        private int m_waitBetweenMailsms = 0;


        private Object sendLock = new Object();
        private Object countLock = new Object();

        private bool m_parsed = true;
        private bool m_fallBack = true;
        //private Exception m_connectException = new Exception();

        //Stopping MailSending
        private volatile bool _shouldStop;

        //RecipientGroup
        private bool m_useRecipientGroup = false;
        private string m_recipientGroup = String.Empty;
        private int m_recipientGroupStart = 0;
        private int m_recipientGroupEnd = 0;
        private List<string> m_recipientsGroupList = null;
        private int m_recipientGroupIndex = 0;
        private Object m_recipientGroupIndexLock = new Object();
        private bool m_subjectMailCount = false;

        //MailSendEvent
        //public delegate void MailsendHandler(MessageWrapper msg, MailState mailState);
        override public event MailsendHandler MailsendEvent;
        override public void OnMailSent(MessageWrapper mail, MailState mailState)
        {
            MailsendHandler myEvent = MailsendEvent;
            if (myEvent != null)
            {
                myEvent(mail, mailState);
            }
        }

        //public delegate void MaxMailsToSendHandler(MailState mailState);
        override public event MaxMailsToSendHandler MaxMailsToSendEvent;
        override public void OnMaxMailsToSend(MailState mailState)
        {
            MaxMailsToSendHandler myEvent = MaxMailsToSendEvent;
            if (myEvent != null)
            {
                myEvent(mailState);
            }
        }

        //public delegate void SendMailEndsHandler(MailState sendMailEndsStatus);
        override public event SendMailEndsHandler SendMailEndsEvent;
        override public void OnSendMailEnds(MailState sendMailEndsStatus)
        {
            SendMailEndsHandler myEvent = SendMailEndsEvent;
            if (myEvent != null)
            {
                myEvent(sendMailEndsStatus);
            }
        }

        //public delegate void BeforeMailsentHandler(MessageWrapper msg, MailState mailState);
        override public event BeforeMailsentHandler BeforeMailsentEvent;
        override public void OnBeforeMailSent(MessageWrapper mail, MailState mailState)
        {
            BeforeMailsentHandler myEvent = BeforeMailsentEvent;
            if (myEvent != null)
            {
                myEvent(mail, mailState);
            }
        }

        public Client()
        {
            //this.m_senderIPAddress = IPAddress.Any;
        }

        public Client(IPAddress smartHostIPAddress, int port, string smtpFrom, string smtpTo, string smartHostName, string smartHostPassword, ulong rounds, int connections, bool endlessSending)
        {
            this.m_senderIPAddress = IPAddress.Any;
            //this.m_senderIPAddress = IPAddress.Parse(@"172.30.200.130");
            m_useSmartHost = true;
            //m_smartHostIPAddresses = new IPAddress[m_numberConnections];
            m_smartHostIPAddress = smartHostIPAddress;
            m_smartHostPort = port;
            m_smtpFrom = smtpFrom;
            string[] myParams = { ",", ";" };
            string[] recipients = smtpTo.Split(myParams, StringSplitOptions.RemoveEmptyEntries);
            m_smtpTo = recipients;
            m_smartHostName = smartHostName;
            m_smartHostPassword = smartHostPassword;
            m_mailState.RoundOfMailsToSend = rounds;
            m_numberConnections = connections;
            m_endlessSending = endlessSending;
            //m_anonymousAuthentication = true;
            if ((String.IsNullOrEmpty(smartHostName) || smartHostPassword == null))
            {
                m_anonymousAuthentication = true;
            }


        }
        public Client(IPAddress smartHostIPAddress, int port, string smtpFrom, string smtpTo, string smartHostName, string smartHostPassword, ulong rounds, int connections, bool endlessSending, MailAttributes mailAttributes, bool parsed, int waitBetweenMailsms, bool sendOriginal, bool newMessageID, bool fallBack, string id, bool addSenderToMail, bool addRecipientToMail, bool anonymousAuth, string host, long cacheMailsUntilInByte, bool codePageEnabled, int codePage, string recipientGroup, int recipientGroupStart, int recipientGroupEnd, string recipientGroupDomain, bool subjectMailCount)
        {
            if (smartHostIPAddress == null && String.IsNullOrEmpty(host))
            {
                throw new ApplicationException("No Host set");
            }
            this.m_senderIPAddress = IPAddress.Any;
            //this.m_senderIPAddress = IPAddress.Parse(@"172.30.200.130");

            //m_smartHostIPAddresses = new IPAddress[m_numberConnections];
            if (smartHostIPAddress != null)
            {
                m_smartHostIPAddress = smartHostIPAddress;

            }
            else
            {
                m_smartHostIPAddress = null;
                m_host = host;
            }
            m_useSmartHost = true;
            m_smartHostPort = port;
            m_smtpFrom = smtpFrom;
            string[] myParams = { ",", ";" };
            string[] recipients = smtpTo.Split(myParams, StringSplitOptions.RemoveEmptyEntries);
            m_smtpTo = recipients;
            m_smartHostName = smartHostName;



            m_smartHostPassword = smartHostPassword;
            m_mailState.RoundOfMailsToSend = rounds;
            m_numberConnections = connections;
            m_endlessSending = endlessSending;
            if ((String.IsNullOrEmpty(smartHostName) || smartHostPassword == null))
            {
                m_anonymousAuthentication = true;
            }
            if (mailAttributes != null)
            {
                m_mailAttributes = mailAttributes;
                if (m_mailAttributes.Headers.Count > 0)
                {
                    m_addHeader = true;
                }
            }
            m_parsed = parsed;
            m_waitBetweenMailsms = waitBetweenMailsms;
            m_sendOriginal = sendOriginal;
            m_newMessageID = newMessageID;
            m_fallBack = fallBack;
            m_mailState.Id = id;
            m_addRecipientToMail = addRecipientToMail;
            m_addSenderToMail = addSenderToMail;
            if (mailAttributes.Subject.position != null)
            {
                if (!String.IsNullOrEmpty(mailAttributes.Subject.subjectstring))
                {
                    m_addHeader = true;
                }
            }

            if (m_addRecipientToMail)
            {
                MailSend.MailAttributes.SHeader myHeader = new MailAttributes.SHeader();
                myHeader.name = Tools.sMailTo;
                myHeader.value = smtpTo;
                m_mailAttributes.Headers.Add(myHeader);
                m_addHeader = true;
            }
            if (m_addSenderToMail)
            {
                MailSend.MailAttributes.SHeader myHeader = new MailAttributes.SHeader();
                myHeader.name = Tools.sMailFrom;
                myHeader.value = m_smtpFrom;
                m_mailAttributes.Headers.Add(myHeader);
                m_addHeader = true;
            }
            m_mailState.EndlessSending = endlessSending;
            m_anonymousAuthentication = anonymousAuth;
            m_cacheMailsUntilInByte = cacheMailsUntilInByte;
            m_codePageEnabled = codePageEnabled;
            m_codePage = codePage;
            if (!String.IsNullOrEmpty(recipientGroup))
            {
                if (recipientGroupStart <= recipientGroupEnd)
                {
                    m_useRecipientGroup = true;
                    int capacy = recipientGroupEnd - recipientGroupStart + 1;
                    m_recipientsGroupList = new List<string>(capacy);
                    m_recipientGroupIndex = 0;

                    for (int i = recipientGroupStart; i <= recipientGroupEnd; i++)
                    {
                        m_recipientsGroupList.Add(recipientGroup.Trim() + i.ToString().Trim() + "@" + recipientGroupDomain.Trim());
                    }
                }
            }
            m_subjectMailCount = subjectMailCount;

        }
        public Client(IPAddress smartHostIPAddress, int port, string smtpFrom, string smtpTo, string smartHostName, string smartHostPassword, ulong rounds, int connections, bool endlessSending, MailAttributes mailAttributes)
        {
            this.m_senderIPAddress = IPAddress.Any;
            //this.m_senderIPAddress = IPAddress.Parse(@"172.30.200.130");
            m_useSmartHost = true;
            //m_smartHostIPAddresses = new IPAddress[m_numberConnections];
            m_smartHostIPAddress = smartHostIPAddress;
            m_smartHostPort = port;
            m_smtpFrom = smtpFrom;
            string[] myParams = { ",", ";" };
            string[] recipients = smtpTo.Split(myParams, StringSplitOptions.RemoveEmptyEntries);
            m_smtpTo = recipients;
            m_smartHostName = smartHostName;
            m_smartHostPassword = smartHostPassword;
            m_mailState.RoundOfMailsToSend = rounds;
            m_numberConnections = connections;
            m_endlessSending = endlessSending;
            if ((String.IsNullOrEmpty(smartHostName) || smartHostPassword == null))
            {
                m_anonymousAuthentication = true;
            }
            if (mailAttributes != null)
            {
                m_mailAttributes = mailAttributes;
            }

        }

        private void RecipientIndexPlusPlus()
        {
            lock (m_recipientGroupIndexLock)
            {
                m_recipientGroupIndex++;
            }
        }
        private string GetRecipientFromRecipientGroup()
        {
            string recipient = String.Empty;
            lock (m_recipientGroupIndexLock)
            {

                recipient = (string)m_recipientsGroupList[m_recipientGroupIndex];
                if (m_recipientGroupIndex < (m_recipientsGroupList.Count - 1))
                {
                    m_recipientGroupIndex++;
                }
                else
                {
                    m_recipientGroupIndex = 0;
                }

            }
            return recipient;
        }

        override public void RequestStop()
        {
            _shouldStop = true;
        }


        public Queue<MessageWrapper> Mail_Queue
        {
            get { return mail_Queue; }
            set { mail_Queue = value; }
        }



        //Bei Rekursion wird nciht geprüft ob ein angegebenes Verzeichnis ein anderes enthält. Enthaltene Emls werden dann doppelt vorkommen
        override public ulong FillQueue(List<string> dirs, bool recursive, List<string> filters, ulong connections, bool endlessSending)
        {
            int countDirs = dirs.Count;
            synchroniseCountMails = new Dictionary<ulong, ulong>(countDirs);
            SearchOption mySearchOption = SearchOption.TopDirectoryOnly;
            if (countDirs > 1)
            {
                //dirs = Tools.RemoveDoubleItems<string>(dirs);
            }

            if (recursive)
            {
                mySearchOption = SearchOption.AllDirectories;
            }
            ulong tooManyConnectionCounter = 0;
            while (connections > m_mailState.WholeMailsToSend)
            {
                ulong internMailCounter = 0;
                foreach (string dir in dirs)
                {
                    if (Directory.Exists(dir))
                    {
                        foreach (string filter in filters)
                        {
                            try
                            {
                                foreach (string messageFile in Directory.GetFiles(dir, filter, mySearchOption))
                                {
                                    MessageWrapper msg = new MessageWrapper(messageFile);
                                    mail_Queue.Enqueue(msg);

                                    msg.MailID = internMailCounter;
                                    if (tooManyConnectionCounter < 1)
                                    {
                                        synchroniseCountMails.Add(msg.MailID, 0);
                                    }

                                    m_mailState.WholeMailsToSend++;
                                    internMailCounter++;

                                }
                            }
                            catch (Exception ex)
                            {
                                Log.logger.Error(ex.ToString());
                            }
                        }
                    }
                    else if (File.Exists(dir))
                    {
                        MessageWrapper msg = new MessageWrapper(dir);
                        mail_Queue.Enqueue(msg);
                        msg.MailID = internMailCounter;
                        if (tooManyConnectionCounter < 1)
                        {
                            synchroniseCountMails.Add(msg.MailID, 0);
                        }
                        m_mailState.WholeMailsToSend++;
                        internMailCounter++;
                    }
                    else
                    {
                        Log.logger.Error("Verzeichnis " + dir + " existiert nicht");
                    }
                }
                tooManyConnectionCounter++;
                //if (!endlessSending) break;
            }
            if (tooManyConnectionCounter > 1)
            {
                m_mailState.WholeMailsToSend = m_mailState.WholeMailsToSend / tooManyConnectionCounter;

            }
            //if (connections > m_mailState.WholeMailsToSend)
            //{
            //    mail_Queue = new Queue<MessageWrapper>(mail_Queue
            //}
            //Anzahl der Mails die gesendet werden sollen * Die anzahl der Runden ergibt die Gesamtanzahl


            m_mailState.WholeMailsToSend = m_mailState.WholeMailsToSend * m_mailState.RoundOfMailsToSend;
            Log.logger.Info("Anzahl der Durchläufe: " + m_mailState.RoundOfMailsToSend);
            Log.logger.Info("Gesamtanzahl der Nachrichten: " + m_mailState.WholeMailsToSend);
            OnMaxMailsToSend(m_mailState);
            Thread.Sleep(1);
            return m_mailState.WholeMailsToSend;
        } //end FillQueue

        override public void SetMimeAndSend()
        {

            //mailThreads = new Thread[m_numberConnections];
            m_smtpClient = new SMTP_Client[m_numberConnections];
            doneEvents = new ManualResetEvent[m_numberConnections];
            Thread[] myThread = new Thread[m_numberConnections];


            try
            {
                for (int i = 0; i < m_numberConnections; i++)
                {
                    //mailThreads[i] = new Thread(new ThreadStart(StartSend));
                    doneEvents[i] = new ManualResetEvent(false);
                    m_smtpClient[i] = new SMTP_Client();
                    m_smtpClient[i].Timeout = 0;
                    ParameterizedThreadStart threadStart = new ParameterizedThreadStart(StartSendNew);
                    myThread[i] = new Thread(threadStart);
                    myThread[i].Start(i);

                    //ThreadPool.QueueUserWorkItem(StartSend, i);

                    //ThreadPool.
                    //mailThreads[i].Start();
                }

            }
            catch (Exception ex)
            {
            }
            //WaitHandle.WaitAll(doneEvents);
            /*
            for (int i = 0; i < m_numberConnections; i++)
            {
                if (m_smtpClient[i].IsConnected)
                {
                    m_smtpClient[i].Disconnect();
                }
            }
             */


            //return m_mailState;
        }



        public void StartSendNew(Object threadContext)
        {
            int threadIndex = (int)threadContext;
            try
            {

                // Smart host enabled, use it
                if (m_useSmartHost)
                {
                    /*
                    m_smtpClient[threadIndex].Logger = new LumiSoft.Net.Log.Logger();
                    m_smtpClient[threadIndex].Logger.WriteLog += delegate(object sender, LumiSoft.Net.Log.WriteLogEventArgs e)
                    {
                        Log.logger.Info("-----------------------------------BEGIN-SMTPLOG-----------------------------------------");
                        Log.logger.Info(e.LogEntry.ID);
                        Log.logger.Info(e.LogEntry.Time);
                        Log.logger.Info("Entry-Type: " + e.LogEntry.EntryType);
                        Log.logger.Info("Size: " + e.LogEntry.Size);
                        if (e.LogEntry.EntryType == LumiSoft.Net.Log.LogEntryType.Exception)
                        {
                            Log.logger.Info(e.LogEntry.Exception);
                            Log.logger.Info(e.LogEntry.Exception.ToString());
                        }
                        Log.logger.Info(e.LogEntry.Text);

                        Log.logger.Info("LocalEndpoint: " + e.LogEntry.LocalEndPoint.ToString());
                        Log.logger.Info("RemoteEndpoint: " + e.LogEntry.RemoteEndPoint.ToString());
                        Log.logger.Info("-----------------------------------END-SMTPLOG-----------------------------------------");

                    };
                     * */
                    //m_smtpClient.BeginConnect(new IPEndPoint(m_senderIPAddress, 25), new IPEndPoint(m_smartHostIPAddress, m_smartHostPort), m_smartHostUseSSL, new AsyncCallback(ConnectCompleted), null);
                    //m_smtpClient.Connect(new IPEndPoint(m_senderIPAddress, 25), new IPEndPoint(m_smartHostIPAddress, 25), false);
                    //int index = (int) threadContext;
                    while (!_shouldStop)
                    {
                        if (!m_smtpClient[threadIndex].IsConnected)
                        {
                            Connect(threadContext);
                        }
                        if (!m_smtpClient[threadIndex].IsAuthenticated)
                        {
                            Authenticate(threadContext);
                        }
                        SendMessageNew(threadContext);
                    }

                }
                // Use direct delivery
                else
                {
                    //m_pSmtpClient.BeginConnect(new IPEndPoint(m_pRelayServer.SendingIP,0),m_pRelayInfo.To,25,new CommadCompleted(this.ConnectCompleted));
                }
            }
            catch (AllMailsSendException ex)
            {
                Log.logger.Info(ex.ToString());
            }
            catch (NoConnectionPossibleException ex)
            {
                Log.logger.Error(ex.ToString());
                doneEvents[threadIndex].Set();


            }
            catch (Exception ex)
            {

                Log.logger.Fatal(ex.ToString());
                Log.logger.Fatal(new System.Diagnostics.StackTrace().ToString());
                if (m_smtpClient[threadIndex].IsConnected)
                {
                    m_smtpClient[threadIndex].Disconnect();
                }
                
                m_smtpClient[threadIndex].Dispose();
                //Error.DumpError(x, new System.Diagnostics.StackTrace());
                //End(false, x);
            }

            finally
            {
                //Thread soll Event schmeissen und bekanntgeben das er fertig ist
                OnSendMailEnds(m_mailState);
                if (m_smtpClient[threadIndex].IsConnected)
                {
                    m_smtpClient[threadIndex].Disconnect();
                }
                /*
                for (int i = 0; i < m_numberConnections; i++)
                {
                    if (m_smtpClient[i].IsConnected)
                    {
                        m_smtpClient[i].Disconnect();
                    }
                }
                 */
            }
        }

        public void SendMessageNew(object threadContext)
        {
            int threadIndex = (int)threadContext;
            MessageWrapper msg = null;
            try
            {

                /*
                if (m_mailState.CountWholeMailsSend >= m_mailState.WholeMailsToSend)
                {
                    break;
                }
                 */
                if (m_waitBetweenMailsms == 0)
                {

                }
                SMTP_Client client = m_smtpClient[threadIndex];



                try
                {

                    msg = GetMessage();
                    if (msg == null)
                    {
                        //break;
                        throw new AllMailsSendException("No more Message in Queue");
                    }
                }
                catch (QueueEmptyException ex)
                {
                    if (m_mailState.CountWholeMailsSend >= m_mailState.WholeMailsToSend && !m_endlessSending)
                    {
                        //Alle Mails abgearbeitet
                        throw new AllMailsSendException(ex.ToString());
                    }
                }
                catch (AllMailsSendException ex)
                {

                    //Alle Mails abgearbeitet
                    throw ex;

                }
                catch (Exception ex)
                {
                    Log.logger.Fatal(ex.ToString());
                    //break;
                }
                //-------------------------------------------------------------------------------------------------------------------------------
                Stream msgStream = BuildStream(msg, threadContext);


                /* ToDo Was passiert wenn immer ein Fehler geschmissen wird
                 * Es muss die genauere Fehlerursache gesucht werden, bei SMTP_ClientException muss auch die Mail als fehlerhaft markiert werden und evtl. die nächste Mail geholt werden.
                 * Anzahl der versuche muss definiert werden!!!!
                 */
                //----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
                try
                {

                    if (m_waitBetweenMailsms <= 0)
                    {
                        Log.logger.Info(msg.MessagePath);

                        if (IsMessageToSend(msg))
                        {
                            m_smtpClient[threadIndex].SendMessage(msgStream);
                            lock (semOnMailSend)
                            {
                                OnMailSent(msg, m_mailState);
                            }
                            lock (countLock)
                            {
                                //m_mailState.CountWholeMailsSend++;
                                CountWholeMailToSendPlus1();
                            }
                        }
                        else
                        {
                            m_smtpClient[threadIndex].Rset();
                            Log.logger.Debug("Message is not alloed to be sent again " + msg.MessagePath + " Size: " + msg.MsgSize + " ErrorMail: " + msg.SendErrors + " Number of Exceptions: " + msg.Ex.Count);
                            /*
                            lock (semOnMailSend)
                            {
                                OnMailSent(msg, m_mailState);
                            }
                             */ 
                        }
                    }
                    else
                    {
                        lock (sendLock)
                        {


                            if (IsMessageToSend(msg))
                            {
                                Thread.Sleep(m_waitBetweenMailsms);
                                Log.logger.Info(msg.MessagePath);
                                //Console.WriteLine(msg.MessagePath);
                                m_smtpClient[threadIndex].SendMessage(msgStream);
                                lock (semOnMailSend)
                                {
                                    OnMailSent(msg, m_mailState);
                                }
                                lock (countLock)
                                {
                                    //m_mailState.CountWholeMailsSend++;
                                    CountWholeMailToSendPlus1();
                                }

                            }
                            else
                            {
                                m_smtpClient[threadIndex].Rset();
                                Log.logger.Debug("Message is not alloed to be sent again " + msg.MessagePath + " Size: " + msg.MsgSize + " ErrorMail: " + msg.SendErrors + " Number of Exceptions: " + msg.Ex.Count);
                            }
                        }
                    }

                }
                catch (InvalidOperationException ex)
                {
                    //client nicht verbunden -> neu verbinden
                    SendingErrorHandlingNew(ref msg, ex, threadContext, msgStream);
                   
                }
                catch (SMTP_ClientException ex)
                {
                    //ToDo SMTP Server gibt Fehler zurück
                    SendingErrorHandlingNew(ref msg, ex, threadContext, msgStream);
                   
                }
                catch (LumiSoft.Net.IO.LineSizeExceededException ex)
                {
                    SendingErrorHandlingNew(ref msg, ex, threadContext, msgStream);
                    
                }
                catch (AllMailsSendException ex)
                {
                    throw ex;
                }
                catch (Exception ex)
                {
                    SendingErrorHandlingNew(ref msg, ex, threadContext, msgStream);
                    
                }


                if (msgStream != null)
                {
                    msgStream.Close();
                }

                if (!(msg.MsgCached))
                {
                    msg.MailMsg = null;
                    msg.MailMsgString = null;
                    msg.MailMsgByte = null;
                }

                if (IsMessageToSendWithoutCount(msg) || m_endlessSending)
                {
                    //Corrupte Mails bleiben erst mal drin in der Queue
                    msg.SendOnce = true;
                    lock (m_dequeueLock)
                    {
                        mail_Queue.Enqueue(msg);
                    }
                }
            }//end try
            catch (AllMailsSendException ex)
            {
                Log.logger.Debug(ex.ToString());
                
                doneEvents[threadIndex].Set();
                throw ex;
            }

            catch (Exception ex)
            {

                Log.logger.Fatal(ex.ToString());
                Log.logger.Fatal(new System.Diagnostics.StackTrace().ToString());
                doneEvents[threadIndex].Set();
                if (ex is NoConnectionPossibleException)
                {
                    throw ex;
                }
            }

        }

        public void SendingErrorHandlingNew(ref MessageWrapper msg, Exception ex, object threadContext, Stream msgStream)
        {
            int threadIndex = (int)threadContext;
            Log.logger.Fatal(ex.ToString());
            Log.logger.Fatal(new System.Diagnostics.StackTrace().ToString());
            msg.Ex.Add(ex);
            msg.Corrupt = true;
            MailState.errorMails.Add(msg);
            msg.AnzGesendet++;
            if (msgStream != null)
            {
                msgStream.Close();
            }
            lock (countLock)
            {
                m_mailState.ErrorsWhileSendingMails++;
                CountWholeMailToSendPlus1();
            }
            //ToDo:Fehler mitgeben
            lock (semOnMailSend)
            {
                OnMailSent(msg, m_mailState);
            }
            m_smtpClient[threadIndex].Disconnect(false);
        }


        public void StartSend(Object threadContext)
        {
            int threadIndex = (int)threadContext;
            try
            {

                // Smart host enabled, use it
                if (m_useSmartHost)
                {
                    /*
                    m_smtpClient[threadIndex].Logger = new LumiSoft.Net.Log.Logger();
                    m_smtpClient[threadIndex].Logger.WriteLog += delegate(object sender, LumiSoft.Net.Log.WriteLogEventArgs e)
                    {
                        Log.logger.Info("-----------------------------------BEGIN-SMTPLOG-----------------------------------------");
                        Log.logger.Info(e.LogEntry.ID);
                        Log.logger.Info(e.LogEntry.Time);
                        Log.logger.Info("Entry-Type: " + e.LogEntry.EntryType);
                        Log.logger.Info("Size: " + e.LogEntry.Size);
                        if (e.LogEntry.EntryType == LumiSoft.Net.Log.LogEntryType.Exception)
                        {
                            Log.logger.Info(e.LogEntry.Exception);
                            Log.logger.Info(e.LogEntry.Exception.ToString());
                        }
                        Log.logger.Info(e.LogEntry.Text);

                        Log.logger.Info("LocalEndpoint: " + e.LogEntry.LocalEndPoint.ToString());
                        Log.logger.Info("RemoteEndpoint: " + e.LogEntry.RemoteEndPoint.ToString());
                        Log.logger.Info("-----------------------------------END-SMTPLOG-----------------------------------------");

                    };
                     * */
                    //m_smtpClient.BeginConnect(new IPEndPoint(m_senderIPAddress, 25), new IPEndPoint(m_smartHostIPAddress, m_smartHostPort), m_smartHostUseSSL, new AsyncCallback(ConnectCompleted), null);
                    //m_smtpClient.Connect(new IPEndPoint(m_senderIPAddress, 25), new IPEndPoint(m_smartHostIPAddress, 25), false);
                    //int index = (int) threadContext;

                    Connect(threadContext);
                    Authenticate(threadContext);
                    SendMessage(threadContext);

                }
                // Use direct delivery
                else
                {
                    //m_pSmtpClient.BeginConnect(new IPEndPoint(m_pRelayServer.SendingIP,0),m_pRelayInfo.To,25,new CommadCompleted(this.ConnectCompleted));
                }
            }
            catch (NoConnectionPossibleException ex)
            {
                Log.logger.Error(ex.ToString());
                doneEvents[threadIndex].Set();


            }
            catch (Exception ex)
            {

                Log.logger.Fatal(ex.ToString());
                Log.logger.Fatal(new System.Diagnostics.StackTrace().ToString());
                m_smtpClient[threadIndex].Disconnect();
                m_smtpClient[threadIndex].Dispose();
                //Error.DumpError(x, new System.Diagnostics.StackTrace());
                //End(false, x);
            }

            finally
            {
                //Thread soll Event schmeissen und bekanntgeben das er fertig ist
                OnSendMailEnds(m_mailState);
                if (m_smtpClient[threadIndex].IsConnected)
                {
                    m_smtpClient[threadIndex].Disconnect();
                }
                /*
                for (int i = 0; i < m_numberConnections; i++)
                {
                    if (m_smtpClient[i].IsConnected)
                    {
                        m_smtpClient[i].Disconnect();
                    }
                }
                 */
            }



        }
        override public void AboardThreads()
        {
            RequestStop();
        }

        private void Authenticate(Object threadContext)
        {
            int threadIndex = (int)threadContext;

            try
            {
                //if (result.IsCompleted)
                {
                    m_smtpClient[threadIndex].EhloHelo(m_senderIPAddress.ToString());
                    //m_pRemoteEndPoint = (IPEndPoint)m_pSmtpClient.RemoteEndPoint;
                    if (!m_anonymousAuthentication && !m_smtpClient[threadIndex].IsAuthenticated)
                    {
                        m_smtpClient[threadIndex].Authenticate(m_smartHostName, m_smartHostPassword);
                    }


                }
                //else
                {

                    //End(false, exception);
                }
            }
            catch (Exception ex)
            {
                Log.logger.Fatal(ex.ToString());
                Log.logger.Fatal(new System.Diagnostics.StackTrace().ToString());
                throw ex;
            }

        }
        public void SendMessage(object threadContext)
        {
            int threadIndex = (int)threadContext;
            MessageWrapper msg = null;
            try
            {

                //MessageWrapper msg = GetMessage();
                //SMTP_Client.QuickSend(msg.MailMsg);
                while (!_shouldStop)
                {
                    /*
                    if (m_mailState.CountWholeMailsSend >= m_mailState.WholeMailsToSend)
                    {
                        break;
                    }
                     */
                    //Wenn zwi
                    if (m_waitBetweenMailsms == 0)
                    {

                    }
                    SMTP_Client client = m_smtpClient[threadIndex];



                    try
                    {

                        msg = GetMessage();
                        if (msg == null)
                        {
                            break;
                        }
                    }
                    catch (QueueEmptyException ex)
                    {
                        if (m_mailState.CountWholeMailsSend >= m_mailState.WholeMailsToSend && !m_endlessSending)
                        {
                            //Alle Mails abgearbeitet
                            throw new AllMailsSendException(ex.ToString());
                        }
                    }
                    catch (AllMailsSendException ex)
                    {

                        //Alle Mails abgearbeitet
                        throw ex;

                    }
                    catch (Exception ex)
                    {
                        Log.logger.Fatal(ex.ToString());
                        //break;
                    }



                    Stream msgStream = null;
                    try
                    {

                        if (m_parsed && !m_sendOriginal)
                        {
                            if (!msg.MsgCached)
                            {
                                ParseMessage(msg);

                            }

                            if (msg.Parsed)
                            {
                                msg = AddMailAttributes(msg);
                                MemoryStream memoryStream = new MemoryStream();
                                msg.MailMsg.ToStream(memoryStream, (new MIME_Encoding_EncodedWord(MIME_EncodedWordEncoding.Q, Encoding.UTF8)), Encoding.UTF8);
                                if (!msg.SendOnce)
                                {
                                    msg.MsgSize = memoryStream.Length;
                                    msg.CacheMSG(m_cacheMailsUntilInByte, msg.MsgSize);
                                }
                                memoryStream.Position = memoryStream.Seek(0, SeekOrigin.Begin);
                                msgStream = (Stream)memoryStream;
                            }

                            //Message lässt sich nciht parsen und sie soll trotzdem gesendet werden
                            if (m_parsed && !msg.Parsed && !m_sendOriginal && m_fallBack)
                            {

                                if (!msg.MsgCached)
                                {
                                    byte[] file = File.ReadAllBytes(msg.MessagePath);
                                    msg.MailMsgByte = file;
                                }

                                int bodyPosition;
                                byte[] byteHeaderSchnell = StringHelper.StringHelper.GetHeaderAndBodyPosition(msg.MailMsgByte, out bodyPosition);
                                if (byteHeaderSchnell != null)
                                {
                                    msg.BodyPosition = bodyPosition;
                                    System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
                                    string header = enc.GetString(byteHeaderSchnell);



                                    header = AddHeaderUnparsed(header, msg, true);

                                    byteHeaderSchnell = enc.GetBytes(header);
                                }
                                msg.MailMsgByte = StringHelper.StringHelper.AddHeaderToBodyByte(byteHeaderSchnell, msg.MailMsgByte, msg.BodyPosition);



                                MemoryStream ms = new MemoryStream(msg.MailMsgByte);
                                if (!msg.SendOnce)
                                {
                                    msg.MsgSize = ms.Length;
                                    msg.CacheMSG(m_cacheMailsUntilInByte, msg.MsgSize);
                                }
                                msgStream = (Stream)ms;



                            }
                        }





                        if (m_sendOriginal && !m_newMessageID && !m_parsed)//Message wird mit original MessageID gesendet
                        {
                            if (!msg.MsgCached)
                            {
                                byte[] file = File.ReadAllBytes(msg.MessagePath);
                                msg.MailMsgByte = file;
                            }
                            MemoryStream ms = new MemoryStream(msg.MailMsgByte);
                            if (!msg.SendOnce)
                            {
                                msg.MsgSize = ms.Length;
                                msg.CacheMSG(m_cacheMailsUntilInByte, msg.MsgSize);
                            }

                            msgStream = (Stream)ms;

                        }
                        else if (m_sendOriginal && m_newMessageID && !m_parsed && !m_addHeader)//Es wird im Header nur die MessageID entfernt, für maximale Performance
                        {
                            if (!msg.MsgCached)
                            {

                                byte[] file = File.ReadAllBytes(msg.MessagePath);

                                msg.MailMsgByte = file;

                                int bodyPosition;
                                byte[] byteHeaderSchnell = StringHelper.StringHelper.GetHeaderAndBodyPosition(file, out bodyPosition);
                                if (byteHeaderSchnell != null)
                                {
                                    msg.BodyPosition = bodyPosition;
                                    System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
                                    string header = enc.GetString(byteHeaderSchnell);

                                    header = StringHelper.StringHelper.RemoveMessageID(header);
                                    if (m_subjectMailCount)
                                    {
                                        header = StringHelper.StringHelper.ChangeSubject(header, ("-" + GetCountWholeMailToSend().ToString()), MailAttributes.HeaderPosition.end);
                                    }
                                    byteHeaderSchnell = enc.GetBytes(header);
                                }
                                msg.MailMsgByte = StringHelper.StringHelper.AddHeaderToBodyByte(byteHeaderSchnell, file, msg.BodyPosition);
                            }
                            MemoryStream ms = new MemoryStream(msg.MailMsgByte);

                            if (!msg.SendOnce)
                            {
                                msg.MsgSize = ms.Length;
                                msg.CacheMSG(m_cacheMailsUntilInByte, msg.MsgSize);
                            }

                            msgStream = (Stream)ms;
                        }
                        else if (m_sendOriginal && m_newMessageID && !m_parsed && m_addHeader)//Es werden weitere Elemente in den Header eingefügt -> langsamer
                        {
                            if (!msg.MsgCached)
                            {
                                byte[] file = File.ReadAllBytes(msg.MessagePath);
                                msg.MailMsgByte = file;

                            }

                            int bodyPosition;
                            byte[] byteHeaderSchnell = StringHelper.StringHelper.GetHeaderAndBodyPosition(msg.MailMsgByte, out bodyPosition);
                            if (byteHeaderSchnell != null)
                            {
                                msg.BodyPosition = bodyPosition;
                                System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
                                string header = enc.GetString(byteHeaderSchnell);

                                header = AddHeaderUnparsed(header, msg, true);

                                byteHeaderSchnell = enc.GetBytes(header);
                            }
                            msg.MailMsgByte = StringHelper.StringHelper.AddHeaderToBodyByte(byteHeaderSchnell, msg.MailMsgByte, msg.BodyPosition);

                            MemoryStream ms = new MemoryStream(msg.MailMsgByte);


                            if (!msg.SendOnce)
                            {
                                msg.MsgSize = ms.Length;
                                msg.CacheMSG(m_cacheMailsUntilInByte, msg.MsgSize);
                            }

                            msgStream = (Stream)ms;
                        }


                        m_smtpClient[threadIndex].MailFrom(m_smtpFrom, -1);
                        if (!m_useRecipientGroup)
                        {
                            foreach (String mailTo in m_smtpTo)
                            {
                                m_smtpClient[threadIndex].RcptTo(mailTo);
                            }
                        }
                        else
                        {
                            m_smtpClient[threadIndex].RcptTo(GetRecipientFromRecipientGroup());
                        }


                    }
                    catch (SMTP_ClientException ex)
                    {
                        //m_smtpClient[threadIndex].Rset();
                        //ToDoMaxSizeExceed Mail ist zu groß Fehlerbehandlung und Continue
                    }
                    catch (Exception ex)
                    {
                        if (msgStream != null)
                        {
                            msgStream.Close();
                        }
                        throw ex;
                    }
                    /* ToDo Was passiert wenn immer ein Fehler geschmissen wird
                     * Es muss die genauere Fehlerursache gesucht werden, bei SMTP_ClientException muss auch die Mail als fehlerhaft markiert werden und evtl. die nächste Mail geholt werden.
                     * Anzahl der versuche muss definiert werden!!!!
                     */
                    bool sendError = false;
                    int countTriesToSendMail = 0;
                    const int triesToSendMail = 1;
                    do
                    {
                        sendError = false;
                        try
                        {

                            if (m_waitBetweenMailsms <= 0)
                            {
                                Log.logger.Info(msg.MessagePath);

                                if (IsMessageToSend(msg))
                                {
                                    m_smtpClient[threadIndex].SendMessage(msgStream);
                                    lock (semOnMailSend)
                                    {
                                        OnMailSent(msg, m_mailState);
                                    }
                                    lock (countLock)
                                    {
                                        //m_mailState.CountWholeMailsSend++;
                                        CountWholeMailToSendPlus1();
                                    }
                                }
                                else
                                {
                                    m_smtpClient[threadIndex].Rset();
                                    //ToDo, Error vielleicht fehlen deshalb hochzähler und bricht ab nach RSET muss auch hochgezählt werden!!
                                }
                            }
                            else
                            {
                                lock (sendLock)
                                {


                                    if (IsMessageToSend(msg))
                                    {
                                        Thread.Sleep(m_waitBetweenMailsms);
                                        Log.logger.Info(msg.MessagePath);
                                        //Console.WriteLine(msg.MessagePath);
                                        m_smtpClient[threadIndex].SendMessage(msgStream);
                                        lock (semOnMailSend)
                                        {
                                            OnMailSent(msg, m_mailState);
                                        }
                                        lock (countLock)
                                        {
                                            //m_mailState.CountWholeMailsSend++;
                                            CountWholeMailToSendPlus1();
                                        }

                                    }
                                    else
                                    {
                                        m_smtpClient[threadIndex].Rset();
                                    }
                                }
                            }
                            sendError = false;
                            //msg.AnzGesendet++;

                        }
                        catch (InvalidOperationException ex)
                        {
                            //ToDo client nicht verbunden -> neu verbinden

                            //ToDo Connection Funktion mit Fehlerabfrage

                            //m_smtpClient.Connect(new IPEndPoint(m_senderIPAddress, 25), new IPEndPoint(m_smartHostIPAddress, 25), false);
                            //sendError = true;
                            //countTriesToSendMail++;
                            SendingErrorHandling(ref msg, ex, threadContext, ref sendError, ref countTriesToSendMail, msgStream);
                        }
                        catch (SMTP_ClientException ex)
                        {
                            //ToDo SMTP Server gibt Fehler zurück
                            //m_smtpClient.Connect(new IPEndPoint(m_senderIPAddress, 25), new IPEndPoint(m_smartHostIPAddress, 25), false);
                            //sendError = true;
                            //countTriesToSendMail++;
                            SendingErrorHandlingWithRetrie(ref msg, ex, threadContext, ref sendError, ref countTriesToSendMail, triesToSendMail, msgStream);
                        }
                        catch (LumiSoft.Net.IO.LineSizeExceededException ex)
                        {
                            SendingErrorHandling(ref msg, ex, threadContext, ref sendError, ref countTriesToSendMail, msgStream);
                        }
                        catch (AllMailsSendException ex)
                        {
                            throw ex;
                        }
                        catch (Exception ex)
                        {
                            SendingErrorHandling(ref msg, ex, threadContext, ref sendError, ref countTriesToSendMail, msgStream);
                        }
                    }
                    while (sendError && countTriesToSendMail < triesToSendMail);
                    try
                    {
                        if (msgStream != null)
                        {
                            msgStream.Close();
                        }
                        if (!msg.Parsed && !String.IsNullOrEmpty(msg.Temp))
                        {
                            if (msgStream != null)
                            {
                                msgStream.Close();
                            }
                            //File.Delete(msg.Temp);
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.logger.Info(ex.ToString());
                    }

                    if (!(msg.MsgCached))
                    {
                        msg.MailMsg = null;
                        msg.MailMsgString = null;
                        msg.MailMsgByte = null;
                    }

                    if (IsMessageToSendWithoutCount(msg) || m_endlessSending)
                    {
                        //Corrupte Mails bleiben erst mal drin in der Queue
                        msg.SendOnce = true;
                        lock (m_dequeueLock)
                        {
                            mail_Queue.Enqueue(msg);
                        }
                    }
                    //message.ToStream(ms, new MIME_Encoding_EncodedWord(MIME_EncodedWordEncoding.Q, Encoding.UTF8), Encoding.UTF8);

                    //m_smtpClient.SendMessage(
                }
            }
            //catch (InvalidOperationException ex)
            //{
            //ToDo Muss geändert werden
            //}
            catch (AllMailsSendException ex)
            {
                Log.logger.Debug(ex.ToString());

                doneEvents[threadIndex].Set();
            }

            catch (Exception ex)
            {

                Log.logger.Fatal(ex.ToString());
                Log.logger.Fatal(new System.Diagnostics.StackTrace().ToString());
                doneEvents[threadIndex].Set();
                if (ex is NoConnectionPossibleException)
                {
                    throw ex;
                }
            }

        }

        public bool IsMessageToSendWithoutCount(MessageWrapper msg)
        {
            if (!m_endlessSending)
            {
                lock (semSychroniseCountMails)
                {
                    ulong countSend = synchroniseCountMails[msg.MailID];
                    if (countSend < m_mailState.RoundOfMailsToSend)
                    {
                        return true;
                    }
                }
            }
            else
            {
                //msg.AnzGesendet++;
                return true;
            }
            return false;
        }

        public bool IsMessageToSend(MessageWrapper msg)
        {
            if (!m_endlessSending)
            {
                lock (semSychroniseCountMails)
                {
                    ulong countSend = synchroniseCountMails[msg.MailID];
                    if (countSend < m_mailState.RoundOfMailsToSend)
                    {
                        countSend++;
                        synchroniseCountMails[msg.MailID] = countSend;
                        msg.AnzGesendet = countSend;
                        return true;
                    }
                }
            }
            else
            {
                //msg.AnzGesendet++;
                return true;
            }
            return false;
        }

        public void SendingErrorHandlingWithRetrie(ref MessageWrapper msg, Exception ex, object threadContext, ref bool sendError, ref int countTriesToSendMail, int triesToSendMail, Stream msgStream)
        {
            int threadIndex = (int)threadContext;
            Log.logger.Fatal(ex.ToString());
            Log.logger.Fatal(new System.Diagnostics.StackTrace().ToString());
            msg.Ex.Add(ex);
            sendError = true;
            countTriesToSendMail++;
            m_smtpClient[threadIndex].Disconnect(false);
            Connect(threadContext);
            Authenticate(threadContext);

            if (countTriesToSendMail >= triesToSendMail)
            {
                msg.Corrupt = true;
                msg.AnzGesendet++;

                //MailState.errorMails.Add(msg);
                if (msgStream != null)
                {
                    msgStream.Close();
                }
                lock (countLock)
                {
                    m_mailState.ErrorsWhileSendingMails++;
                    //Hier hochzählen ist nicht sauber da Programm sonst HIER beendet werden kann wenn alle Mails gesendet wurden
                    CountWholeMailToSendPlus1();
                }
                lock (semOnMailSend)
                {
                    OnMailSent(msg, m_mailState);
                }
            }
        }

        public void SendingErrorHandling(ref MessageWrapper msg, Exception ex, object threadContext, ref bool sendError, ref int countTriesToSendMail, Stream msgStream)
        {
            int threadIndex = (int)threadContext;
            Log.logger.Fatal(ex.ToString());
            Log.logger.Fatal(new System.Diagnostics.StackTrace().ToString());
            msg.Ex.Add(ex);
            msg.Corrupt = true;
            MailState.errorMails.Add(msg);
            msg.AnzGesendet++;
            //sendError muss hier false gesetzt werden sonst wird eine Endlosschleife erzeugt wenn vorher eine andere Exception geworfen wurde.
            sendError = false;
            countTriesToSendMail++;
            if (msgStream != null)
            {
                msgStream.Close();
            }
            lock (countLock)
            {
                m_mailState.ErrorsWhileSendingMails++;
                CountWholeMailToSendPlus1();
            }
            //ToDo:Fehler mitgeben
            lock (semOnMailSend)
            {
                OnMailSent(msg, m_mailState);
            }
            m_smtpClient[threadIndex].Disconnect(false);
            Connect(threadContext);
            Authenticate(threadContext);
        }

        public void Connect(Object threadContext)
        {
            int triesToConnect = 0;
            bool connectionError = false;
            int threadIndex = (int)threadContext;
            do
            {
                try
                {
                    if (!m_smtpClient[threadIndex].IsConnected)
                    {
                        if (m_smartHostIPAddress != null)
                        {
                            m_smtpClient[threadIndex].Connect(new IPEndPoint(m_senderIPAddress, 0), new IPEndPoint(m_smartHostIPAddress, m_smartHostPort), false);
                        }
                        else
                        {
                            m_smtpClient[threadIndex].Connect(m_host, m_smartHostPort);
                        }
                        //m_smtpClient[threadIndex].Connect((new IPEndPoint(IPAddress.Parse("192.168.178.21"), 25), new IPEndPoint(m_smartHostIPAddress, 25), false);
                        //m_smtpClient[threadIndex].Connect("192.168.178.25", 25);
                    }
                    //SMTP_Client client2 = new SMTP_Client();
                    //client2.Connect(new IPEndPoint(IPAddress.Any, 25), new IPEndPoint(m_smartHostIPAddress, 25), false);

                }
                catch (Exception ex)
                {
                    Log.logger.Error(ex.ToString());
                    connectionError = true;
                    triesToConnect++;
                    if (triesToConnect > 1)
                    {
                        throw new NoConnectionPossibleException(ex.ToString());
                    }
                }
            }
            while (connectionError && triesToConnect < 3);
        }
        public MessageWrapper GetMessage()
        {

            MessageWrapper msgwrapper = null;
            /*
            if (m_countWholeMailsSend >= m_wholeMailsToSend && !m_endlessSending)
            {
                //return null;
                throw new AllMailsSendException("All mails have been sent");
            }
            */
            while (!_shouldStop)
            {
                try
                {

                    lock (m_dequeueLock)
                    {
                        msgwrapper = Mail_Queue.Dequeue();
                    }
                }
                catch (InvalidOperationException ex)
                {
                    //Queue ist leer
                    if ((m_mailState.CountWholeMailsSend >= m_mailState.WholeMailsToSend) && !m_endlessSending)
                    {
                        //Alle Mails abgearbeitet
                        Log.logger.Info("Queue is empty and all Mails sent in GetMessage: " + m_mailState.CountWholeMailsSend + " from: " + m_mailState.WholeMailsToSend);
                        throw new AllMailsSendException(ex.ToString());
                    }
                    else
                    {
                        //Nicht alle Mails abgearbeitet aber die vorige Mail darf nciht mehr gesendet werden also muss die NAchricht Null werden
                        msgwrapper = null;
                        Thread.Sleep(100);
                        Log.logger.Info("Queue is empty BUT NOT all Mails sent");
                        continue;
                    }
                    //throw new QueueEmptyException(ex.ToString());
                }
                /*
                if ((msgwrapper.AnzGesendet >= m_mailState.RoundOfMailsToSend) && !m_endlessSending)
                {
                    //ToDo: exception schmeissen??
                    msgwrapper = null;
                    //return null;
                    //break;
                }
                 */

                if ((m_mailState.CountWholeMailsSend >= m_mailState.WholeMailsToSend) && !m_endlessSending)
                {
                    //Alle Mails abgearbeitet
                    Log.logger.Info("All Mails sent in GetMessage sent: " + m_mailState.CountWholeMailsSend + " from: " + m_mailState.WholeMailsToSend);
                    throw new AllMailsSendException("All Mails sent");
                }

                if (msgwrapper != null)
                {
                    break;
                }
                /*
                if ((m_mailState.CountWholeMailsSend >= m_mailState.WholeMailsToSend) && !m_endlessSending)
                {
                    //Alle Mails abgearbeitet

                    throw new AllMailsSendException("All Mails sent");
                }
                 */
                //Thread.Sleep(200);
            }

            //Mail_Message msg = 
            return msgwrapper;
        }

        public ulong GetCountWholeMailToSend()
        {
            lock (countLock)
            {
                if (!m_endlessSending)
                {
                    return (m_mailState.CountWholeMailsSend + 1);
                }
                else
                {
                    return 0;
                }
            }
        }

        public void CountWholeMailToSendPlus1()
        {
            if (!m_endlessSending)
            {
                m_mailState.CountWholeMailsSend++;
            }
        }
        public static void ParseMailMessage()
        {

            //foreach(string msg in )
            //foreach(string msg in )
            //foreach(string msg in )

            Mail_Message msg = Mail_Message.ParseFromFile(@"c:\Mails\MailBig5.eml");
            Mail_Message msg3 = Mail_Message.ParseFromFile(@"c:\Mails\TestUTF8.eml", Encoding.Default);
            Mail_Message msg4 = Mail_Message.ParseFromFile(@"c:\Mails\TestUTF82.eml", Encoding.Default);
            Mime mime = Mime.Parse(@"c:\Mails\MailBig5.eml");
            Mail_Message msg2 = Mail_Message.ParseFromFile(@"c:\Mails\MailBig5.eml", Encoding.Default);
            // SmtpClientEx.QuickSendSmartHost(
            //msg.Subject = msg.Subject + "hallo";
            //msg.ToFile(@"c:\"


        }

        //public void AddHeaderUnparsed(MessageWrapper msg)
        //{
        //    string newHeader = String.Empty;
        //    if (newHeader == String.Empty)
        //    {
        //        newHeader = MakeHeaderLine("Message-ID", Guid.NewGuid().ToString());
        //    }
        //    else
        //    {
        //        newHeader = newHeader + Environment.NewLine + MakeHeaderLine("Message-ID", Guid.NewGuid().ToString());
        //    }
        //    /*
        //    if (m_mailAttributes == null)
        //    {
        //        return;
        //    }
        //   */

        //    try
        //    {


        //        foreach (MailSend.MailAttributes.SHeader header in m_mailAttributes.Headers)
        //        {
        //            if (newHeader == String.Empty)
        //            {
        //                newHeader = MakeHeaderLine(header.name, header.value);
        //            }
        //            else
        //            {
        //                newHeader = newHeader + Environment.NewLine + MakeHeaderLine(header.name, header.value);
        //            }

        //        }

        //        string tempPath = System.IO.Path.GetTempPath();
        //        msg.Temp = tempPath + System.Guid.NewGuid().ToString() + ".eml";
        //        File.Copy(msg.MessagePath, msg.Temp, true);

        //        AddToTxtFileStr(msg.Temp, newHeader, true, false, true, "Message-ID", "xxxxxxxxxx");
        //    }
        //    catch (Exception ex)
        //    {
        //        return;
        //    }
        //}
        public MessageWrapper AddMailAttributes(MessageWrapper msg)
        {
            /*
            if (m_mailAttributes == null)
            {
                return msg;
            }
             */
            msg.MailMsg.MessageID = Guid.NewGuid().ToString();
            /*
            if (msg.SendOnce)
            {
                return msg;
            }
             */
            try
            {
                Mail_Message myMSG = msg.MailMsg;
                MIME_h_Collection headerCollection = myMSG.Header;

                foreach (MailSend.MailAttributes.SHeader header in m_mailAttributes.Headers)
                {

                    if (header.addHeaderOnce == MailAttributes.AttributeInsertMethod.addOnce)
                    {
                        if (!msg.SendOnce || !msg.MsgCached)
                        {
                            MIME_h newHeader = new MIME_h_Unstructured(header.name, header.value);
                            headerCollection.RemoveAll(header.name);
                            headerCollection.Add(newHeader);
                        }
                    }
                    else if (header.addHeaderOnce == MailAttributes.AttributeInsertMethod.change)
                    {
                        MIME_h[] allHeaderByName = (MIME_h[])headerCollection[header.name];
                        if (allHeaderByName.Length <= 0)
                        {
                            MIME_h newHeader = new MIME_h_Unstructured(header.name, header.value);
                            headerCollection.Add(newHeader);
                        }
                        else
                        {
                            foreach (MIME_h_Unstructured lHeader in headerCollection)
                            {
                                if (lHeader.Name.Equals(header.name))
                                {
                                    lHeader.Value = header.value;
                                }
                            }

                        }
                    }
                    else if (header.addHeaderOnce == MailAttributes.AttributeInsertMethod.addAlways)
                    {
                        MIME_h newHeader = new MIME_h_Unstructured(header.name, header.value);
                        headerCollection.Add(newHeader);
                    }

                }
                if (!String.IsNullOrEmpty(m_mailAttributes.Subject.subjectstring))
                {
                    if (m_mailAttributes.Subject.position == MailAttributes.HeaderPosition.begin)
                    {
                        myMSG.Subject.Insert(0, m_mailAttributes.Subject.subjectstring);
                    }
                    else if (m_mailAttributes.Subject.position == MailAttributes.HeaderPosition.end)
                    {
                        myMSG.Subject = myMSG.Subject + m_mailAttributes.Subject.subjectstring;
                    }
                }
                if (m_subjectMailCount)
                {
                    myMSG.Subject = myMSG.Subject + "-" + GetCountWholeMailToSend().ToString();
                }
                return msg;
            }
            catch (Exception ex)
            {
                throw new AttributeNotSetException(ex.ToString());
            }
        }

        public string AddHeaderUnparsed(string sHeader, MessageWrapper msg, bool deleteMessageID = true)
        {
            try
            {
                //MessageID löschen
                if (deleteMessageID)
                {
                    sHeader = StringHelper.StringHelper.RemoveMessageID(sHeader);
                }

                foreach (MailSend.MailAttributes.SHeader header in m_mailAttributes.Headers)
                {
                    if (header.addHeaderOnce == MailAttributes.AttributeInsertMethod.addOnce)
                    {
                        if (!msg.SendOnce || !msg.MsgCached)
                        {
                            //ToDo Hier muss eigentlich noch nach schon existirenden Headern gesucht werden. 
                            sHeader = StringHelper.StringHelper.OverWriteHeaderAttributes(sHeader, header.name, header.value);
                        }
                    }
                    else if (header.addHeaderOnce == MailAttributes.AttributeInsertMethod.change)
                    {
                        sHeader = StringHelper.StringHelper.OverWriteHeaderAttributes(sHeader, header.name, header.value);
                    }
                    else if (header.addHeaderOnce == MailAttributes.AttributeInsertMethod.addAlways)
                    {
                        StringHelper.StringHelper.AddHeader(ref sHeader, header.name, header.value);
                    }
                    else if (header.addHeaderOnce == MailAttributes.AttributeInsertMethod.extent)
                    {

                        MailSend.MailAttributes.HeaderPosition myHeaderPosition = header.position;
                        sHeader = StringHelper.StringHelper.ChangeAttributes(sHeader, header.name, header.value, myHeaderPosition);
                    }
                }

                MailSend.MailAttributes.SSubject subject = m_mailAttributes.Subject;
                if (!String.IsNullOrEmpty(subject.subjectstring))
                {
                    if (subject.position != null)
                    {
                        MailSend.MailAttributes.HeaderPosition myHeaderPosition = subject.position;
                        sHeader = StringHelper.StringHelper.ChangeSubject(sHeader, subject.subjectstring, myHeaderPosition);

                    }
                    else
                    {
                        MailSend.MailAttributes.HeaderPosition myHeaderPosition = MailAttributes.HeaderPosition.end;
                        sHeader = StringHelper.StringHelper.ChangeSubject(sHeader, subject.subjectstring, myHeaderPosition);
                    }
                }
                if (m_subjectMailCount)
                {
                    sHeader = StringHelper.StringHelper.ChangeSubject(sHeader, ("-" + GetCountWholeMailToSend().ToString()), MailAttributes.HeaderPosition.end);
                }

                return sHeader;
            }
            catch (Exception ex)
            {
                return sHeader;
            }
        }

        public string AddHeaderUnparsedObsolete(String sMsg)
        {
            string newHeader = String.Empty;
            if (newHeader == String.Empty)
            {
                newHeader = StringHelper.StringHelper.MakeHeaderLine("Message-ID", Guid.NewGuid().ToString());
            }
            else
            {
                newHeader = newHeader + Environment.NewLine + StringHelper.StringHelper.MakeHeaderLine("Message-ID", Guid.NewGuid().ToString());
            }
            /*
            if (m_mailAttributes == null)
            {
                return;
            }
           */

            try
            {


                foreach (MailSend.MailAttributes.SHeader header in m_mailAttributes.Headers)
                {
                    if (newHeader == String.Empty)
                    {
                        newHeader = StringHelper.StringHelper.MakeHeaderLine(header.name, header.value);
                    }
                    else
                    {
                        newHeader = newHeader + Environment.NewLine + StringHelper.StringHelper.MakeHeaderLine(header.name, header.value);
                    }

                }
                //Noch ein NewLine hinzufügen
                newHeader = newHeader + Environment.NewLine;

                //string tempPath = System.IO.Path.GetTempPath();
                //msg.Temp = tempPath + System.Guid.NewGuid().ToString() + ".eml";
                //File.Copy(msg.MessagePath, msg.Temp, true);

                String newMSG = AddToTxtString(sMsg, newHeader, true, false, true, Tools.sMessageID, Tools.sReplaceForMessageID);
                return newMSG;
            }
            catch (Exception ex)
            {
                return sMsg;
            }
        }


        public string GetFileContents(string file_name)
        {
            System.IO.StreamReader stream_reader = new System.IO.StreamReader(file_name, true);
            string contents = stream_reader.ReadToEnd();

            stream_reader.Close();
            return contents;
        }

        public void AddToTxtFileStr(string fileName, string str, bool atTop = false, bool fDel = false, bool replace = false, string oldValue = null, string newValue = null, bool caseSensitive = false)
        {
            FileStream FiStr;
            string oldTxt = null;
            if (replace && !String.IsNullOrEmpty(oldValue) && !String.IsNullOrEmpty(newValue))
            {
                oldTxt = GetFileContents(fileName);
                if (caseSensitive)
                {
                    oldTxt.Replace(oldValue, newValue);
                }
                else
                {
                    oldTxt = StringHelper.StringHelper.ReplaceText(oldTxt, oldValue, newValue, StringHelper.StringHelper.CompareMethods.Binary);
                }
            }
            if (atTop)
            {
                // am Anfang einfügen
                if (oldTxt == null)
                {
                    oldTxt = GetFileContents(fileName);
                }
                FiStr = new FileStream(fileName, FileMode.Truncate); //neu erstellen
                StreamWriter StrWr = new StreamWriter(FiStr);
                StrWr.WriteLine(str);
                StrWr.Write(oldTxt);
                //return FiStr;
                StrWr.Close();
            }
            else
            {
                if (fDel)
                {
                    // neu einschreiben
                    FiStr = new FileStream(fileName, FileMode.Truncate);
                }
                else
                {
                    // am Ende anfügen
                    FiStr = new FileStream(fileName, FileMode.Append);
                }
                StreamWriter StrWr = new StreamWriter(FiStr);
                StrWr.WriteLine(str);
                //return FiStr;
                StrWr.Close();
            }
        }


        public string AddToTxtString(string sMailMessage, string str, bool atTop = false, bool fDel = false, bool replace = false, string oldValue = null, string newValue = null, bool caseSensitive = false)
        {
            string oldTxt = sMailMessage;
            string newTxt = str;
            if (replace && !String.IsNullOrEmpty(oldValue) && !String.IsNullOrEmpty(newValue))
            {
                if (caseSensitive)
                {
                    oldTxt.Replace(oldValue, newValue);
                }
                else
                {
                    oldTxt = StringHelper.StringHelper.ReplaceText(oldTxt, oldValue, newValue, StringHelper.StringHelper.CompareMethods.Binary);
                }
            }
            if (atTop)
            {
                // am Anfang einfügen
                newTxt = newTxt + oldTxt;
                return newTxt;
            }
            else
            {
                if (fDel)
                {
                    // neu einschreiben
                    oldTxt.Remove(0, newTxt.Length);
                    oldTxt.Insert(0, newTxt);
                }
                else
                {
                    // am Ende anfügen
                    oldTxt = oldTxt + newTxt;
                    return oldTxt;
                }
                return oldTxt;
            }
        }


        public string ReplaceMessageID(string sMailMessage, string oldValue, string newValue, bool caseSensitive = false)
        {
            string oldTxt = sMailMessage;
            if (!String.IsNullOrEmpty(oldValue) && !String.IsNullOrEmpty(newValue))
            {
                if (caseSensitive)
                {
                    oldTxt.Replace(oldValue, newValue);
                }
                else
                {
                    oldTxt = StringHelper.StringHelper.ReplaceText(oldTxt, oldValue, newValue, StringHelper.StringHelper.CompareMethods.Binary);
                }
            }
            return oldTxt;
        }

        public bool ParseMessage(MessageWrapper msg)
        {
            bool messageParsed = false;

            try
            {
                if (!msg.MsgCached && m_parsed)
                {
                    if (!m_sendOriginal)
                    {
                        msg.MailMsg = Mail_Message.ParseFromFile(msg.MessagePath);
                        messageParsed = true;
                    }
                    //here adding attributes to mail

                }
                else if (!msg.MsgCached && !m_parsed)
                {
                    msg.Parsed = false;
                }
            }
            catch (AttributeNotSetException ex)
            {
                //Attribute couldn' been set
                Log.logger.Error("Attribute couldn't been set " + msg.MessagePath + " " + ex.ToString());
            }
            catch (ArgumentException ex)
            {
                //Irgendein falscher wert in einem Argument ->Mail als fehlerhaft markieren -> weitermachen mit nächster Mail.
                messageParsed = false;
                msg.SendErrors++;
                lock (countLock)
                {
                    //m_mailState.CountWholeMailsSend++;
                    CountWholeMailToSendPlus1();

                }
                mail_Queue.Enqueue(msg);
            }

            catch (Exception ex)
            {
                //Parsing der Nachricht ging schief -> Schicke Message als Stream weg -> Markiere Nachricht Corrupted!
                Log.logger.Error("Message couldn't be parsed " + msg.MessagePath + "  " + ex.ToString());
                msg.Parsed = false;
                messageParsed = false;
                mail_Queue.Enqueue(msg);
            }

            return messageParsed;


        }

        private Stream BuildStream(MessageWrapper msg, object threadContext)
        {
            int threadIndex = (int)threadContext;
            Stream msgStream = null;
            try
            {

                if (m_parsed && !m_sendOriginal)
                {
                    if (!msg.MsgCached)
                    {
                        ParseMessage(msg);

                    }

                    if (msg.Parsed)
                    {
                        msg = AddMailAttributes(msg);
                        MemoryStream memoryStream = new MemoryStream();
                        msg.MailMsg.ToStream(memoryStream, (new MIME_Encoding_EncodedWord(MIME_EncodedWordEncoding.Q, Encoding.UTF8)), Encoding.UTF8);
                        if (!msg.SendOnce)
                        {
                            msg.CacheMSG(m_cacheMailsUntilInByte, memoryStream.Length);
                        }
                        memoryStream.Position = memoryStream.Seek(0, SeekOrigin.Begin);
                        msgStream = (Stream)memoryStream;
                    }

                    //Message lässt sich nciht parsen und sie soll trotzdem gesendet werden
                    if (m_parsed && !msg.Parsed && !m_sendOriginal && m_fallBack)
                    {

                        if (!msg.MsgCached)
                        {
                            byte[] file = File.ReadAllBytes(msg.MessagePath);
                            msg.MailMsgByte = file;
                        }

                        int bodyPosition;
                        byte[] byteHeaderSchnell = StringHelper.StringHelper.GetHeaderAndBodyPosition(msg.MailMsgByte, out bodyPosition);
                        if (byteHeaderSchnell != null)
                        {
                            msg.BodyPosition = bodyPosition;
                            System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
                            string header = enc.GetString(byteHeaderSchnell);



                            header = AddHeaderUnparsed(header, msg, true);

                            byteHeaderSchnell = enc.GetBytes(header);
                        }
                        else
                        {
                            //ToDo: kein header gefunden, erzeuge Header wenn nötig subject hinzufügen
                        }
                        msg.MailMsgByte = StringHelper.StringHelper.AddHeaderToBodyByte(byteHeaderSchnell, msg.MailMsgByte, msg.BodyPosition);



                        MemoryStream ms = new MemoryStream(msg.MailMsgByte);
                        if (!msg.SendOnce)
                        {
                            msg.CacheMSG(m_cacheMailsUntilInByte, ms.Length);
                        }
                        msgStream = (Stream)ms;



                    }
                }





                if (m_sendOriginal && !m_newMessageID && !m_parsed)//Message wird mit original MessageID gesendet
                {
                    if (!msg.MsgCached)
                    {
                        byte[] file = File.ReadAllBytes(msg.MessagePath);
                        msg.MailMsgByte = file;
                    }
                    MemoryStream ms = new MemoryStream(msg.MailMsgByte);
                    if (!msg.SendOnce)
                    {
                        msg.CacheMSG(m_cacheMailsUntilInByte, ms.Length);
                    }

                    msgStream = (Stream)ms;

                }
                else if (m_sendOriginal && m_newMessageID && !m_parsed && !m_addHeader)//Es wird im Header nur die MessageID entfernt, für maximale Performance
                {
                    if (!msg.MsgCached)
                    {

                        byte[] file = File.ReadAllBytes(msg.MessagePath);

                        msg.MailMsgByte = file;

                        int bodyPosition;
                        byte[] byteHeaderSchnell = StringHelper.StringHelper.GetHeaderAndBodyPosition(file, out bodyPosition);
                        if (byteHeaderSchnell != null)
                        {
                            msg.BodyPosition = bodyPosition;
                            System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
                            string header = enc.GetString(byteHeaderSchnell);

                            header = StringHelper.StringHelper.RemoveMessageID(header);
                            if (m_subjectMailCount)
                            {
                                header = StringHelper.StringHelper.ChangeSubject(header, ("-" + GetCountWholeMailToSend().ToString()), MailAttributes.HeaderPosition.end);
                            }
                            byteHeaderSchnell = enc.GetBytes(header);
                        }
                        msg.MailMsgByte = StringHelper.StringHelper.AddHeaderToBodyByte(byteHeaderSchnell, file, msg.BodyPosition);
                    }
                    MemoryStream ms = new MemoryStream(msg.MailMsgByte);

                    if (!msg.SendOnce)
                    {
                        msg.CacheMSG(m_cacheMailsUntilInByte, ms.Length);
                    }

                    msgStream = (Stream)ms;
                }
                else if (m_sendOriginal && m_newMessageID && !m_parsed && m_addHeader)//Es werden weitere Elemente in den Header eingefügt -> langsamer
                {
                    if (!msg.MsgCached)
                    {
                        byte[] file = File.ReadAllBytes(msg.MessagePath);
                        msg.MailMsgByte = file;

                    }

                    int bodyPosition;
                    byte[] byteHeaderSchnell = StringHelper.StringHelper.GetHeaderAndBodyPosition(msg.MailMsgByte, out bodyPosition);
                    if (byteHeaderSchnell != null)
                    {
                        msg.BodyPosition = bodyPosition;
                        System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
                        string header = enc.GetString(byteHeaderSchnell);

                        header = AddHeaderUnparsed(header, msg, true);

                        byteHeaderSchnell = enc.GetBytes(header);
                    }
                    msg.MailMsgByte = StringHelper.StringHelper.AddHeaderToBodyByte(byteHeaderSchnell, msg.MailMsgByte, msg.BodyPosition);

                    MemoryStream ms = new MemoryStream(msg.MailMsgByte);


                    if (!msg.SendOnce)
                    {
                        msg.CacheMSG(m_cacheMailsUntilInByte, ms.Length);
                    }

                    msgStream = (Stream)ms;
                }


                m_smtpClient[threadIndex].MailFrom(m_smtpFrom, -1);
                if (!m_useRecipientGroup)
                {
                    foreach (String mailTo in m_smtpTo)
                    {
                        m_smtpClient[threadIndex].RcptTo(mailTo);
                    }
                }
                else
                {
                    m_smtpClient[threadIndex].RcptTo(GetRecipientFromRecipientGroup());
                }


            }
            catch (SMTP_ClientException ex)
            {
                //m_smtpClient[threadIndex].Rset();
                //ToDoMaxSizeExceed Mail ist zu groß Fehlerbehandlung und Continue
                throw ex;
            }
            catch (Exception ex)
            {
                if (msgStream != null)
                {
                    msgStream.Close();
                }
                throw ex;
            }
            return msgStream;
        }//BuildStream

    }//end class
}
