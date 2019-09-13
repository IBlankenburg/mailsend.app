using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using MailSend;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.IO;
using System.Collections;

namespace MailSendWPF.Server
{
    class SelfSmtp
    {
        private ServerConfig m_serverConfig = null;
        private TcpClient tcpc = null;
        private NetworkStream tcpcStream = null;
        private SslStream m_sslStream = null;

        KMSPrivate.KMSBackendPrivateWebServicePortTypeClient m_clientKMSPrivate = null;
        KMSPublic.KMSBackendPublicWebServicePortTypeClient m_clientKMSPublic = null;

        private int sendTimeout = 50000;//0;//50000;

        public int SendTimeout
        {
            get { return sendTimeout; }
            set { sendTimeout = value; }
        }
        private int recieveTimeout = 50000;//0;// 50000;

        public int RecieveTimeout
        {
            get { return recieveTimeout; }
            set { recieveTimeout = value; }
        }
        private int receiveBufferSize = 8192;//1024;

        public int ReceiveBufferSize
        {
            get { return receiveBufferSize; }
            set { receiveBufferSize = value; }
        }
        private int sendBufferSize = 8192;//65536;//16384;//8192;//1024;

        public int SendBufferSize
        {
            get { return sendBufferSize; }
            set { sendBufferSize = value; }
        }
        private String m_hostname = String.Empty;
        private bool m_securedConnection = false;

        private bool isConnected = false;

        public bool IsConnected
        {
            get { return isConnected; }
            set { isConnected = value; }
        }
        private bool isAuthenticated = false;

        public bool IsAuthenticated
        {
            get { return isAuthenticated; }
            //set { isAuthenticated = value; }
        }

        private bool isEhloHeloSent = false;

        public bool IsEhloHeloSent
        {
            get { return isEhloHeloSent; }
            set { isEhloHeloSent = value; }
        }
        public struct SupportedServerExtensions
        {
            public bool eightBitMime;
            public bool binaryMime;
            public bool chunking;
            public bool ehlo;
            public SupportedServerExtensions(bool eightBitMimeS, bool binaryMimeS, bool chunkingS, bool ehloS)
            {
                eightBitMime = eightBitMimeS;
                binaryMime = binaryMimeS;
                chunking = chunkingS;
                ehlo = ehloS;
            }

        }
        public SupportedServerExtensions supportedServerExtensions = new SupportedServerExtensions(false, false, false, true);


        public int Timeout
        {
            set
            {
                sendTimeout = value;
                recieveTimeout = value;
            }
        }
        public event EventHandler Connected;
        public event EventHandler Disconnected;
        public event EventHandler Authenticated;
        public event EventHandler StartedMessageTransfer;
        public event EventHandler EndedMessageTransfer;

        public SelfSmtp(ServerConfig serverConfig)
        {
            m_serverConfig = serverConfig;
        }
        public SelfSmtp()
        {

        }



        internal void OnConnect(EventArgs e)
        {
            if (Connected != null)
                Connected(this, e);
        }

        internal void OnDisconnect(EventArgs e)
        {
            if (Disconnected != null)
                Disconnected(this, e);
        }

        internal void OnAuthenticated(EventArgs e)
        {
            if (Authenticated != null)
                Authenticated(this, e);
        }

        internal void OnStartedMessageTransfer(EventArgs e)
        {
            if (StartedMessageTransfer != null)
                StartedMessageTransfer(this, e);
        }

        internal void OnEndedMessageTransfer(EventArgs e)
        {
            if (EndedMessageTransfer != null)
                EndedMessageTransfer(this, e);
        }

        public NetworkStream Connect(IPEndPoint endPointSource, IPEndPoint endPointDest)
        {
            try
            {
                if (endPointDest == null)
                {
                    throw new SmtpException("No SmartHost set");
                }

                if (endPointDest == null || endPointDest.Port == 0)
                {
                    throw new SmtpException("Cannot use SendMail() method without specifying target host and port");
                }
                //m_hostname = endPointSource.Address.ToString();
                tcpc = new TcpClient(endPointSource);

                tcpc.Connect(endPointDest);
                Log.logger.Info("Cannot connect from: " + endPointSource.ToString() + " to: " + endPointDest.ToString());
                tcpc.ReceiveTimeout = recieveTimeout;
                tcpc.SendTimeout = sendTimeout;
                tcpc.ReceiveBufferSize = receiveBufferSize;
                tcpc.SendBufferSize = sendBufferSize;

                LingerOption lingerOption = new LingerOption(true, 10);
                tcpc.LingerState = lingerOption;
                isConnected = true;
                OnConnect(EventArgs.Empty);
                tcpcStream = tcpc.GetStream();
                return tcpcStream;
            }
            catch (SocketException e)
            {
                throw new SmtpException("Cannot connect from: " + endPointSource.ToString() + " to: " + endPointDest.ToString(), e);
            }


        }

        public NetworkStream Connect(string host, int port)
        {
            try
            {

                if (String.IsNullOrEmpty(host) || port == 0)
                {
                    throw new SmtpException("Cannot use SendMail() method without specifying target host and port");
                }
                //m_hostname = Dns.GetHostName();
                tcpc = new TcpClient(host, port);
                Log.logger.Info("connecting to: " + host + ":" + port);
                tcpc.ReceiveTimeout = recieveTimeout;
                tcpc.SendTimeout = sendTimeout;
                tcpc.ReceiveBufferSize = receiveBufferSize;
                tcpc.SendBufferSize = sendBufferSize;

                LingerOption lingerOption = new LingerOption(true, 10);
                tcpc.LingerState = lingerOption;

                OnConnect(EventArgs.Empty);
                tcpcStream = tcpc.GetStream();
                return tcpcStream;
            }
            catch (SocketException e)
            {
                throw new SmtpException("Cannot connect to specified smtp host(" + host + ":" + port + ").", e);
            }
        }

        public void Disconnect()
        {
            // add delimeter to log file
            Log.logger.Info("------------------------------------------------------\r\n");
            //WriteLnToStream("QUIT\r\n");
            //CheckForErrorWithoutException(ReadFromStream(), ReplyConstants.QUIT);
            isEhloHeloSent = false;
            isConnected = false;
            isAuthenticated = false;
            if (m_sslStream != null)
            {
                m_sslStream.Close();
            }
            m_securedConnection = false;
            // fire disconnect event
            OnDisconnect(EventArgs.Empty);


            // destroy tcp connection if it hasn't already closed
            if (tcpc != null)
            {
                tcpc.Close();
                //tcpc = null;
            }
        }

        public void Dispose()
        {

        }

        public bool AuthLogin(string username, string password)
        {
            //NetworkStream tcpcStream = tcpc.GetStream();
            if (username != null && username.Length > 0 && password != null && password.Length >= 0)
            {
                WriteLnToStream("AUTH LOGIN\r\n");
                if (AuthImplemented(ReadFromStream()))
                {
                    WriteLnToStream(Convert.ToBase64String(Encoding.ASCII.GetBytes(username.ToCharArray())) + "\r\n");

                    CheckForError(ReadFromStream(), ReplyConstants.SERVER_CHALLENGE);

                    WriteLnToStream(Convert.ToBase64String(Encoding.ASCII.GetBytes(password.ToCharArray())) + "\r\n");
                    CheckForError(ReadFromStream(), ReplyConstants.AUTH_SUCCESSFUL);
                    isAuthenticated = true;
                    OnAuthenticated(EventArgs.Empty);
                    return true;
                }
            }

            return false;
        }
        /*
        private void SendRecipientList(ref NetworkStream nwstream, ArrayList recipients)
        {
            //	Iterate through all addresses and send them:
            for (IEnumerator i = recipients.GetEnumerator(); i.MoveNext(); )
            {
                EmailAddress recipient = (EmailAddress)i.Current;
                WriteToStream(ref nwstream, "RCPT TO: <" + recipient.address + ">\r\n");

                // potential 501 error (not valid sender, bad email address) below:
                CheckForError(ReadFromStream(ref nwstream), ReplyConstants.OK);
            }
        }
        */

        public void RcptTo(string recipient)
        {
            //NetworkStream tcpcStream = tcpc.GetStream();

            WriteLnToStream("RCPT TO: <" + recipient + ">\r\n");

            // potential 501 error (not valid sender, bad email address) below:
            CheckForError(ReadFromStream(), ReplyConstants.OK);

        }

        public void MailFrom(string sender)
        {
            //NetworkStream tcpcStream = tcpc.GetStream();
            sender = "MAIL FROM: <" + sender + ">";
            if (supportedServerExtensions.binaryMime && supportedServerExtensions.chunking)
            {
                //Mail kann als BiINARYMIME verschickt werden. Muss BODY=BINARYMIME anhängen
                sender = sender + " " + "BODY=BINARYMIME";

            }
            else if (supportedServerExtensions.eightBitMime)
            {
                sender = sender + " " + "BODY=8BITMIME";
            }

            WriteLnToStream(sender + "\r\n");

            // potential 501 error (not valid sender, bad email address) below:
            CheckForError(ReadFromStream(), ReplyConstants.OK);

        }
        public void MailFrom(string sender, int length)
        {
            sender = "MAIL FROM: <" + sender + ">";
            if (supportedServerExtensions.binaryMime && supportedServerExtensions.chunking)
            {
                //Mail kann als BiINARYMIME verschickt werden. Muss BODY=BINARYMIME anhängen
                sender = sender + " " + "BODY=BINARYMIME";

            }
            else if (supportedServerExtensions.eightBitMime)
            {
                sender = sender + " " + "BODY=8BITMIME";
            }
            //NetworkStream tcpcStream = tcpc.GetStream();
           
            WriteLnToStream(sender + "\r\n");

            // potential 501 error (not valid sender, bad email address) below:
            CheckForError(ReadFromStream(), ReplyConstants.OK);

        }

        public bool StartTLS(string senderIPAddress, IPAddress destServerIPAddress, string destServerHostName, string userName, string pwd)
        {
            WriteLnToStream("STARTTLS" + "\r\n");
            CheckForError(ReadFromStream(), ReplyConstants.HELO_REPLY);


            try
            {
                m_sslStream = new SslStream(tcpcStream, true, new RemoteCertificateValidationCallback(ValidateServerCertificate), null);
                if (destServerIPAddress != null)
                {
                    m_sslStream.AuthenticateAsClient(destServerIPAddress.ToString());
                }
                else if (!String.IsNullOrEmpty(destServerHostName))
                {
                    m_sslStream.AuthenticateAsClient(destServerHostName);
                }
                if (m_sslStream.IsEncrypted)
                {
                    m_securedConnection = true;
                }
                SendHELOEHLOTLS(senderIPAddress);
                return m_securedConnection;
            }
            catch (SmtpException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public static bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            //if (sslPolicyErrors == SslPolicyErrors.None)
            return true;

            //Console.WriteLine("Certificate error: {0}", sslPolicyErrors);

            // Do not allow this client to communicate with unauthenticated servers.
            //return false;
        }

        public bool StartSSLSecurePort(string senderIPAddress, IPAddress destServerIPAddress, string destServerHostName, string userName, string pwd)
        {
            try
            {
                m_sslStream = new SslStream(tcpcStream, true, new RemoteCertificateValidationCallback(ValidateServerCertificate), null);
                if (destServerIPAddress != null)
                {
                    m_sslStream.AuthenticateAsClient(destServerIPAddress.ToString());
                }
                else if (!String.IsNullOrEmpty(destServerHostName))
                {
                    m_sslStream.AuthenticateAsClient(destServerHostName);
                }
                if (m_sslStream.IsEncrypted)
                {
                    m_securedConnection = true;
                }
                SendHELOEHLO(senderIPAddress);
                return m_securedConnection;
            }
            catch (SmtpException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        public void Rset()
        {
            //NetworkStream tcpcStream = tcpc.GetStream();
            WriteLnToStream("RSET" + "\r\n");

            // potential 501 error (not valid sender, bad email address) below:
            CheckForError(ReadFromStream(), ReplyConstants.OK);

        }

        private bool AuthImplemented(string s)
        {
            if (s.IndexOf(ReplyConstants.SERVER_CHALLENGE) != -1)
            { return true; }

            return false;
        }

        private void CheckForError(string s, string successCode)
        {
            //ToDo:Bei Fehler Meldungen aufspalten und Exceptions werfen
            if (s.IndexOf(successCode) == -1)
                throw new SmtpException("ERROR - Expecting: " + successCode + ". Recieved: " + s);
        }

        private string CheckServerExtensionsAfterEhloOrHeloResponse(string s)
        {
            if (s.ToLower().Contains(Constants.sSMTPExtension8BitMIME.ToLower()))
            {
                supportedServerExtensions.eightBitMime = true;
            }

            if (s.ToLower().Contains(Constants.sSMTPExtensionBinaryMIME.ToLower()))
            {
                supportedServerExtensions.binaryMime = true;
                
            }

            if (s.ToLower().Contains(Constants.sSMTPExtensionCHUNKING.ToLower()))
            {
                supportedServerExtensions.chunking = true;
            }
            return s;
        }

        private bool CheckIfCommandAccepted(string s)
        {
            if (s.IndexOf(ReplyConstants.OK) == -1)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private void WriteLnToStream(string line)
        {
            try
            {
                byte[] arrToSend = Encoding.ASCII.GetBytes(line);
                if (!m_securedConnection)
                {
                    tcpcStream.Write(arrToSend, 0, arrToSend.Length);
                }
                else
                {
                    m_sslStream.Write(arrToSend, 0, arrToSend.Length);
                }
                //Console.WriteLine("[client]:" + line);
                Log.logger.Debug("[client]: " + line);
            }
            catch (ArgumentNullException ex)
            {
                throw new SmtpException(ex.Message, ex);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                throw new SmtpException(ex.Message, ex);
            }
            catch (System.IO.IOException ex)
            {
                throw ex;
                //throw new SmtpException("Write to Stream threw an IOException", ex);
            }
            catch (ObjectDisposedException ex)
            {
                throw new SmtpException(ex.Message, ex);
            }
            catch (System.Exception ex)
            {
                throw new SmtpException(ex.Message, ex);
            }
        }

        private void WriteToStream(byte[] buffer, int offset, int size)
        {
            try
            {
                //string log = Encoding.ASCII.GetString(buffer, offset, size);
                if (!m_securedConnection)
                {
                    tcpcStream.Write(buffer, offset, size);
                }
                else
                {
                    m_sslStream.Write(buffer, offset, size);
                }
                //Console.WriteLine("[client]:" + line);
                //Log.logger.Debug("[client]: " + line);
            }
            catch (ArgumentNullException ex)
            {
                throw new SmtpException(ex.Message, ex);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                throw new SmtpException(ex.Message, ex);
            }
            catch (System.IO.IOException ex)
            {
                throw ex;
                //throw new SmtpException("Write to Stream threw an IOException", ex);
            }
            catch (ObjectDisposedException ex)
            {
                throw new SmtpException(ex.Message, ex);
            }
            catch (System.Exception ex)
            {
                throw new SmtpException(ex.Message, ex);
            }
        }

        private void WriteToStream(byte[] arrToSend)
        {
            try
            {
                if (!m_securedConnection)
                {
                    tcpcStream.Write(arrToSend, 0, arrToSend.Length);
                }
                else
                {
                    m_sslStream.Write(arrToSend, 0, arrToSend.Length);
                }
                //Console.WriteLine("[client]:" + line);
                //LogMessage(line, "[client]: ");
            }
            catch (ArgumentNullException ex)
            {
                throw new SmtpException(ex.Message, ex);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                throw new SmtpException(ex.Message, ex);
            }
            catch (System.IO.IOException ex)
            {
                throw ex;
                //throw new SmtpException("Write to Stream threw an IOException", ex);
            }
            catch (ObjectDisposedException ex)
            {
                throw new SmtpException(ex.Message, ex);
            }
            catch (System.Exception ex)
            {
                throw new SmtpException(ex.Message, ex);
            }
        }

        private string ReadFromStream()
        {
            try
            {

                byte[] readBuffer = new byte[4096];
                int length;
                if (!m_securedConnection)
                {
                    length = tcpcStream.Read(readBuffer, 0, readBuffer.Length);
                }
                else
                {
                    length = m_sslStream.Read(readBuffer, 0, readBuffer.Length);
                }
                string returnMsg = Encoding.ASCII.GetString(readBuffer, 0, length);

                Log.logger.Debug("[server]: " + returnMsg);
                return returnMsg;
            }
            catch (ArgumentNullException ex)
            {
                throw new SmtpException(ex.Message, ex);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                throw new SmtpException(ex.Message, ex);
            }
            catch (System.IO.IOException ex)
            {
                throw ex;
                //throw new SmtpException("Write to Stream threw an IOException", ex);
            }
            catch (ObjectDisposedException ex)
            {
                throw new SmtpException(ex.Message, ex);
            }
            catch (System.Exception ex)
            {
                throw new SmtpException(ex.Message, ex);
            }
        }

        public void SendHELOEHLO()
        {
            //NetworkStream nwstream = tcpc.GetStream();
            CheckForError(ReadFromStream(), ReplyConstants.HELO_REPLY);

            WriteLnToStream("EHLO " + m_hostname + "\r\n");

            bool result = CheckIfCommandAccepted(CheckServerExtensionsAfterEhloOrHeloResponse(ReadFromStream()));
            if (!result)
            {
                Log.logger.Debug("Experimental - EHLO Command is not accepted try to fallback to HELO");
                supportedServerExtensions.ehlo = false;
                WriteLnToStream("HELO " + m_hostname + "\r\n");
                CheckForError(ReadFromStream(), ReplyConstants.OK);
            }
            isEhloHeloSent = true;

        }

        public void SendHELOEHLO(string hostname)
        {
            //NetworkStream nwstream = tcpc.GetStream();
            CheckForError(ReadFromStream(), ReplyConstants.HELO_REPLY);

            WriteLnToStream("EHLO " + hostname + "\r\n");

            bool result = CheckIfCommandAccepted(CheckServerExtensionsAfterEhloOrHeloResponse(ReadFromStream()));
            if (!result)
            {
                Log.logger.Debug("Experimental - EHLO Command is not accepted try to fallback to HELO");
                supportedServerExtensions.ehlo = false;
                WriteLnToStream("HELO " + hostname + "\r\n");
                CheckForError(ReadFromStream(), ReplyConstants.OK);
            }
            isEhloHeloSent = true;

        }
        public void SendHELOEHLOTLS(string hostname)
        {
            //NetworkStream nwstream = tcpc.GetStream();
            //CheckForError(ReadFromStream(), ReplyConstants.HELO_REPLY);

            WriteLnToStream("EHLO " + hostname + "\r\n");

            bool result = CheckIfCommandAccepted(CheckServerExtensionsAfterEhloOrHeloResponse(ReadFromStream()));
            if (!result)
            {
                Log.logger.Debug("Experimental - EHLO Command is not accepted try to fallback to HELO");
                supportedServerExtensions.ehlo = false;
                WriteLnToStream("HELO " + hostname + "\r\n");
                CheckForError(ReadFromStream(), ReplyConstants.OK);
            }
            isEhloHeloSent = true;

        }

        public void KeyManagerConnect(MailSend.Constants.KeyManager keyManager)
        {
            EndpointAddress endPointAddressPrivateHost = new EndpointAddress(keyManager.privateHost);
            EndpointAddress endPointAddressPublicHost = new EndpointAddress(keyManager.publicHost);
            BasicHttpBinding bind = new BasicHttpBinding();
            bind.SendTimeout = new TimeSpan(0, 0, sendTimeout / 1000);
            bind.ReceiveTimeout = new TimeSpan(0, 0, recieveTimeout / 1000);
            m_clientKMSPrivate = new KMSPrivate.KMSBackendPrivateWebServicePortTypeClient(bind, endPointAddressPrivateHost);
            m_clientKMSPublic = new KMSPublic.KMSBackendPublicWebServicePortTypeClient(bind, endPointAddressPublicHost);

            Log.logger.Debug("ConnectionState Private Webservices: " + m_clientKMSPrivate.State);
            Log.logger.Debug("ConnectionState Public Webservices: " + m_clientKMSPublic.State);

            if (m_clientKMSPrivate.State == CommunicationState.Opened && m_clientKMSPublic.State == CommunicationState.Opened)
            {
                isConnected = true;
            }
            else
            {
                isConnected = false;
            }
        }

        public void SendRequests(MailSend.Constants.KeyManager keyManager, ref MessageWrapper msg)
        {

        }

        public void SendMessageAsBinary(byte[] msg, int length, bool chunks = false)
        {
            //WriteLnToStream("BDAT " + length.ToString() + " LAST");
            //WriteLnToStream("BDAT " + length.ToString() + " LAST" );
            //WriteLnToStream("BDAT " + 0 + " LAST");
            //CheckForError(ReadFromStream(), ReplyConstants.OK);
            if (chunks)
            {
                int pos = 0;
                int i = 0;

                for (i = 0; i < length; )
                {
                    if ((i + sendBufferSize) >= length)
                    {
                        break;
                    }
                    i = i + sendBufferSize;
                    if ((i - pos) % sendBufferSize == 0)
                    {
                        if (i != 0)
                        {
                            //Log.logger.Debug("BINARYMIME i:= " + i + " Position:= " + pos);
                            WriteLnToStream("BDAT " + (i - pos).ToString() + "\r\n");
                            WriteToStream(msg, pos, i - pos);
                            //Log.logger.Debug("After BDAT in Loop: " + ReadFromStream());
                            CheckForError(ReadFromStream(), ReplyConstants.OK);
                            pos = i;
                        }

                    }
                }
                WriteLnToStream("BDAT " + (length - pos).ToString() + "\r\n");
                WriteToStream(msg, pos, length - pos);
                CheckForError(ReadFromStream(), ReplyConstants.OK);
                WriteLnToStream("BDAT " + 0 + " LAST" + "\r\n");
            }
            else
            {

                
                WriteLnToStream("BDAT " + length + " LAST" + "\r\n");
                WriteToStream(msg, 0, length);
                
            }
            CheckForError(ReadFromStream(), ReplyConstants.OK);
        }
        public void SendMessageNormal(byte[] msg, int length)
        {
            WriteLnToStream("DATA\r\n");

            CheckForError(ReadFromStream(), ReplyConstants.START_INPUT);
            int pos = 0;
            byte n = 0;
            byte n_1 = 0;
            n = msg[length - 1];
            n_1 = msg[length - 2];


            for (int i = 0; i < length; i++)
            {
                if ((i + 1) >= length)
                {
                    //. dezimal 46
                    //Kein Punkt (mehr) vorhanden, verschicke Restnachricht
                    WriteToStream(msg, pos, length - pos);
                    //End of data reached
                    break;
                }
                if (msg[i] == 46 && msg[i + 1] == 13)
                {
                    if ((i - 1) < 0)
                    {
                        //Senden incl. '.' Dann '.' hinzufügen
                        //von Pos bis i senden
                        WriteToStream(msg, pos, i - pos);
                        WriteLnToStream(".");
                        pos = i;
                    }
                    else
                    {
                        if (msg[i - 1] == 10 || msg[i - 1] == 13)
                        {
                            //Senden incl. '.' Dann '.' hinzufügen
                            //von Pos bis i senden
                            WriteToStream(msg, pos, i - pos);
                            WriteLnToStream(".");
                            pos = i;
                        }
                    }
                }
                //if ((i-pos) % 8192 == 0)

                if ((i - pos) % sendBufferSize == 0)
                {
                    WriteToStream(msg, pos, i - pos);
                    pos = i;
                }

            }//end for


            if (n_1 == 13 && n == 10)
            {
                WriteLnToStream(".\r\n");
            }
            else
            {
                WriteLnToStream("\r\n.\r\n");
            }
            /*
        else if (n == 10 || n == 13)
        {
            WriteLnToStream("\r\n.\r\n");
        }
        else
        {
            WriteLnToStream("\r\n.\r\n");
        }
        */
            CheckForError(ReadFromStream(), ReplyConstants.OK);
        }

        public void SendMessage(byte[] msg)
        {
            int length = msg.Length;
            
            try
            {
                OnStartedMessageTransfer(EventArgs.Empty);
                if (supportedServerExtensions.binaryMime && supportedServerExtensions.chunking)
                {
                    
                    SendMessageAsBinary(msg, length);
                }
                else
                {
                   SendMessageNormal(msg, length);
                }//end if 
                OnEndedMessageTransfer(EventArgs.Empty);

            }
            catch (SmtpException ex)
            {
                Log.logger.Error(ex.ToString());
                if (ex.InnerException != null)
                {
                    Log.logger.Error(ex.InnerException.ToString());
                    /*
                    if (ex.InnerException is IOException)
                    {
                        if (ex.InnerException.InnerException != null)
                        {
                            Log.logger.Error(ex.InnerException.InnerException.ToString());
                            if (ex.InnerException.InnerException is SocketException)
                            {
                                SocketException socketException = (SocketException)ex.InnerException.InnerException;
                                Log.logger.Error("SocketException Error code: " + socketException.ErrorCode);
                            }
                        }
                    }
                     */
                }

                throw ex;
            }
            catch (IOException ex)
            {
                if (ex.InnerException != null)
                {
                    Log.logger.Error(ex.InnerException.ToString());
                    if (ex.InnerException is SocketException)
                    {
                        SocketException socketException = (SocketException)ex.InnerException;
                        Log.logger.Error("SocketException Error code: " + socketException.ErrorCode);
                    }
                }
                throw ex;
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    Log.logger.Error(ex.InnerException.ToString());
                }
                Log.logger.Debug(ex.ToString());
                //ToDo added at 23.08. 20:13
                throw ex;
            }
            //WriteLnToStream("QUIT\r\n");
            //CheckForError(ReadFromStream(), ReplyConstants.QUIT);


        }


    }
}
/*
                if (this.username != null && this.username.Length > 0 && this.password != null && this.password.Length > 0)
                {
                    WriteToStream(ref tcpcStream, "EHLO " + Dns.GetHostName() + "\r\n");
                }
                else
                    WriteToStream(ref tcpcStream, "HELO " + Dns.GetHostName() + "\r\n");

                CheckForError(ReadFromStream(ref tcpcStream), ReplyConstants.OK);
                */
// Authentication is used if the u/p are supplied
//AuthLogin(ref tcpcStream);

//WriteToStream("MAIL FROM: <" + msg.from.address + ">\r\n");
//CheckForError(ReadFromStream(), ReplyConstants.OK);

/*
SendRecipientList(ref tcpcStream, msg.recipientList);
SendRecipientList(ref tcpcStream, msg.ccList);
SendRecipientList(ref tcpcStream, msg.bccList);

 */

