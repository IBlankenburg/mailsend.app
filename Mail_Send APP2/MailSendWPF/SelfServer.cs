using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using MailSend;
using System.Threading;
using System.IO;
using MailSendWPF.Server;
using System.ServiceModel;
using System.Collections.ObjectModel;
using System.Diagnostics;
using MailSendWPF.Tools;
using MailSendWPF.Interfaces;
using MailSendWPF.Configuration;

namespace MailSendWPF
{
    public class SelfServer : IServer
    {
        private SelfSmtp[] m_smtpClient = null;
        private SelfKMS[] m_KMSClient = null;
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
        private string m_smtpToString = "";
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

        //private MailAttributes m_mailAttributes = new MailAttributes();
        private MailAttributes[] m_mailAttributes;
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
        //SenderGroup
        private bool m_useSenderGroup = false;
        private string m_senderGroup = String.Empty;
        private int m_senderGroupStart = 0;
        private int m_senderGroupEnd = 0;
        private List<string> m_sendersGroupList = null;
        private List<string> m_tenantGroupList = null;
        private int m_senderGroupIndex = 0;
        private int m_tenantGroupIndex = 0;
        private Object m_senderGroupIndexLock = new Object();
        private Object m_tenantGroupIndexLock = new Object();
        private Object m_makeCyclicCertsForAllTenantsSameLock = new Object();
        private string[] m_sender; //= null; //= String.Empty;
        private string[] m_recipient;// = null; // = String.Empty;

        private string[] m_tenant;


        private bool m_subjectMailCount = false;
        private StringHelper.StringHelper.ESubjectAddMailNamePosition m_subjectAddMailNamePosition;
        private bool m_addNotExistingHeader = false;
        private bool m_useTLS = false;
        private bool m_TLSSecured = false;
        private bool m_ignoreAllErrors = false;
        private bool m_useSSLSecurePort = false;
        private bool m_SSLSecuredSecuredPort = false;

        int m_sendTimeout = 50000;
        int m_receiveTimeout = 50000;
        int m_sendBuffer = 8192;
        int m_receiveBuffer = 8192;

        //Zum Experimentieren
        //ToDo: Deaktivieren, ist nur für Experiment gedacht macht alles langsam!!!
        private bool isExperimentalParserTest = true;

        private MailSend.Constants.KeyManager m_keyManager;

        //public delegate void RequestSendHandler(MessageWrapper msg, MailState mailState);
        //public delegate void RequestSendFailedHandler(MessageWrapper msg, MailState mailState);
        override public event IServer.RequestSendHandler RequestSendEvent;
        override public void OnRequestSent(MessageWrapper mail, MailState mailState)
        {
            IServer.RequestSendHandler myEvent = RequestSendEvent;
            if (myEvent != null)
            {
                myEvent(mail, mailState);
            }
        }

        override public event IServer.RequestSendFailedHandler RequestSendFailedEvent;
        override public void OnRequestSentFailed(MessageWrapper mail, MailState mailState)
        {
            IServer.RequestSendFailedHandler myEvent = RequestSendFailedEvent;
            if (myEvent != null)
            {
                myEvent(mail, mailState);
            }
        }

        //MailSendEvent
        //public delegate void MailsendHandler(MessageWrapper msg, MailState mailState);
        override public event IServer.MailsendHandler MailsendEvent;
        override public void OnMailSent(MessageWrapper mail, MailState mailState)
        {
            IServer.MailsendHandler myEvent = MailsendEvent;
            if (myEvent != null)
            {
                myEvent(mail, mailState);
            }
        }

        //public delegate void MaxMailsToSendHandler(MailState mailState);
        override public event IServer.MaxMailsToSendHandler MaxMailsToSendEvent;
        override public void OnMaxMailsToSend(MailState mailState)
        {
            IServer.MaxMailsToSendHandler myEvent = MaxMailsToSendEvent;
            if (myEvent != null)
            {
                myEvent(mailState);
            }
        }

        //public delegate void SendMailEndsHandler(MailState sendMailEndsStatus);
        override public event IServer.SendMailEndsHandler SendMailEndsEvent;
        override public void OnSendMailEnds(MailState sendMailEndsStatus)
        {
            IServer.SendMailEndsHandler myEvent = SendMailEndsEvent;
            if (myEvent != null)
            {
                myEvent(sendMailEndsStatus);
            }
        }

        //public delegate void BeforeMailsentHandler(MessageWrapper msg, MailState mailState);
        override public event IServer.BeforeMailsentHandler BeforeMailsentEvent;
        override public void OnBeforeMailSent(MessageWrapper mail, MailState mailState)
        {
            IServer.BeforeMailsentHandler myEvent = BeforeMailsentEvent;
            if (myEvent != null)
            {
                myEvent(mail, mailState);
            }
        }

        override public event IServer.StatusMessageHandler StatusMessageEvent;
        override public void OnStatusMessage(string message, string key)
        {
            IServer.StatusMessageHandler myEvent = StatusMessageEvent;
            if (myEvent != null)
            {
                myEvent(message, key);
            }
        }

        public SelfServer()
        {
            //this.m_senderIPAddress = IPAddress.Any;
        }

        public SelfServer(IPAddress smartHostIPAddress, int port, string smtpFrom, string smtpTo, string smartHostName, string smartHostPassword, ulong rounds, int connections, bool endlessSending)
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
        public SelfServer(IPAddress smartHostIPAddress, int port, string smtpFrom, string smtpTo, string smartHostName, string smartHostPassword, ulong rounds, int connections, bool endlessSending, MailAttributes mailAttributes, bool parsed, int waitBetweenMailsms, bool sendOriginal, bool newMessageID, bool fallBack, string id, bool addSenderToMail, bool addRecipientToMail, bool anonymousAuth, string host, long cacheMailsUntilInByte, bool codePageEnabled, int codePage, string recipientGroup, int recipientGroupStart, int recipientGroupEnd, string recipientGroupDomain, bool useRecipientGroup, bool subjectMailCount, StringHelper.StringHelper.ESubjectAddMailNamePosition subjectAddMailNamePosition, string senderGroup, int senderGroupStart, int senderGroupEnd, string senderGroupDomain, bool useSenderGroup, bool addHeader, bool UseNoneSSL, bool UseSMTPS, bool UseTLS, bool IgnoreAllErrors, int sendTimout, int receiveTimeout, int sendBuffer, int receiveBuffer)
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
            m_smtpToString = smtpTo;
            m_smartHostName = smartHostName;



            m_smartHostPassword = smartHostPassword;
            m_mailState.RoundOfMailsToSend = rounds;
            m_numberConnections = connections;
            m_sender = new string[connections];
            m_recipient = new string[connections];
            m_mailAttributes = new MailAttributes[connections];
            m_endlessSending = endlessSending;
            if ((String.IsNullOrEmpty(smartHostName) || smartHostPassword == null))
            {
                m_anonymousAuthentication = true;
            }
            if (mailAttributes != null)
            {
                for (int i = 0; i < m_mailAttributes.Length; i++)
                {
                    m_mailAttributes[i] = mailAttributes;
                    if (m_mailAttributes[i].Headers.Count > 0)
                    {
                        m_addHeader = true;
                    }
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
                /*
                for (int i = 0; i < m_mailAttributes.Length; i++)
                {
                    MailSend.MailAttributes.SHeader myHeader = new MailAttributes.SHeader();
                    myHeader.addHeaderOnce = MailAttributes.AttributeInsertMethod.change;
                    myHeader.name = Tools.sMailTo;
                    myHeader.value = smtpTo;
                    m_mailAttributes[i].Headers.Add(myHeader);
                    m_addHeader = true;
                }
                 */
                m_addHeader = true;
            }
            if (m_addSenderToMail)
            {
                /*
                for (int i = 0; i < m_mailAttributes.Length; i++)
                {
                    MailSend.MailAttributes.SHeader myHeader = new MailAttributes.SHeader();
                    myHeader.addHeaderOnce = MailAttributes.AttributeInsertMethod.change;
                    myHeader.name = Tools.sMailFrom;
                    myHeader.value = m_smtpFrom;
                    m_mailAttributes[i].Headers.Add(myHeader);
                    m_addHeader = true;
                }
                 */
                m_addHeader = true;
            }


            m_mailState.EndlessSending = endlessSending;
            m_anonymousAuthentication = anonymousAuth;
            m_cacheMailsUntilInByte = cacheMailsUntilInByte;
            m_codePageEnabled = codePageEnabled;
            m_codePage = codePage;
            if (useRecipientGroup)
            {
                if (recipientGroup != null)
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
            }
            //SenderGroup
            if (useSenderGroup)
            {
                if (senderGroup != null)
                {
                    if (senderGroupStart <= senderGroupEnd)
                    {
                        m_useSenderGroup = true;
                        int capacy = senderGroupEnd - senderGroupStart + 1;
                        m_sendersGroupList = new List<string>(capacy);
                        m_senderGroupIndex = 0;

                        for (int i = senderGroupStart; i <= senderGroupEnd; i++)
                        {
                            m_sendersGroupList.Add(senderGroup.Trim() + i.ToString().Trim() + "@" + senderGroupDomain.Trim());
                        }
                    }
                }
            }

            m_subjectMailCount = subjectMailCount;
            m_subjectAddMailNamePosition = subjectAddMailNamePosition;
            m_addNotExistingHeader = addHeader;

            m_ignoreAllErrors = IgnoreAllErrors;
            if (UseNoneSSL)
            {
                m_SSLSecuredSecuredPort = false;
                m_useTLS = false;
            }
            else
            {
                m_SSLSecuredSecuredPort = UseSMTPS;
                m_useTLS = UseTLS;
            }
            m_sendBuffer = sendBuffer;
            m_receiveBuffer = receiveBuffer;
            m_sendTimeout = sendTimout;
            m_receiveTimeout = receiveTimeout;
        }
        /*
        public SelfServer(IPAddress smartHostIPAddress, int port, string smtpFrom, string smtpTo, string smartHostName, string smartHostPassword, ulong rounds, int connections, bool endlessSending, MailAttributes mailAttributes)
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
                for (int i = 0; i < m_mailAttributes.Length; i++)
                {
                    m_mailAttributes[i] = mailAttributes;
                }
            }

        }
        */
        override public void SetKeyManager(MailSend.Constants.KeyManager keyManager)
        {
            m_keyManager = keyManager;
            m_tenantGroupList = new List<string>(keyManager.Tenants.Count);
            foreach (ServerSchemaTenants serverSchemaTenant in keyManager.Tenants)
            {
                m_tenantGroupList.Add(serverSchemaTenant.TenantValue);
            }
            if (keyManager.Tenants.Count < 1)
            {
                m_tenantGroupList.Add("");
            }

            string transport = "";
            if (m_keyManager.useHttps)
            {
                transport = "https://";
            }
            else
            {
                transport = "http://";
            }

            if (m_smartHostIPAddress != null)
            {
                m_keyManager.publicHost = transport + m_smartHostIPAddress.ToString() + ":" + m_smartHostPort + "/km.backend/services/KMSBackendPublicWebService";
                m_keyManager.privateHost = transport + m_smartHostIPAddress.ToString() + ":" + m_smartHostPort + "/km.backend/services/KMSBackendPrivateWebService";
            }
            else if (!String.IsNullOrEmpty(m_host))
            {
                m_keyManager.publicHost = transport + m_host + ":" + m_smartHostPort + "/km.backend/services/KMSBackendPublicWebService";
                m_keyManager.privateHost = transport + m_host + ":" + m_smartHostPort + "/km.backend/services/KMSBackendPrivateWebService";
            }
            m_tenant = new string[m_numberConnections];
            //m_keyManager.
        }
        private void RecipientIndexPlusPlus()
        {
            lock (m_recipientGroupIndexLock)
            {
                m_recipientGroupIndex++;
            }
        }
        private string GetRecipientFromRecipientGroup(ref bool newTurn)
        {
            string recipient = String.Empty;
            lock (m_recipientGroupIndexLock)
            {
                if (m_recipientGroupIndex == 0)
                {
                    newTurn = true;
                }

                recipient = (string)m_recipientsGroupList[m_recipientGroupIndex];
                //m_recipient = recipient;
                if (m_recipientGroupIndex < (m_recipientsGroupList.Count - 1))
                {
                    m_recipientGroupIndex++;
                }
                else
                {
                    m_recipientGroupIndex = 0;
                    //newTurn = true;
                }

            }
            return recipient;
        }

        private string GetSenderFromSenderGroup(ref bool newTurn)
        {
            string sender = String.Empty;
            bool changeTenant = false;
            lock (m_senderGroupIndexLock)
            {
                /*
                if (m_senderGroupIndex == 0)
                {
                    newTurn = true;
                }
                 */
                sender = (string)m_sendersGroupList[m_senderGroupIndex];
                //BUG
                //m_sender = sender;
                if (m_senderGroupIndex < (m_sendersGroupList.Count - 1))
                {
                    m_senderGroupIndex++;
                }
                else
                {
                    m_senderGroupIndex = 0;
                    newTurn = true;
                    //changeTenant = true;

                }
                /*
                if (m_senderGroupIndex == 1)
                {

                }
                */
            }
            return sender;
        }

        private string GetTenantFromTenantGroup(ref bool newTurn)
        {
            string tenant = String.Empty;
            lock (m_tenantGroupIndexLock)
            {
                if (m_tenantGroupIndex == 0)
                {
                    newTurn = true;
                }
                tenant = (string)m_tenantGroupList[m_tenantGroupIndex];
                //BUG
                //m_sender = sender;
                if (m_tenantGroupIndex < (m_tenantGroupList.Count - 1))
                {
                    m_tenantGroupIndex++;
                }
                else
                {
                    m_tenantGroupIndex = 0;
                    //newTurn = true;
                }

            }
            return tenant;
        }

        private string GetTenantFromTenantGroupWithoutCount()
        {
            string tenant = String.Empty;
            lock (m_tenantGroupIndexLock)
            {
                tenant = (string)m_tenantGroupList[m_tenantGroupIndex];
                //BUG
                //m_sender = sender;
            }
            return tenant;
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

        byte[] CreateTestMail()
        {
            byte[] testMailByte = Encoding.UTF8.GetBytes(TestMail.WholeTestMail);
            return testMailByte;
        }

        //Bei Rekursion wird nciht geprüft ob ein angegebenes Verzeichnis ein anderes enthält. Enthaltene Emls werden dann doppelt vorkommen
        override public ulong FillQueue(List<string> dirs, bool recursive, List<string> filters, ulong connections, bool endlessSending, string key)
        {
            int countDirs = dirs.Count;
            synchroniseCountMails = new Dictionary<ulong, ulong>(countDirs);
            SearchOption mySearchOption = SearchOption.TopDirectoryOnly;
            ulong tooManyConnectionCounter = 0;
            if (countDirs > 1)
            {
                //dirs = Tools.RemoveDoubleItems<string>(dirs);
            }
            if (countDirs < 1)
            {

                while (connections > m_mailState.WholeMailsToSend)
                {
                    MessageWrapper testMessage = new MessageWrapper("TestMessage");
                    testMessage.IsTestMessage = true;
                    mail_Queue.Enqueue(testMessage);
                    testMessage.MailID = 0;
                    if (tooManyConnectionCounter < 1)
                    {
                        synchroniseCountMails.Add(testMessage.MailID, 0);
                    }
                    tooManyConnectionCounter++;
                    m_mailState.WholeMailsToSend++;
                }
                if (tooManyConnectionCounter > 1)
                {
                    m_mailState.WholeMailsToSend = m_mailState.WholeMailsToSend / tooManyConnectionCounter;

                }
                m_mailState.WholeMailsToSend = 1;
                m_mailState.WholeMailsToSend = m_mailState.WholeMailsToSend * m_mailState.RoundOfMailsToSend;
                Log.logger.Info("Anzahl der Durchläufe der Testmail: " + m_mailState.RoundOfMailsToSend);
                Log.logger.Info("Gesamtanzahl der TestMail Nachrichten: " + m_mailState.WholeMailsToSend);
                OnMaxMailsToSend(m_mailState);
                return m_mailState.WholeMailsToSend;
            }

            if (recursive)
            {
                mySearchOption = SearchOption.AllDirectories;
            }

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
                        OnStatusMessage("Directory/File " + dir + " doesn't exist", key);

                        // m_mailState.who
                    }
                }
                if (m_mailState.WholeMailsToSend <= 0)
                {
                    //Nachricht anzeigen das keine Mail oder Mail im angegebenen Verzeichnis vorhanden ist
                    OnStatusMessage("No files exist, please check paths", key);
                    return 0;
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
            m_smtpClient = new SelfSmtp[m_numberConnections];
            doneEvents = new ManualResetEvent[m_numberConnections];
            Thread[] myThread = new Thread[m_numberConnections];
            //System.ServiceModel.BasicHttpBinding bindingn = new System.ServiceModel.BasicHttpBinding();
            //bindingn.TransferMode = System.ServiceModel.TransferMode.Streamed;

            try
            {
                for (int i = 0; i < m_numberConnections; i++)
                {
                    //mailThreads[i] = new Thread(new ThreadStart(StartSend));
                    doneEvents[i] = new ManualResetEvent(false);
                    m_smtpClient[i] = new SelfSmtp();
                    //m_smtpClient[i].Timeout = 50000;
                    m_smtpClient[i].RecieveTimeout = m_receiveTimeout;
                    m_smtpClient[i].SendTimeout = m_sendTimeout;
                    m_smtpClient[i].SendBufferSize = m_sendBuffer;
                    m_smtpClient[i].ReceiveBufferSize = m_receiveBuffer;
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


        override public void SetKeyManagerAndSend()
        {

            //mailThreads = new Thread[m_numberConnections];
            m_KMSClient = new SelfKMS[m_numberConnections];
            doneEvents = new ManualResetEvent[m_numberConnections];
            Thread[] myThread = new Thread[m_numberConnections];
            //System.ServiceModel.BasicHttpBinding bindingn = new System.ServiceModel.BasicHttpBinding();
            //bindingn.TransferMode = System.ServiceModel.TransferMode.Streamed;
            //bindingn.Security = 




            //client.InnerChannel.

            try
            {
                //KMS.KMSBackendPrivateWebServicePortTypeClient client = new KMS.KMSBackendPrivateWebServicePortTypeClient("/km.backend/services/KMSBackendPrivateWebService", @"http://172.30.200.133:8080");
                //EndpointAddress endPointAddress = new EndpointAddress(@"http://172.30.200.133:8080/km.backend/services/KMSBackendPrivateWebService");
                for (int i = 0; i < m_numberConnections; i++)
                {
                    //mailThreads[i] = new Thread(new ThreadStart(StartSend));
                    doneEvents[i] = new ManualResetEvent(false);
                    m_KMSClient[i] = new SelfKMS();
                    //m_smtpClient[i].Timeout = 50000;
                    m_KMSClient[i].RecieveTimeout = m_receiveTimeout;
                    m_KMSClient[i].SendTimeout = m_sendTimeout;
                    m_KMSClient[i].SendBufferSize = m_sendBuffer;
                    m_KMSClient[i].ReceiveBufferSize = m_receiveBuffer;
                    ParameterizedThreadStart threadStart = new ParameterizedThreadStart(StartKeyManagerSendNew);
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
                        try
                        {
                            if (!m_smtpClient[threadIndex].IsConnected)
                            {
                                Connect(threadContext);
                            }
                            if (!m_smtpClient[threadIndex].IsEhloHeloSent && !m_useSSLSecurePort)
                            {
                                m_smtpClient[threadIndex].SendHELOEHLO(m_senderIPAddress.ToString());
                            }
                            if (m_useTLS && !m_TLSSecured)
                            {
                                m_TLSSecured = m_smtpClient[threadIndex].StartTLS(m_senderIPAddress.ToString(), m_smartHostIPAddress, m_host, m_smartHostName, m_smartHostPassword);
                            }
                            if (m_useSSLSecurePort && !m_SSLSecuredSecuredPort)
                            {
                                m_SSLSecuredSecuredPort = m_smtpClient[threadIndex].StartSSLSecurePort(m_senderIPAddress.ToString(), m_smartHostIPAddress, m_host, m_smartHostName, m_smartHostPassword);
                            }
                            if (!m_smtpClient[threadIndex].IsAuthenticated)
                            {
                                Authenticate(threadContext);

                            }

                            SendMessageNew(threadContext);
                        }
                        catch (AllMailsSendException ex)
                        {
                            throw ex;
                        }
                        catch (Exception ex)
                        {
                            if (!m_ignoreAllErrors)
                            {
                                Log.logger.Error(ex.ToString());
                                throw ex;
                            }
                            Disconnect(threadIndex);
                        }
                    }//end while

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
                m_mailState.Ex.Add(ex);
                doneEvents[threadIndex].Set();


            }
            catch (Exception ex)
            {

                m_mailState.Ex.Add(ex);
                Log.logger.Fatal(ex.ToString());
                Log.logger.Fatal(new System.Diagnostics.StackTrace().ToString());
                doneEvents[threadIndex].Set();
                Disconnect(threadIndex);
                /*
                if (m_smtpClient[threadIndex].IsConnected)
                {
                    m_smtpClient[threadIndex].Disconnect();
                }
                */
                m_smtpClient[threadIndex].Dispose();
                //Error.DumpError(x, new System.Diagnostics.StackTrace());
                //End(false, x);
            }

            finally
            {
                //Thread soll Event schmeissen und bekanntgeben das er fertig ist
                OnSendMailEnds(m_mailState);
                Disconnect(threadIndex);
                /*
                if (m_smtpClient[threadIndex].IsConnected)
                {
                    m_smtpClient[threadIndex].Disconnect();
                }
                 */
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

        public void StartKeyManagerSendNew(Object threadContext)
        {
            int threadIndex = (int)threadContext;
            try
            {

                // Smart host enabled, use it
                if (m_useSmartHost)
                {
                    while (!_shouldStop)
                    {
                        try
                        {
                            if (!m_KMSClient[threadIndex].IsConnected)
                            {
                                m_KMSClient[threadIndex].KeyManagerConnect(m_keyManager);
                            }
                            SendKeyManagerRequests(threadContext);
                        }
                        catch (AllMailsSendException ex)
                        {
                            throw ex;
                        }
                        catch (Exception ex)
                        {
                            if (!m_ignoreAllErrors)
                            {
                                Log.logger.Error(ex.ToString());
                                throw ex;
                            }
                            //Disconnect(threadIndex);
                        }
                    }//end while

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
                m_mailState.Ex.Add(ex);
                doneEvents[threadIndex].Set();


            }
            catch (Exception ex)
            {

                m_mailState.Ex.Add(ex);
                Log.logger.Fatal(ex.ToString());
                Log.logger.Fatal(new System.Diagnostics.StackTrace().ToString());
                doneEvents[threadIndex].Set();
                //Disconnect(threadIndex);
                m_KMSClient[threadIndex].Dispose();
            }

            finally
            {
                //Thread soll Event schmeissen und bekanntgeben das er fertig ist
                OnSendMailEnds(m_mailState);
                //Disconnect(threadIndex);
            }
        }


        public void Disconnect(int threadIndex)
        {
            //if (m_smtpClient[threadIndex].IsConnected)
            {
                m_smtpClient[threadIndex].Disconnect();
            }
            m_TLSSecured = false;
            m_useSSLSecurePort = false;
        }
        public void DisconnectKMS(int threadIndex)
        {
            //if (m_smtpClient[threadIndex].IsConnected)
            {
                m_KMSClient[threadIndex].Disconnect();
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
                SelfSmtp client = m_smtpClient[threadIndex];



                try
                {

                    msg = GetMessage();
                    if (msg == null)
                    {
                        //break;
                        throw new AllMailsSendException("No more Message in Queue");
                    }
                    OnBeforeMailSent(msg, m_mailState);
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



                /* ToDo Was passiert wenn immer ein Fehler geschmissen wird
                 * Es muss die genauere Fehlerursache gesucht werden, bei SmtpException muss auch die Mail als fehlerhaft markiert werden und evtl. die nächste Mail geholt werden.
                 * Anzahl der versuche muss definiert werden!!!!
                 */
                //----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
                try
                {
                    byte[] msgStream = BuildStream(msg, threadContext);

                    if (m_waitBetweenMailsms <= 0)
                    {
                        Log.logger.Info(msg.MessagePath);

                        if (IsMessageToSend(msg))
                        {
                            try
                            {
                                //Beide Kommandos gibt es 4 mal :(
                                SendFromAndReciepe(threadIndex);
                                m_smtpClient[threadIndex].SendMessage(msgStream);
                            }
                            catch (IOException ex)
                            {
                                Log.logger.Debug("Retry after IOException!!!");
                                Disconnect(threadIndex);
                                ConnectAndAuthenticate(threadContext, threadIndex);
                                SendFromAndReciepe(threadIndex);
                                m_smtpClient[threadIndex].SendMessage(msgStream);

                            }
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
                                try
                                {
                                    //Beide Kommandos gibt es 4 mal :(
                                    SendFromAndReciepe(threadIndex);
                                    m_smtpClient[threadIndex].SendMessage(msgStream);
                                }
                                catch (IOException ex)
                                {
                                    Log.logger.Debug("Retry after IOException!!!");
                                    Disconnect(threadIndex);
                                    ConnectAndAuthenticate(threadContext, threadIndex);
                                    SendFromAndReciepe(threadIndex);
                                    m_smtpClient[threadIndex].SendMessage(msgStream);
                                }
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
                    SendingErrorHandlingNew(ref msg, ex, threadContext);

                }
                catch (SmtpException ex)
                {
                    //ToDo SMTP Server gibt Fehler zurück
                    SendingErrorHandlingNew(ref msg, ex, threadContext);

                }
                /*
            catch (LumiSoft.Net.IO.LineSizeExceededException ex)
            {
                SendingErrorHandlingNew(ref msg, ex, threadContext);

            }
                 */
                catch (AllMailsSendException ex)
                {
                    throw ex;
                }
                catch (Exception ex)
                {
                    SendingErrorHandlingNew(ref msg, ex, threadContext);

                }

                /*
                if (msgStream != null)
                {
                    msgStream.Close();
                }
                */

                if (!(msg.MsgCached))
                {
                    //msg.MailMsg = null;
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
                //ToDo: Experimentell eine Hierrarchie höher verschoben
                //doneEvents[threadIndex].Set();
                if (ex is NoConnectionPossibleException)
                {
                    throw ex;
                }
                else
                {
                    //ToDo: Experimentelles wefen aller Exceptions hinzugefügt
                    throw ex;
                }
            }

        }

        public void SendKeyManagerRequests(object threadContext)
        {
            int threadIndex = (int)threadContext;
            MessageWrapper msg = null;
            try
            {
                try
                {

                    msg = GetMessage();
                    if (msg == null)
                    {
                        //break;
                        throw new AllMailsSendException("No more Message in Queue");
                    }
                    OnBeforeMailSent(msg, m_mailState);
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
                try
                {
                    if (m_useRecipientGroup)
                    {
                        bool newturnRecipientGroup = false;
                        m_recipient[threadIndex] = GetRecipientFromRecipientGroup(ref newturnRecipientGroup);
                    }
                    bool newturnSenderGroup = false;
                    if (m_useSenderGroup)
                    {
                        lock (m_makeCyclicCertsForAllTenantsSameLock)
                        {
                            m_sender[threadIndex] = GetSenderFromSenderGroup(ref newturnSenderGroup);

                            //if (m_keyManager.KeyManagerTest)
                            //{

                            if (m_keyManager.MakeCyclicCertsForAllTenantsSame)
                            {
                                if (newturnSenderGroup)
                                {
                                    bool newturnTenantgroup = false;
                                    m_tenant[threadIndex] = GetTenantFromTenantGroup(ref newturnTenantgroup);
                                }
                                else
                                {
                                    m_tenant[threadIndex] = GetTenantFromTenantGroupWithoutCount();
                                }
                            }
                            else
                            {
                                bool newturnTenantgroup = false;
                                m_tenant[threadIndex] = GetTenantFromTenantGroup(ref newturnTenantgroup);
                            }
                            //}

                        }
                    }

                    if (msg.IsTestMessage && !msg.SendOnce)
                    {
                        msg.MsgCached = true;
                        msg.SendOnce = true;
                        //msg.MailMsgByte = CreateTestMail();

                    }
                    if (m_keyManager.GetPrivateKeyFromEnvelopeRequest || m_keyManager.GetX509CertificateForVerificationBySignedDataRequest)
                    {
                        byte[] msgStream = BuildStreamKMS(msg, threadContext);
                    }

                    if (m_waitBetweenMailsms <= 0)
                    {
                        Log.logger.Info(msg.MessagePath);

                        if (IsKeyManagerRequestToDo())
                        {
                            //m_smtpClient[threadIndex].SendMessage(msgStream);
                            m_KMSClient[threadIndex].SendRequests(m_tenant[threadIndex], m_sender[threadIndex], m_keyManager, ref msg);



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
                            Log.logger.Debug("Message is not allowed to be sent again " + msg.MessagePath + " Size: " + msg.MsgSize + " ErrorMail: " + msg.SendErrors + " Number of Exceptions: " + msg.Ex.Count);
                            throw new AllMailsSendException("All requests created");
                        }
                    }
                    else
                    {
                        lock (sendLock)
                        {


                            if (IsKeyManagerRequestToDo())
                            {
                                Thread.Sleep(m_waitBetweenMailsms);
                                Log.logger.Info(msg.MessagePath);
                                //Console.WriteLine(msg.MessagePath);
                                //m_smtpClient[threadIndex].SendMessage(msgStream);
                                m_KMSClient[threadIndex].SendRequests(m_tenant[threadIndex], m_sender[threadIndex], m_keyManager, ref msg);
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
                                Log.logger.Debug("Message is not alloed to be sent again " + msg.MessagePath + " Size: " + msg.MsgSize + " ErrorMail: " + msg.SendErrors + " Number of Exceptions: " + msg.Ex.Count);
                                throw new AllMailsSendException("All requests created");
                            }
                        }
                    }

                }
                catch (InvalidOperationException ex)
                {
                    //client nicht verbunden -> neu verbinden
                    SendingErrorHandlingNewKMS(ref msg, ex, threadContext);

                }
                catch (SmtpException ex)
                {
                    //ToDo SMTP Server gibt Fehler zurück
                    SendingErrorHandlingNewKMS(ref msg, ex, threadContext);

                }
                catch (AllMailsSendException ex)
                {
                    throw ex;
                }
                catch (Exception ex)
                {
                    throw ex;
                    //SendingErrorHandlingNew(ref msg, ex, threadContext);

                }

                lock (m_dequeueLock)
                {
                    mail_Queue.Enqueue(msg);
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
                //ToDo: Experimentell eine Hierrarchie höher verschoben
                //doneEvents[threadIndex].Set();
                if (ex is NoConnectionPossibleException)
                {
                    throw ex;
                }
                else
                {
                    //ToDo: Experimentelles wefen aller Exceptions hinzugefügt
                    throw ex;
                }
            }

        }
        public void SendingErrorHandlingNew(ref MessageWrapper msg, Exception ex, object threadContext)
        {
            int threadIndex = (int)threadContext;
            Log.logger.Fatal(ex.ToString());
            Log.logger.Fatal(new System.Diagnostics.StackTrace().ToString());
            msg.Ex.Add(ex);
            msg.Corrupt = true;
            MailState.errorMails.Add(msg);
            msg.AnzGesendet++;

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
            //m_smtpClient[threadIndex].Disconnect();
            Disconnect(threadIndex);
        }
        public void SendingErrorHandlingNewKMS(ref MessageWrapper msg, Exception ex, object threadContext)
        {
            int threadIndex = (int)threadContext;
            Log.logger.Fatal(ex.ToString());
            Log.logger.Fatal(new System.Diagnostics.StackTrace().ToString());
            msg.Ex.Add(ex);
            msg.Corrupt = true;
            MailState.errorMails.Add(msg);
            msg.AnzGesendet++;

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
            //m_smtpClient[threadIndex].Disconnect();
            DisconnectKMS(threadIndex);
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

                    //m_pRemoteEndPoint = (IPEndPoint)m_pSmtpClient.RemoteEndPoint;
                    if (!m_anonymousAuthentication && !m_smtpClient[threadIndex].IsAuthenticated)
                    {
                        m_smtpClient[threadIndex].AuthLogin(m_smartHostName, m_smartHostPassword);
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

        public bool IsKeyManagerRequestToDo()
        {
            //Beim Kms muss nach jedem Senden eins hochgezählt werden, nur senden wenn Anzahl der Requests noch nicht erreicht
            if (!m_endlessSending)
            {
                lock (semSychroniseCountMails)
                {
                    ulong countSend = m_keyManager.CountRequests;//synchroniseCountMails[msg.MailID];
                    if (countSend < m_mailState.WholeMailsToSend)
                    {
                        countSend++;
                        m_keyManager.CountRequests = countSend;
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
            //m_smtpClient[threadIndex].Disconnect();
            Disconnect(threadIndex);
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
            //m_smtpClient[threadIndex].Disconnect();
            Disconnect(threadIndex);
            Connect(threadContext);
            Authenticate(threadContext);
        }
        public void SendFromAndReciepe(int threadIndex)
        {
            if (!m_useSenderGroup)
            {
                m_smtpClient[threadIndex].MailFrom(m_smtpFrom, -1);
            }
            else
            {
                //m_smtpClient[threadIndex].MailFrom(GetSenderFromSenderGroup());
                m_smtpClient[threadIndex].MailFrom(m_sender[threadIndex]);
            }
            if (!m_useRecipientGroup)
            {
                foreach (String mailTo in m_smtpTo)
                {
                    m_smtpClient[threadIndex].RcptTo(mailTo);
                }
            }
            else
            {
                //m_smtpClient[threadIndex].RcptTo(GetRecipientFromRecipientGroup());
                m_smtpClient[threadIndex].RcptTo(m_recipient[threadIndex]);
            }
        }

        public void ConnectAndAuthenticate(Object threadContext, int threadIndex)
        {
            try
            {
                if (!m_smtpClient[threadIndex].IsConnected)
                {
                    Connect(threadContext);
                }
                if (!m_smtpClient[threadIndex].IsEhloHeloSent && !m_useSSLSecurePort)
                {
                    m_smtpClient[threadIndex].SendHELOEHLO(m_senderIPAddress.ToString());
                }
                if (m_useTLS && !m_TLSSecured)
                {
                    m_TLSSecured = m_smtpClient[threadIndex].StartTLS(m_senderIPAddress.ToString(), m_smartHostIPAddress, m_host, m_smartHostName, m_smartHostPassword);
                }
                if (m_useSSLSecurePort && !m_SSLSecuredSecuredPort)
                {
                    m_SSLSecuredSecuredPort = m_smtpClient[threadIndex].StartSSLSecurePort(m_senderIPAddress.ToString(), m_smartHostIPAddress, m_host, m_smartHostName, m_smartHostPassword);
                }
                if (!m_smtpClient[threadIndex].IsAuthenticated)
                {
                    Authenticate(threadContext);

                }


            }
            catch (Exception ex)
            {
                Log.logger.Error(ex.ToString());
                throw ex;
            }
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
                            m_smtpClient[threadIndex].Connect(new IPEndPoint(m_senderIPAddress, 0), new IPEndPoint(m_smartHostIPAddress, m_smartHostPort));
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
                    //triesToConnect++;
                    //Log.logger.Debug("Endless LOOP");
                    triesToConnect++;
                }
                catch (Exception ex)
                {
                    Log.logger.Error(ex.ToString());
                    connectionError = true;

                    m_smtpClient[threadIndex].IsConnected = false;
                    triesToConnect++;
                    if (triesToConnect > 1)
                    {
                        throw new NoConnectionPossibleException(ex);
                    }
                }
            }
            while (connectionError && triesToConnect < 2);
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




        public string AddHeaderUnparsed(string sHeader, MessageWrapper msg, int threadIndex, bool deleteMessageID = true)
        {
            try
            {
                bool isNewHeader = false;
                if (sHeader.Equals("\r\n"))
                {
                    sHeader = String.Empty;
                    isNewHeader = true;
                }
                if (sHeader == String.Empty)
                {
                    isNewHeader = true;
                }
                //MessageID löschen
                if (deleteMessageID)
                {
                    sHeader = StringHelper.StringHelper.RemoveMessageID(sHeader);
                }
                /*
                if (m_addSenderToMail)
                {
                    for (int i = 0; i < m_mailAttributes.Length; i++)
                    {
                        MailSend.MailAttributes.SHeader myHeader = new MailAttributes.SHeader();
                        myHeader.addHeaderOnce = MailAttributes.AttributeInsertMethod.change;
                        myHeader.name = Tools.sMailFrom;
                        myHeader.value = m_smtpFrom;
                        m_mailAttributes[i].Headers.Add(myHeader);
                        m_addHeader = true;
                    }
                }
                 */
                //MailSend.MailAttributes.SHeader recipientHeader = new MailAttributes.SHeader();
                if (m_addRecipientToMail)
                {
                    if (m_useRecipientGroup)
                    {
                        /*
                        //m_recipient = GetRecipientFromRecipientGroup();
                        MailSend.MailAttributes.SHeader myHeader = new MailAttributes.SHeader();
                        myHeader.addHeaderOnce = MailAttributes.AttributeInsertMethod.change;
                        myHeader.name = Tools.sMailTo;
                        myHeader.value = m_recipient[threadIndex];
                        m_mailAttributes[threadIndex].Headers.Clear();
                        m_mailAttributes[threadIndex].Headers.Add(myHeader);
                         */
                        sHeader = StringHelper.StringHelper.OverWriteHeaderAttributes(sHeader, Constants.sMailTo, m_recipient[threadIndex]);

                    }
                    else
                    {
                        sHeader = StringHelper.StringHelper.OverWriteHeaderAttributes(sHeader, Constants.sMailTo, m_smtpToString);
                    }


                }

                if (m_addSenderToMail)
                {
                    if (m_useSenderGroup)
                    {
                        /*
                        //m_sender = GetSenderFromSenderGroup();
                        MailSend.MailAttributes.SHeader myHeader = new MailAttributes.SHeader();
                        myHeader.addHeaderOnce = MailAttributes.AttributeInsertMethod.change;
                        myHeader.name = Tools.sMailFrom;
                        myHeader.value = m_sender[threadIndex];
                        if (myHeader.value.Contains("pilot"))
                        {
                            Log.logger.Fatal("ERROR Conatinas Pilot in AddHeaderUnparsed");
                        }
                        m_mailAttributes[threadIndex].Headers.Clear();
                        m_mailAttributes[threadIndex].Headers.Add(myHeader);
                         */

                        sHeader = StringHelper.StringHelper.OverWriteHeaderAttributes(sHeader, Constants.sMailFrom, m_sender[threadIndex]);
                    }
                    else
                    {
                        sHeader = StringHelper.StringHelper.OverWriteHeaderAttributes(sHeader, Constants.sMailFrom, m_smtpFrom);
                    }

                }

                //Content-Transfer-Encoding ermitteln


                //ToDo: Sichern der Membervariablen bevor Foreach weil ein andere Thread die ändert, dann Probleme
                foreach (MailSend.MailAttributes.SHeader header in m_mailAttributes[threadIndex].Headers)
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
                        if (header.value.Contains("pilot"))
                        {
                            Log.logger.Fatal("ERROR Contains pilot in Header");
                        }
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

                MailSend.MailAttributes.SSubject subject = m_mailAttributes[threadIndex].Subject;
                if (subject.position != MailAttributes.HeaderPosition.none)
                {
                    if (!String.IsNullOrEmpty(subject.subjectstring))
                    {
                        string utf8Begin = @"=?utf-8?B?";
                        string utf8End = @"?=";
                        Encoding systemEncoding = Encoding.Default;

                        byte[] systemEncodedName = systemEncoding.GetBytes(subject.subjectstring);
                        byte[] utf8EncodedName = Encoding.Convert(systemEncoding, Encoding.UTF8, systemEncodedName);
                        string newStringName = utf8Begin + Convert.ToBase64String(utf8EncodedName) + utf8End;

                        if (subject.position != null)
                        {
                            MailSend.MailAttributes.HeaderPosition myHeaderPosition = subject.position;
                            sHeader = StringHelper.StringHelper.ChangeSubject(sHeader, newStringName, myHeaderPosition);

                        }
                        else
                        {
                            MailSend.MailAttributes.HeaderPosition myHeaderPosition = MailAttributes.HeaderPosition.end;
                            sHeader = StringHelper.StringHelper.ChangeSubject(sHeader, newStringName, myHeaderPosition);
                        }
                    }
                }
                if (m_subjectMailCount)
                {
                    sHeader = StringHelper.StringHelper.ChangeSubject(sHeader, ("-" + GetCountWholeMailToSend().ToString()), MailAttributes.HeaderPosition.end);
                }
                if (m_subjectAddMailNamePosition != StringHelper.StringHelper.ESubjectAddMailNamePosition.None)
                {

                    string utf8Begin = @"=?utf-8?B?";
                    string utf8End = @"?=";
                    Encoding systemEncoding = Encoding.Default;
                    string filename = Path.GetFileName(msg.MessagePath);
                    byte[] systemEncodedFileName = systemEncoding.GetBytes(filename);
                    byte[] utf8EncodedFileName = Encoding.Convert(systemEncoding, Encoding.UTF8, systemEncodedFileName);
                    string newFileName = utf8Begin + Convert.ToBase64String(utf8EncodedFileName) + utf8End;

                    //string filename = utf8Begin + Path.GetFileName(msg.MessagePath) + utf8End;
                    if (m_subjectAddMailNamePosition == StringHelper.StringHelper.ESubjectAddMailNamePosition.Begin)
                    {
                        MailSend.MailAttributes.HeaderPosition myHeaderPosition = MailAttributes.HeaderPosition.begin;
                        sHeader = StringHelper.StringHelper.ChangeSubject(sHeader, newFileName + " ", myHeaderPosition);
                    }
                    else if (m_subjectAddMailNamePosition == StringHelper.StringHelper.ESubjectAddMailNamePosition.End)
                    {
                        MailSend.MailAttributes.HeaderPosition myHeaderPosition = MailAttributes.HeaderPosition.end;
                        sHeader = StringHelper.StringHelper.ChangeSubject(sHeader, " " + newFileName, myHeaderPosition);
                    }
                    else if (m_subjectAddMailNamePosition == StringHelper.StringHelper.ESubjectAddMailNamePosition.Replace)
                    {
                        sHeader = StringHelper.StringHelper.OverWriteHeaderAttributes(sHeader, "Subject:", newFileName);
                    }
                }
                if (isNewHeader)
                {
                    sHeader = sHeader + "\r\n";
                }
                return sHeader;
            }
            catch (Exception ex)
            {
                Log.logger.Error("Error in AddHeaderUnparsed " + ex.ToString());
                return sHeader;
            }
        }
        /*
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
            
            if (m_mailAttributes == null)
            {
                return;
            }
           

            try
            {


                foreach (MailSend.MailAttributes.SHeader header in m_mailAttributes[threadI].Headers)
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
         */


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


        private byte[] BuildStream(MessageWrapper msg, object threadContext)
        {
            int threadIndex = (int)threadContext;
            //string sender = string.Empty;
            //string recipient = string.Empty;
            //Stream msgStream = null;
            try
            {
                if (m_useRecipientGroup)
                {
                    bool newTurnRecipientGroup = false;
                    m_recipient[threadIndex] = GetRecipientFromRecipientGroup(ref newTurnRecipientGroup);
                    //ToDo: Only Testing
                    /*
                    if (m_recipient[threadIndex].Contains("pilot"))
                    {
                        Log.logger.Fatal("FATAL ERROR Recipient CONTAINS pilot ");
                    }
                     */
                }

                if (m_useSenderGroup)
                {
                    bool newTurnSenderGroup = false;
                    m_sender[threadIndex] = GetSenderFromSenderGroup(ref newTurnSenderGroup);
                    //ToDo: Only Testing
                    /*
                    if (m_sender[threadIndex].Contains("pilot"))
                    {
                        Log.logger.Fatal("FATAL ERROR Sender CONTAINS pilot ");
                    }
                     */
                }

                if (msg.IsTestMessage)
                {
                    msg.MsgCached = true;
                    msg.SendOnce = true;
                    msg.MailMsgByte = CreateTestMail();

                }
                /*
                 * //obsolete
                 * ToDo: löschen
                if (m_parsed && !m_sendOriginal)
                {
                    if (!msg.MsgCached)
                    {
                        //ParseMessage(msg);

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



                            header = AddHeaderUnparsed(header, msg, threadIndex, true);

                            byteHeaderSchnell = enc.GetBytes(header);
                        }
                        else
                        {
                            //ToDo: kein header gefunden, erzeuge Header wenn nötig subject hinzufügen
                        }
                        msg.MailMsgByte = StringHelper.StringHelper.AddHeaderToBodyByte(byteHeaderSchnell, msg.MailMsgByte, msg.BodyPosition);



                        //MemoryStream ms = new MemoryStream(msg.MailMsgByte);
                        if (!msg.SendOnce)
                        {
                            msg.CacheMSG(m_cacheMailsUntilInByte, msg.MailMsgByte.Length);
                        }
                        //msgStream = (Stream)ms;



                    }
                }
                */

                //**********************Experimental ParserTest************************************************************************************

                //ToDo: löschen
                TestParserInDebugModus(msg);


                //**********************End of Experimental ParserTest******************************************************************************

                if (m_sendOriginal && !m_newMessageID && !m_parsed)//Message wird mit original MessageID gesendet
                {
                    if (!msg.MsgCached)
                    {
                        byte[] file = File.ReadAllBytes(msg.MessagePath);
                        msg.MailMsgByte = file;
                    }
                    //MemoryStream ms = new MemoryStream(msg.MailMsgByte);
                    if (!msg.SendOnce)
                    {
                        msg.CacheMSG(m_cacheMailsUntilInByte, msg.MailMsgByte.Length);
                    }

                    //msgStream = (Stream)ms;

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
                    //MemoryStream ms = new MemoryStream(msg.MailMsgByte);

                    if (!msg.SendOnce)
                    {
                        msg.CacheMSG(m_cacheMailsUntilInByte, msg.MailMsgByte.Length);
                    }

                    //msgStream = (Stream)ms;
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
                    //TEST
                    //byte[] body = StringHelper.StringHelper.GetBody(bodyPosition, msg.MailMsgByte);
                    //System.Text.ASCIIEncoding encTest = new System.Text.ASCIIEncoding();
                    //string bodyString = encTest.GetString(body);
                    //End TEST
                    if (byteHeaderSchnell != null)
                    {
                        msg.BodyPosition = bodyPosition;
                        System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
                        string header = enc.GetString(byteHeaderSchnell);

                        header = AddHeaderUnparsed(header, msg, threadIndex, true);

                        byteHeaderSchnell = enc.GetBytes(header);
                    }
                    else
                    {
                        if (m_addNotExistingHeader)
                        {
                            msg.BodyPosition = bodyPosition;
                            System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
                            //byteHeaderSchnell = new byte[0];

                            //string header = enc.GetString(byteHeaderSchnell);
                            //string header = "\r\n\r\n";
                            string header = "\r\n";
                            header = AddHeaderUnparsed(header, msg, threadIndex, false);
                            byteHeaderSchnell = enc.GetBytes(header);
                        }
                    }
                    msg.MailMsgByte = StringHelper.StringHelper.AddHeaderToBodyByte(byteHeaderSchnell, msg.MailMsgByte, msg.BodyPosition);

                    //MemoryStream ms = new MemoryStream(msg.MailMsgByte);


                    if (!msg.SendOnce)
                    {
                        msg.CacheMSG(m_cacheMailsUntilInByte, msg.MailMsgByte.Length);
                    }

                    //msgStream = (Stream)ms;
                }
                /*
                if (!m_useSenderGroup)
                {
                    m_smtpClient[threadIndex].MailFrom(m_smtpFrom, -1);
                }
                else
                {
                    //m_smtpClient[threadIndex].MailFrom(GetSenderFromSenderGroup());
                    m_smtpClient[threadIndex].MailFrom(m_sender[threadIndex]);
                }
                if (!m_useRecipientGroup)
                {
                    foreach (String mailTo in m_smtpTo)
                    {
                        m_smtpClient[threadIndex].RcptTo(mailTo);
                    }
                }
                else
                {
                    //m_smtpClient[threadIndex].RcptTo(GetRecipientFromRecipientGroup());
                    m_smtpClient[threadIndex].RcptTo(m_recipient[threadIndex]);
                }
                 */


            }
            catch (SmtpException ex)
            {
                //m_smtpClient[threadIndex].Rset();
                //ToDoMaxSizeExceed Mail ist zu groß Fehlerbehandlung und Continue

                throw ex;
            }
            catch (Exception ex)
            {
                /*
                if (msgStream != null)
                {
                    msgStream.Close();
                }
                 */
                throw ex;
            }
            return msg.MailMsgByte;
        }//BuildStream


        private byte[] BuildStreamKMS(MessageWrapper msg, object threadContext)
        {
            int threadIndex = (int)threadContext;
            //string sender = string.Empty;
            //string recipient = string.Empty;
            //Stream msgStream = null;
            try
            {

                if (msg.IsTestMessage)
                {
                    msg.MsgCached = true;
                    msg.SendOnce = true;
                    msg.MailMsgByte = CreateTestMail();

                }


                if (!msg.MsgCached)
                {
                    byte[] file = File.ReadAllBytes(msg.MessagePath);
                    msg.MailMsgByte = file;
                }
                //MemoryStream ms = new MemoryStream(msg.MailMsgByte);
                if (!msg.SendOnce)
                {
                    msg.CacheMSG(m_cacheMailsUntilInByte, msg.MailMsgByte.Length);
                }

                //msgStream = (Stream)ms;

            }
            catch (SmtpException ex)
            {
                //m_smtpClient[threadIndex].Rset();
                //ToDoMaxSizeExceed Mail ist zu groß Fehlerbehandlung und Continue

                throw ex;
            }
            catch (Exception ex)
            {
                /*
                if (msgStream != null)
                {
                    msgStream.Close();
                }
                 */
                throw ex;
            }
            return msg.MailMsgByte;
        }//BuildStream

        //ToDo: Nur Test. Soll nur im Debug Modus laufen.
        [Conditional("DEBUG")]
        public void TestParserInDebugModus(MessageWrapper msg)
        {
            Console.WriteLine("DEBUG is defined");
            if (isExperimentalParserTest)
            {
                byte[] file = File.ReadAllBytes(msg.MessagePath);
                msg.MailMsgByte = file;
                int bodyPosition;
                byte[] byteHeaderSchnell = StringHelper.StringHelper.GetHeaderAndBodyPosition(file, out bodyPosition);

                

                //StringHelper.StringHelper.GetFirstHeaderAttributeAndValue

                if (byteHeaderSchnell != null)
                {
                    msg.BodyPosition = bodyPosition;
                    System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
                    string header = enc.GetString(byteHeaderSchnell);

                    MailNode<object> rootNode = new MailNode<object>(null, null, MailTypes.eMailTypes.RootNode);
                Constants.LineBreak lineBreak = new Constants.LineBreak();
                StringHelper.StringHelper.CreateHeaderTree(header, rootNode, ref lineBreak);
                    /*
                MailNode<byte[]> headerNode = new MailNode<byte[]>();
                headerNode.MailType = MailTypes.eMailTypes.MainHeader;
                headerNode.Parent = null;
                headerNode.Value = byteHeaderSchnell;
*/


                    header = StringHelper.StringHelper.RemoveMessageID(header);
                    if (m_subjectMailCount)
                    {
                        header = StringHelper.StringHelper.ChangeSubject(header, ("-" + GetCountWholeMailToSend().ToString()), MailAttributes.HeaderPosition.end);
                    }
                    byteHeaderSchnell = enc.GetBytes(header);
                }
                msg.MailMsgByte = StringHelper.StringHelper.AddHeaderToBodyByte(byteHeaderSchnell, file, msg.BodyPosition);

            }
        }

    }
}
