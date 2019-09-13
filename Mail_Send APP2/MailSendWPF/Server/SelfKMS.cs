using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using MailSend;
using System.IO;
using MailSendWPF.KMSPrivate;
using MailSendWPF.Interfaces;


namespace MailSendWPF.Server
{
    class SelfKMS
    {

        private ServerConfig m_serverConfig = null;
        private SslStream m_sslStream = null;

        KMSPrivate.KMSBackendPrivateWebServicePortTypeClient m_clientKMSPrivate = null;
        KMSPublic.KMSBackendPublicWebServicePortTypeClient m_clientKMSPublic = null;

        private int sendTimeout = 90000;//0;//50000;

        public int SendTimeout
        {
            get { return sendTimeout; }
            set { sendTimeout = value; }
        }
        private int recieveTimeout = 90000;//0;// 50000;

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
        public event IServer.RequestSendHandler RequestSendEvent;
        public void OnRequestSent(MessageWrapper mail, MailState mailState)
        {
            IServer.RequestSendHandler myEvent = RequestSendEvent;
            if (myEvent != null)
            {
                myEvent(mail, mailState);
            }
        }

        public event IServer.RequestSendFailedHandler RequestSendFailedEvent;
        public void OnRequestSentFailed(MessageWrapper mail, MailState mailState)
        {
            IServer.RequestSendFailedHandler myEvent = RequestSendFailedEvent;
            if (myEvent != null)
            {
                myEvent(mail, mailState);
            }
        }

        public event IServer.BeforeMailsentHandler BeforeMailsentEvent;
        public void OnBeforeMailSent(MessageWrapper mail, MailState mailState)
        {
            IServer.BeforeMailsentHandler myEvent = BeforeMailsentEvent;
            if (myEvent != null)
            {
                myEvent(mail, mailState);
            }
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

        public SelfKMS(ServerConfig serverConfig)
        {
            m_serverConfig = serverConfig;
        }
        public SelfKMS()
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


        public void Dispose()
        {

        }





        public void Disconnect()
        {
            isConnected = false;
        }



        public void KeyManagerConnect(MailSend.Constants.KeyManager keyManager)
        {
            try
            {
                EndpointAddress endPointAddressPrivateHost = new EndpointAddress(keyManager.privateHost);
                EndpointAddress endPointAddressPublicHost = new EndpointAddress(keyManager.publicHost);
                BasicHttpBinding bind = new BasicHttpBinding();
                bind.SendTimeout = new TimeSpan(0, 0, sendTimeout);
                bind.ReceiveTimeout = new TimeSpan(0, 0, recieveTimeout);
                bind.MaxBufferPoolSize = 99999999;
                bind.MaxReceivedMessageSize = 99999999;
                bind.MaxBufferSize = 99999999;

                m_clientKMSPrivate = new KMSPrivate.KMSBackendPrivateWebServicePortTypeClient(bind, endPointAddressPrivateHost);
                m_clientKMSPublic = new KMSPublic.KMSBackendPublicWebServicePortTypeClient(bind, endPointAddressPublicHost);
                m_clientKMSPrivate.Open();
                m_clientKMSPublic.Open();
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
            catch (Exception ex)
            {
                Log.logger.Error(ex.ToString());
                throw (ex);
            }
        }

        public void SendRequests(string tenantGuid, string mail, MailSend.Constants.KeyManager keyManager, ref MessageWrapper msg)
        {
            int nameIndex = mail.IndexOf('@');
            string name = mail.Substring(0, nameIndex);
            DateTime startTime = new DateTime();
            DateTime endTime = new DateTime();
            TimeSpan sendDuration = new TimeSpan();
            //KMSPrivate.getServerVersionRequest serverVersionRequest = new KMSPrivate.getServerVersionRequest();

            //KMSPrivate.KMSVersion respServerVersion = client.getServerVersion();




            //----------------------------------------------------------------------------------------------------------------------------------------------------------------------
            if (keyManager.GetOrCreatePrivateKeyAdvancedRequest)
            {
                String[] nameKey = { "FIRST_NAME", "LAST_NAME" };
                String[] value = { name, name };
                //KMS.getOrCreatePrivateKeyAdvancedRequest req1 = new KMS.getOrCreatePrivateKeyAdvancedRequest("", @"Jan@Haendle.de", name, value, "PKCS12_DER");
                try
                {
                    startTime = DateTime.Now;
                    KMSPrivate.KeyResult respGetCreatePrivKey = m_clientKMSPrivate.getOrCreatePrivateKeyAdvanced(tenantGuid, mail, nameKey, value, keyManager.PrivateKeyType);
                    endTime = DateTime.Now;
                    sendDuration = endTime.Subtract(startTime);
                    if (respGetCreatePrivKey.ret.status == 0 || respGetCreatePrivKey.ret.status == 1)
                    {
                        Log.logger.Info("getOrCreatePrivateKeyAdvanced SUCCESS " + " Duration: " + sendDuration.ToString(@"dd\.hh\:mm\:ss\.fffffff") + " Status: " + respGetCreatePrivKey.ret.status + " " + respGetCreatePrivKey.ret.description + " TenantGuid: " + tenantGuid);
                    }
                    else
                    {
                        Log.logger.Error("getOrCreatePrivateKeyAdvanced ERROR " + " Duration: " + sendDuration.ToString(@"dd\.hh\:mm\:ss\.fffffff") + " Status: " + respGetCreatePrivKey.ret.status + " " + respGetCreatePrivKey.ret.description + " TenantGuid: " + tenantGuid);
                    }

                }
                /*

            catch (EndpointNotFoundException ex)
            {

            }
                 */
                catch (Exception ex)
                {
                    Log.logger.Error("getOrCreatePrivateKeyAdvanced " + ex.Message + " TenantGuid: " + tenantGuid);
                    throw new SmtpException("getOrCreatePrivateKeyAdvanced " + ex.Message);
                }
            }
            //----------------------------------------------------------------------------------------------------------------------------------------------------------------------
            if (keyManager.GetOrCreatePrivateKeyRequest)
            {
                KMSPrivate.CertificateData certData = new KMSPrivate.CertificateData();
                certData.firstName = name;
                certData.lastName = name;
                try
                {
                    startTime = DateTime.Now;
                    KMSPrivate.KeyResult respGetCreatePrivKey = m_clientKMSPrivate.getOrCreatePrivateKey(tenantGuid, mail, certData, keyManager.PrivateKeyType);
                    endTime = DateTime.Now;
                    sendDuration = endTime.Subtract(startTime);
                    if (respGetCreatePrivKey.ret.status == 0 || respGetCreatePrivKey.ret.status == 1)
                    {
                        Log.logger.Info("getOrCreatePrivateKey SUCCESS " + " Duration: " + sendDuration.ToString(@"dd\.hh\:mm\:ss\.fffffff") + " Status: " + respGetCreatePrivKey.ret.status + " " + respGetCreatePrivKey.ret.description + " TenantGuid: " + tenantGuid);
                    }
                    else
                    {
                        Log.logger.Error("getOrCreatePrivateKey ERROR " + " Duration: " + sendDuration.ToString(@"dd\.hh\:mm\:ss\.fffffff") + " Status: " + respGetCreatePrivKey.ret.status + " " + respGetCreatePrivKey.ret.description + " TenantGuid: " + tenantGuid);
                    }

                }
                /*
            catch (EndpointNotFoundException ex)
            {

            }
                 */
                catch (Exception ex)
                {
                    Log.logger.Error("getOrCreatePrivateKey " + ex.Message + " TenantGuid: " + tenantGuid);
                    throw new SmtpException("getOrCreatePrivateKey " + ex.Message);
                }
            }

            if (keyManager.GetPrivateKeyRequest)
            {

                try
                {
                    startTime = DateTime.Now;
                    KMSPrivate.KeyResult respGetCreatePrivKey = m_clientKMSPrivate.getPrivateKey(tenantGuid, mail, keyManager.PrivateKeyType);
                    endTime = DateTime.Now;
                    sendDuration = endTime.Subtract(startTime);

                    if (respGetCreatePrivKey.ret.status == 0)
                    {
                        Log.logger.Info("getPrivateKey SUCCESS " + " Duration: " + sendDuration.ToString(@"dd\.hh\:mm\:ss\.fffffff") + " Status: " + respGetCreatePrivKey.ret.status + " " + respGetCreatePrivKey.ret.description + " TenantGuid: " + tenantGuid);
                    }
                    else
                    {
                        Log.logger.Error("getPrivateKey ERROR " + " Duration: " + sendDuration.ToString(@"dd\.hh\:mm\:ss\.fffffff") + " Status: " + respGetCreatePrivKey.ret.status + " " + respGetCreatePrivKey.ret.description + " TenantGuid: " + tenantGuid);
                    }

                }
                /*
            catch (EndpointNotFoundException ex)
            {

            }
                 */
                catch (Exception ex)
                {
                    Log.logger.Error("getPrivateKey " + ex.Message + " TenantGuid: " + tenantGuid);
                    throw new SmtpException("getPrivateKey " + ex.Message);
                }
            }

            if (keyManager.GetAllPrivateKeysForEmailRequest)
            {
                try
                {
                    startTime = DateTime.Now;
                    KMSPrivate.ArrayOfKeys respGetCreatePrivKey = m_clientKMSPrivate.getAllPrivateKeysForEmail(tenantGuid, mail, keyManager.PrivateKeyType);
                    endTime = DateTime.Now;
                    sendDuration = endTime.Subtract(startTime);

                    if (respGetCreatePrivKey.ret.status == 0)
                    {
                        Log.logger.Info("getAllPrivateKeysForEmail SUCCESS " + " Duration: " + sendDuration.ToString(@"dd\.hh\:mm\:ss\.fffffff") + " Status: " + respGetCreatePrivKey.ret.status + " " + respGetCreatePrivKey.ret.description + " Count " + respGetCreatePrivKey.keysCount + " TenantGuid: " + tenantGuid);
                    }
                    else
                    {
                        Log.logger.Error("getAllPrivateKeysForEmail ERROR " + " Duration: " + sendDuration.ToString(@"dd\.hh\:mm\:ss\.fffffff") + " Status: " + respGetCreatePrivKey.ret.status + " " + respGetCreatePrivKey.ret.description + " Count " + respGetCreatePrivKey.keysCount + " TenantGuid: " + tenantGuid);
                    }

                }
                /*
            catch (EndpointNotFoundException ex)
            {

            }
                 */
                catch (Exception ex)
                {
                    Log.logger.Error("getAllPrivateKeysForEmail " + ex.Message + " TenantGuid: " + tenantGuid);
                    throw new SmtpException("getAllPrivateKeysForEmail " + ex.Message);
                }
            }

            if (keyManager.Get509CertificateRequest)
            {
                //ToDo Nur für kurzen Test auskommentiert!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                
                try
                {
                    startTime = DateTime.Now;
                    KMSPublic.KeyResult respGetCreatePubKey = m_clientKMSPublic.getX509Certificate(tenantGuid, mail, keyManager.PublicKeyType);
                    endTime = DateTime.Now;
                    sendDuration = endTime.Subtract(startTime);

                    if (respGetCreatePubKey.ret.status == 0)
                    {
                        Log.logger.Info("getX509Certificate SUCCESS " + " Duration: " + sendDuration.ToString(@"dd\.hh\:mm\:ss\.fffffff") + " Status: " + respGetCreatePubKey.ret.status + " " + respGetCreatePubKey.ret.description + " TenantGuid: " + tenantGuid);
                    }
                    else
                    {
                        Log.logger.Error("getX509Certificate ERROR " + " Duration: " + sendDuration.ToString(@"dd\.hh\:mm\:ss\.fffffff") + " Status: " + respGetCreatePubKey.ret.status + " " + respGetCreatePubKey.ret.description + " TenantGuid: " + tenantGuid);
                    }

                }
               
                catch (Exception ex)
                {
                    Log.logger.Error("getX509Certificate " + ex.Message + " TenantGuid: " + tenantGuid);
                    throw new SmtpException("getX509Certificate " + ex.Message);
                }
            
                //DownloadCertificates(tenantGuid, keyManager, "");
            }

            if (keyManager.GetAllX509CertificatesForEmailRequest)
            {

                try
                {
                    startTime = DateTime.Now;
                    KMSPublic.ArrayOfKeys respGetCreatePubKey = m_clientKMSPublic.getAllX509CertificatesForEmail(tenantGuid, mail, keyManager.PublicKeyType);
                    endTime = DateTime.Now;
                    sendDuration = endTime.Subtract(startTime);

                    if (respGetCreatePubKey.ret.status == 0)
                    {
                        Log.logger.Info("getAllX509CertificatesForEmail SUCCESS " + " Duration: " + sendDuration.ToString(@"dd\.hh\:mm\:ss\.fffffff") + " Status: " + respGetCreatePubKey.ret.status + " " + respGetCreatePubKey.ret.description + " TenantGuid: " + tenantGuid);
                    }
                    else
                    {
                        Log.logger.Error("getAllX509CertificatesForEmail ERROR " + " Duration: " + sendDuration.ToString(@"dd\.hh\:mm\:ss\.fffffff") + " Status: " + respGetCreatePubKey.ret.status + " " + respGetCreatePubKey.ret.description + " TenantGuid: " + tenantGuid);
                    }

                }
                /*
            catch (EndpointNotFoundException ex)
            {

            }
                 */
                catch (Exception ex)
                {
                    Log.logger.Error("getAllX509CertificatesForEmail " + ex.Message + " TenantGuid: " + tenantGuid);
                    throw new SmtpException("getAllX509CertificatesForEmail " + ex.Message);
                }
            }

            if (keyManager.ImportX509CertificateRequest)
            {
                try
                {
                    startTime = DateTime.Now;
                    KMSPublic.KeyResult respGetCreatePubKey = m_clientKMSPublic.getX509Certificate(keyManager.ImportFromTenant, mail, keyManager.PublicKeyType);
                    endTime = DateTime.Now;
                    sendDuration = endTime.Subtract(startTime);

                    if (respGetCreatePubKey.ret.status == 0)
                    {
                        startTime = DateTime.Now;
                        KMSPublic.KeyResult respGetCreatePubKeyImport = m_clientKMSPublic.importX509Certificates(tenantGuid, respGetCreatePubKey.data, respGetCreatePubKey.dataType);
                        endTime = DateTime.Now;
                        sendDuration = endTime.Subtract(startTime);

                        if (respGetCreatePubKeyImport.ret.status == 0)
                        {
                            Log.logger.Info("importX509Certificates SUCCESS " + " Duration: " + sendDuration.ToString(@"dd\.hh\:mm\:ss\.fffffff") + " Status: " + respGetCreatePubKeyImport.ret.status + " " + respGetCreatePubKeyImport.ret.description + "Imported from: " + keyManager.ImportFromTenant + " Imported to: " + tenantGuid);
                        }
                        else
                        {
                            Log.logger.Error("importX509Certificates ERROR " + " Duration: " + sendDuration.ToString(@"dd\.hh\:mm\:ss\.fffffff") + " Status: " + respGetCreatePubKeyImport.ret.status + " " + respGetCreatePubKeyImport.ret.description + "Imported from: " + keyManager.ImportFromTenant + " Imported to: " + tenantGuid);
                        }
                    }
                    else
                    {
                        Log.logger.Error("getX509Certificate in Import ERROR " + " Duration: " + sendDuration.ToString(@"dd\.hh\:mm\:ss\.fffffff") + " Status: " + respGetCreatePubKey.ret.status + " " + respGetCreatePubKey.ret.description + "Imported from: " + keyManager.ImportFromTenant + " Imported to TenantGuid: " + tenantGuid);
                    }

                }
                /*
            catch (EndpointNotFoundException ex)
            {
                throw new SmtpException("ImportX509CertificateRequest", ex);
            }
                 */
                catch (Exception ex)
                {
                    Log.logger.Error("getX509Certificate in Import ERROR  " + ex.Message + " TenantGuid: " + tenantGuid);
                    throw new SmtpException("ImportX509CertificateRequest " + ex.Message);

                }
            }


            if (keyManager.GetPrivateKeyFromEnvelopeRequest)
            {

                try
                {
                    int bodyPosition;
                    byte[] byteHeaderSchnell = StringHelper.StringHelper.GetHeaderAndBodyPosition(msg.MailMsgByte, out bodyPosition);
                    byte[] body = StringHelper.StringHelper.GetBody(bodyPosition, msg.MailMsgByte);
                    System.Text.ASCIIEncoding encTest = new System.Text.ASCIIEncoding();
                    string myBody = encTest.GetString(body);
                    byte[] body2 = Convert.FromBase64String(myBody);
                    //byte[] body2 = encTest
                    startTime = DateTime.Now;
                    KMSPrivate.KeyResult respGetCreatePrivKey = m_clientKMSPrivate.getPrivateKeyFromEnvelope(tenantGuid, body2, keyManager.PrivateKeyType);
                    endTime = DateTime.Now;
                    sendDuration = endTime.Subtract(startTime);

                    if (respGetCreatePrivKey.ret.status == 0)
                    {
                        Log.logger.Info("getPrivateKeyFromEnvelope SUCCESS " + " Duration: " + sendDuration.ToString(@"dd\.hh\:mm\:ss\.fffffff") + " Status: " + respGetCreatePrivKey.ret.status + " " + respGetCreatePrivKey.ret.description + " TenantGuid: " + tenantGuid);
                    }
                    else
                    {
                        Log.logger.Error("getPrivateKeyFromEnvelope ERROR " + " Duration: " + sendDuration.ToString(@"dd\.hh\:mm\:ss\.fffffff") + " Status: " + respGetCreatePrivKey.ret.status + " " + respGetCreatePrivKey.ret.description + " TenantGuid: " + tenantGuid);
                    }

                }
                catch (Exception ex)
                {
                    Log.logger.Error("getPrivateKeyFromEnvelope " + ex.Message + " TenantGuid: " + tenantGuid);
                    throw new SmtpException("getPrivateKeyFromEnvelope " + ex.Message);
                }
            }//end if
        }

        public void DownloadCertificates(string tenantGuid, MailSend.Constants.KeyManager keyManager, string outputDirectory, bool saveCertificates = false)
        {
            try
            {
                DateTime startTime = DateTime.Now;
                KMSPrivate.ArrayOfCertificateInfos certInfos = m_clientKMSPrivate.getCertificatesList(tenantGuid);
                DateTime endTimeGetList = DateTime.Now;
                TimeSpan sendDurationGetList = endTimeGetList.Subtract(startTime);
                if (certInfos.ret.status == 0)
                {
                    Log.logger.Info("getCertificatesList SUCCESS " + " Duration: " + sendDurationGetList.ToString(@"dd\.hh\:mm\:ss\.fffffff") + " Status: " + certInfos.ret.status + " " + certInfos.ret.description + " TenantGuid: " + tenantGuid);
                }
                else
                {
                    Log.logger.Error("getCertificatesList ERROR " + " Duration: " + sendDurationGetList.ToString(@"dd\.hh\:mm\:ss\.fffffff") + " Status: " + certInfos.ret.status + " " + certInfos.ret.description + " TenantGuid: " + tenantGuid);
                    return;
                }


                string[] hashIDs = new string[certInfos.list.LongLength];
                for (long i = 0; i < certInfos.list.LongLength; i++)
                {
                    hashIDs[i] = certInfos.list[i].hashId;
                }

                KMSPrivate.ArrayOfKeys keys = m_clientKMSPrivate.downloadCertificates(tenantGuid, hashIDs);
               
                if (saveCertificates)
                {
                    foreach (KeyResult keyResult in keys.list)
                    {
                        //keyResult.
                    }
                    FileStream fs = new FileStream("test", FileMode.Create, FileAccess.Write);
                }
                
                DateTime endTimeWhole = DateTime.Now;
                TimeSpan sendDurationWhole = endTimeWhole.Subtract(startTime);
                if (keys.ret.status == 0)
                {
                    Log.logger.Info("downloadCertificates SUCCESS " + " Duration: " + sendDurationWhole.ToString(@"dd\.hh\:mm\:ss\.fffffff") + " Status: " + keys.ret.status + " " + keys.ret.description + " TenantGuid: " + tenantGuid);
                }
                else
                {
                    Log.logger.Error("downloadCertificates ERROR " + " Duration: " + sendDurationWhole.ToString(@"dd\.hh\:mm\:ss\.fffffff") + " Status: " + keys.ret.status + " " + keys.ret.description + " TenantGuid: " + tenantGuid);
                    return;
                }
                
            }
            catch (Exception ex)
            {
                Log.logger.Error("downloadCertificates " + ex.Message + " TenantGuid: " + tenantGuid);
                throw new SmtpException("downloadCertificates " + ex.Message);
            }

        }





    }
}
