using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using MailSend;
using System.Collections.ObjectModel;
using MailSendWPF.Configuration;

namespace MailSendWPF.Server
{
    class ServerConfig
    {
        private string mailTo = @"testuser1@jh1.local";

        public string MailTo
        {
            get { return mailTo; }
            set { mailTo = value; }
        }
        private string mailFrom = @"test@test.local";

        public string MailFrom
        {
            get { return mailFrom; }
            set { mailFrom = value; }
        }
        private IPAddress smartHost = null;

        public IPAddress SmartHost
        {
            get { return smartHost; }
            set { smartHost = value; }
        }
        private string host = String.Empty;

        public string Host
        {
            get { return host; }
            set { host = value; }
        }
        private int port = 25;

        public int Port
        {
            get { return port; }
            set { port = value; }
        }
        private string user = "";

        public string User
        {
            get { return user; }
            set { user = value; }
        }
        private string pwd = "";

        public string Pwd
        {
            get { return pwd; }
            set { pwd = value; }
        }
        //string dir = @"c:\Mails\zwei";
        //string filter = @"*.eml";
        private ulong rounds = 1;

        public ulong Rounds
        {
            get { return rounds; }
            set { rounds = value; }
        }
        private int connections = 1;

        public int Connections
        {
            get { return connections; }
            set { connections = value; }
        }
        private bool endlessSending = false;

        public bool EndlessSending
        {
            get { return endlessSending; }
            set { endlessSending = value; }
        }
        private bool recursive = false;

        public bool Recursive
        {
            get { return recursive; }
            set { recursive = value; }
        }
        private string subject = String.Empty;

        public string Subject
        {
            get { return subject; }
            set { subject = value; }
        }
        private string headerKey = String.Empty;

        public string HeaderKey
        {
            get { return headerKey; }
            set { headerKey = value; }
        }
        private string headerValue = String.Empty;

        public string HeaderValue
        {
            get { return headerValue; }
            set { headerValue = value; }
        }
        private MailAttributes mailAttributes = new MailAttributes();

        public MailAttributes MailAttributes
        {
            get { return mailAttributes; }
            set { mailAttributes = value; }
        }
        private bool parsed = true;

        public bool Parsed
        {
            get { return parsed; }
            set { parsed = value; }
        }
        private int waitBetweenMailsms = 0;

        public int WaitBetweenMailsms
        {
            get { return waitBetweenMailsms; }
            set { waitBetweenMailsms = value; }
        }

        private long cacheMailsUntilInByte = 0;

        public long CacheMailsUntilInByte
        {
            get { return cacheMailsUntilInByte; }
            set { cacheMailsUntilInByte = value; }
        }
        private bool sendOriginal = false;

        public bool SendOriginal
        {
            get { return sendOriginal; }
            set { sendOriginal = value; }
        }
        private bool newMessageID = false;

        public bool NewMessageID
        {
            get { return newMessageID; }
            set { newMessageID = value; }
        }
        private bool fallBack = true;

        public bool FallBack
        {
            get { return fallBack; }
            set { fallBack = value; }
        }

        private bool addSenderToMail = true;

        public bool AddSenderToMail
        {
            get { return addSenderToMail; }
            set { addSenderToMail = value; }
        }
        private bool addRecipientToMail = true;

        public bool AddRecipientToMail
        {
            get { return addRecipientToMail; }
            set { addRecipientToMail = value; }
        }
        private bool anonymousAuth = true;

        public bool AnonymousAuth
        {
            get { return anonymousAuth; }
            set { anonymousAuth = value; }
        }
        private bool basicAuth = false;

        public bool BasicAuth
        {
            get { return basicAuth; }
            set { basicAuth = value; }
        }

        private int codePage = 0;

        public int CodePage
        {
            get { return codePage; }
            set { codePage = value; }
        }

        private bool codePageEnabled = false;

        public bool CodePageEnabled
        {
            get { return codePageEnabled; }
            set { codePageEnabled = value; }
        }

        private ObservableCollection<String> filters = null;

        public ObservableCollection<String> Filters
        {
            get { return filters; }
            set { filters = value; }
        }

        private ObservableCollection<String> dirs = null;

        public ObservableCollection<String> Dirs
        {
            get { return dirs; }
            set { dirs = value; }
        }

        private string recipientGroup = String.Empty;

        public string RecipientGroup
        {
            get { return recipientGroup; }
            set { recipientGroup = value; }
        }

        private int recipientGroupStart = 0;

        public int RecipientGroupStart
        {
            get { return recipientGroupStart; }
            set { recipientGroupStart = value; }
        }

        private int recipientGroupEnd = 0;

        public int RecipientGroupEnd
        {
            get { return recipientGroupEnd; }
            set { recipientGroupEnd = value; }
        }

        private string recipientGroupDomain = String.Empty;

        public string RecipientGroupDomain
        {
            get { return recipientGroupDomain; }
            set { recipientGroupDomain = value; }
        }

//Start SenderGroup-------------------------------------------------------------------
        private string senderGroup = String.Empty;

        public string SenderGroup
        {
            get { return senderGroup; }
            set { senderGroup = value; }
        }

        private int senderGroupStart = 0;

        public int SenderGroupStart
        {
            get { return senderGroupStart; }
            set { senderGroupStart = value; }
        }

        private int senderGroupEnd = 0;

        public int SenderGroupEnd
        {
            get { return senderGroupEnd; }
            set { senderGroupEnd = value; }
        }

        private string senderGroupDomain = String.Empty;

        public string SenderGroupDomain
        {
            get { return senderGroupDomain; }
            set { senderGroupDomain = value; }
        }
//End SenderGroup----------------------------------------------------------------------------------------


        private bool subjectMailCount = false;

        public bool SubjectMailCount
        {
            get { return subjectMailCount; }
            set { subjectMailCount = value; }
        }

        private bool subjectMailName = false;

        public bool SubjectMailName
        {
            get { return subjectMailName; }
            set { subjectMailName = value; }
        }
        
        private bool logging = false;

        public bool Logging
        {
            get { return logging; }
            set { logging = value; }
        }

        private bool addHeader = false;

        public bool AddHeader
        {
            get { return addHeader; }
            set { addHeader = value; }
        }
        /*
        private bool subjectMailNameEnd = false;

        public bool SubjectMailNameEnd
        {
            get { return subjectMailNameEnd; }
            set { subjectMailNameEnd = value; }
        }
        private bool subjectMailNameReplace = false;

        public bool SubjectMailNameReplace
        {
            get { return subjectMailNameReplace; }
            set { subjectMailNameReplace = value; }
        }
         */

        private StringHelper.StringHelper.ESubjectAddMailNamePosition subjectAddMailNamePosition = new StringHelper.StringHelper.ESubjectAddMailNamePosition();

        public StringHelper.StringHelper.ESubjectAddMailNamePosition SubjectAddMailNamePosition
        {
            get { return subjectAddMailNamePosition; }
            set { subjectAddMailNamePosition = value; }
        }

        private bool ignoreAllErrors = false;

        public bool IgnoreAllErrors
        {
            get { return ignoreAllErrors; }
            set { ignoreAllErrors = value; }
        }
        private bool useNoneSSL = false;

        public bool UseNoneSSL
        {
            get { return useNoneSSL; }
            set { useNoneSSL = value; }
        }
        private bool useTLS = false;

        public bool UseTLS
        {
            get { return useTLS; }
            set { useTLS = value; }
        }
        private bool useSMTPS = false;

        public bool UseSMTPS
        {
            get { return useSMTPS; }
            set { useSMTPS = value; }
        }

        private int sendTimeout = 8192;

        public int SendTimeout
        {
            get { return sendTimeout; }
            set { sendTimeout = value; }
        }
        private int receiveTimeout = 8192;

        public int ReceiveTimeout
        {
            get { return receiveTimeout; }
            set { receiveTimeout = value; }
        }
        private int sendBuffer = 50000;

        public int SendBuffer
        {
            get { return sendBuffer; }
            set { sendBuffer = value; }
        }
        private int receiveBuffer = 50000;

        public int ReceiveBuffer
        {
            get { return receiveBuffer; }
            set { receiveBuffer = value; }
        }

        private bool useSenderGroup = false;

        public bool UseSenderGroup
        {
            get { return useSenderGroup; }
            set { useSenderGroup = value; }
        }

        private bool useRecipientGroup = false;

        public bool UseRecipientGroup
        {
            get { return useRecipientGroup; }
            set { useRecipientGroup = value; }
        }
        //KeyManager---------------------------------------------------------------------
        private bool keyManagerUse = false;

        public bool KeyManagerUse
        {
            get { return keyManagerUse; }
            set { keyManagerUse = value; }
        }

        private bool keyManagerTest = false;

        public bool KeyManagerTest
        {
            get { return keyManagerTest; }
            set { keyManagerTest = value; }
        }

        private bool getPrivateKeyRequest = false;

        public bool GetPrivateKeyRequest
        {
            get { return getPrivateKeyRequest; }
            set { getPrivateKeyRequest = value; }
        }

        private bool getAllPrivateKeysForEmailRequest = false;

        public bool GetAllPrivateKeysForEmailRequest
        {
            get { return getAllPrivateKeysForEmailRequest; }
            set { getAllPrivateKeysForEmailRequest = value; }
        }

        private bool getOrCreatePrivateKeyAdvancedRequest = false;

        public bool GetOrCreatePrivateKeyAdvancedRequest
        {
            get { return getOrCreatePrivateKeyAdvancedRequest; }
            set { getOrCreatePrivateKeyAdvancedRequest = value; }
        }

        private bool getOrCreatePrivateKeyRequest = false;

        public bool GetOrCreatePrivateKeyRequest
        {
            get { return getOrCreatePrivateKeyRequest; }
            set { getOrCreatePrivateKeyRequest = value; }
        }

        private bool get509CertificateRequest = false;

        public bool Get509CertificateRequest
        {
            get { return get509CertificateRequest; }
            set { get509CertificateRequest = value; }
        }

        private bool importX509CertificateRequest = false;

        public bool ImportX509CertificateRequest
        {
            get { return importX509CertificateRequest; }
            set { importX509CertificateRequest = value; }
        }

        private bool getAllX509CertificatesForEmailRequest = false;

        public bool GetAllX509CertificatesForEmailRequest
        {
            get { return getAllX509CertificatesForEmailRequest; }
            set { getAllX509CertificatesForEmailRequest = value; }
        }

        private bool getX509CertificateForVerificationBySignedDataRequest = false;

        public bool GetX509CertificateForVerificationBySignedDataRequest
        {
            get { return getX509CertificateForVerificationBySignedDataRequest; }
            set { getX509CertificateForVerificationBySignedDataRequest = value; }
        }



        private bool getPrivateKeyFromEnvelopeRequest = false;

        public bool GetPrivateKeyFromEnvelopeRequest
        {
            get { return getPrivateKeyFromEnvelopeRequest; }
            set { getPrivateKeyFromEnvelopeRequest = value; }
        }



        private bool makeCyclicCertsForAllTenantsSame = false;

        public bool MakeCyclicCertsForAllTenantsSame
        {
            get { return makeCyclicCertsForAllTenantsSame; }
            set { makeCyclicCertsForAllTenantsSame = value; }
        }

        

        private ObservableCollection<ServerSchemaTenants> tenants = null;

        public ObservableCollection<ServerSchemaTenants> Tenants
        {
            get { return tenants; }
            set { tenants = value; }
        }

        private string privateKeyType = "";

        public string PrivateKeyType
        {
            get { return privateKeyType; }
            set { privateKeyType = value; }
        }

        private string publicKeyType = "";

        public string PublicKeyType
        {
            get { return publicKeyType; }
            set { publicKeyType = value; }
        }

        private string importFromTenant = "";

        public string ImportFromTenant
        {
            get { return importFromTenant; }
            set { importFromTenant = value; }
        }

        //KeyManager Ende----------------------------------------------------------------------------------------------------------------------------------------------------


    }
}
