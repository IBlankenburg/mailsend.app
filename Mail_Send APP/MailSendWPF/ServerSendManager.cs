using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StartUp;
using MailSend;
using System.Net;
using System.Windows;
using MailSendWPF.Server;
using System.Threading;
using MailSendWPF.Configuration;

namespace MailSendWPF
{
    class ServerSendManager
    {
        public ServerSendManager(ServerSchema serverSchema, string guid)
        {
            bool frontend = (bool)Application.Current.Resources["FRONTEND"];
        }
        private Dictionary<string, ServerStatus> serverStatis = new Dictionary<string, ServerStatus>();
        private Dictionary<string, ServerConfig> serverConfigs = new Dictionary<string, ServerConfig>();
        private Object startSemaphore = new Object();

        public ServerConfig GetCorrectServerConfig(string key)
        {
            ServerConfig serverConfig = null;
            try
            {
                serverConfig = serverConfigs[key];
            }
            catch (KeyNotFoundException ex)
            {

                serverConfig = new ServerConfig();
                serverConfigs.Add(key, serverConfig);
            }
            return serverConfig;
        }

        public ServerStatus GetCorrectServer(string key)
        {
            ServerStatus serverStatus = null;
            try
            {
                serverStatus = serverStatis[key];
            }
            catch (KeyNotFoundException ex)
            {

                serverStatus = new ServerStatus();
                serverStatis.Add(key, serverStatus);
            }

            return serverStatus;
        }

        public ServerStatus InitialiseServer(string key, ServerSchema schema)
        {
            ServerStatus serverStatus = null;
            try
            {
                serverStatus = serverStatis[key];
            }
            catch (KeyNotFoundException ex)
            {

                serverStatus = new ServerStatus();
                serverStatis.Add(key, serverStatus);
            }
            serverStatus.Server = schema;
            ServerConfig serverConfig = GetCorrectServerConfig(key);
            serverConfigs[key] = setConfig(serverStatus, serverConfig);
            if (!serverConfig.Parsed)
            {
                serverStatus.ServerImpl = new SelfServer(serverConfig.SmartHost, serverConfig.Port, serverConfig.MailFrom, serverConfig.MailTo, serverConfig.User, serverConfig.Pwd, serverConfig.Rounds, serverConfig.Connections, serverConfig.EndlessSending, serverConfig.MailAttributes, serverConfig.Parsed, serverConfig.WaitBetweenMailsms, serverConfig.SendOriginal, serverConfig.NewMessageID, serverConfig.FallBack, key, serverConfig.AddSenderToMail, serverConfig.AddRecipientToMail, serverConfig.AnonymousAuth, serverConfig.Host, serverConfig.CacheMailsUntilInByte, serverConfig.CodePageEnabled, serverConfig.CodePage, serverConfig.RecipientGroup, serverConfig.RecipientGroupStart, serverConfig.RecipientGroupEnd, serverConfig.RecipientGroupDomain, serverConfig.UseRecipientGroup, serverConfig.SubjectMailCount, serverConfig.SubjectAddMailNamePosition, serverConfig.SenderGroup, serverConfig.SenderGroupStart, serverConfig.SenderGroupEnd, serverConfig.SenderGroupDomain, serverConfig.UseSenderGroup, serverConfig.AddHeader, serverConfig.UseNoneSSL, serverConfig.UseSMTPS, serverConfig.UseTLS, serverConfig.IgnoreAllErrors, serverConfig.SendTimeout, serverConfig.ReceiveTimeout, serverConfig.SendBuffer, serverConfig.ReceiveBuffer);
            }
            else
            {
                //serverStatus.ServerImpl = new Client(serverConfig.SmartHost, serverConfig.Port, serverConfig.MailFrom, serverConfig.MailTo, serverConfig.User, serverConfig.Pwd, serverConfig.Rounds, serverConfig.Connections, serverConfig.EndlessSending, serverConfig.MailAttributes, serverConfig.Parsed, serverConfig.WaitBetweenMailsms, serverConfig.SendOriginal, serverConfig.NewMessageID, serverConfig.FallBack, key, serverConfig.AddSenderToMail, serverConfig.AddRecipientToMail, serverConfig.AnonymousAuth, serverConfig.Host, serverConfig.CacheMailsUntilInByte, serverConfig.CodePageEnabled, serverConfig.CodePage, serverConfig.RecipientGroup, serverConfig.RecipientGroupStart, serverConfig.RecipientGroupEnd, serverConfig.RecipientGroupDomain, serverConfig.SubjectMailCount);
            }
            serverStatis[key] = serverStatus;
            return serverStatus;
        }

        public ServerStatus InitialiseServerCommandLine(string key, ServerStatus serverStatus)
        {
            serverStatus = null;
            try
            {
                serverStatus = serverStatis[key];
            }
            catch (KeyNotFoundException ex)
            {

                serverStatus = new ServerStatus();
                serverStatis.Add(key, serverStatus);
            }

            ServerConfig serverConfig = new ServerConfig();

            serverConfig = setConfig(serverStatus, serverConfig);
            if (!serverConfig.Parsed)
            {
                serverStatus.ServerImpl = new SelfServer(serverConfig.SmartHost, serverConfig.Port, serverConfig.MailFrom, serverConfig.MailTo, serverConfig.User, serverConfig.Pwd, serverConfig.Rounds, serverConfig.Connections, serverConfig.EndlessSending, serverConfig.MailAttributes, serverConfig.Parsed, serverConfig.WaitBetweenMailsms, serverConfig.SendOriginal, serverConfig.NewMessageID, serverConfig.FallBack, key, serverConfig.AddSenderToMail, serverConfig.AddRecipientToMail, serverConfig.AnonymousAuth, serverConfig.Host, serverConfig.CacheMailsUntilInByte, serverConfig.CodePageEnabled, serverConfig.CodePage, serverConfig.RecipientGroup, serverConfig.RecipientGroupStart, serverConfig.RecipientGroupEnd, serverConfig.RecipientGroupDomain, serverConfig.UseRecipientGroup, serverConfig.SubjectMailCount, serverConfig.SubjectAddMailNamePosition, serverConfig.SenderGroup, serverConfig.SenderGroupStart, serverConfig.SenderGroupEnd, serverConfig.SenderGroupDomain, serverConfig.UseSenderGroup, serverConfig.AddHeader, serverConfig.UseNoneSSL, serverConfig.UseSMTPS, serverConfig.UseTLS, serverConfig.IgnoreAllErrors, serverConfig.SendTimeout, serverConfig.ReceiveTimeout, serverConfig.SendBuffer, serverConfig.ReceiveBuffer);
            }
            else
            {
                //serverStatus.ServerImpl = new Client(serverConfig.SmartHost, serverConfig.Port, serverConfig.MailFrom, serverConfig.MailTo, serverConfig.User, serverConfig.Pwd, serverConfig.Rounds, serverConfig.Connections, serverConfig.EndlessSending, serverConfig.MailAttributes, serverConfig.Parsed, serverConfig.WaitBetweenMailsms, serverConfig.SendOriginal, serverConfig.NewMessageID, serverConfig.FallBack, key, serverConfig.AddSenderToMail, serverConfig.AddRecipientToMail, serverConfig.AnonymousAuth, serverConfig.Host, serverConfig.CacheMailsUntilInByte, serverConfig.CodePageEnabled, serverConfig.CodePage, serverConfig.RecipientGroup, serverConfig.RecipientGroupStart, serverConfig.RecipientGroupEnd, serverConfig.RecipientGroupDomain, serverConfig.SubjectMailCount);
            }
            
            return serverStatus;
        }

        public ServerSendManager()
        {

        }
        public void AboardThreads(string key)
        {
            GetCorrectServer(key).ServerImpl.AboardThreads();
        }


        public ServerConfig setConfig(ServerStatus serverStatus, ServerConfig serverConfig)
        {
            //ToDo Binding erzeugen, am Besten direkt mit der Config ServerSchema
            bool frontend = (bool)Application.Current.Resources["FRONTEND"];
            if (frontend)
            {
                //resToMailPanel1.lb


                serverConfig.MailTo = serverStatus.Server.MailTo;
                serverConfig.MailFrom = serverStatus.Server.MailFrom;
                if (Uri.CheckHostName(serverStatus.Server.SmartHost) == UriHostNameType.IPv4 || Uri.CheckHostName(serverStatus.Server.SmartHost) == UriHostNameType.IPv6)
                {
                    serverConfig.SmartHost = IPAddress.Parse(serverStatus.Server.SmartHost);
                }
                else
                {
                    serverConfig.Host = serverStatus.Server.SmartHost;
                }
                serverConfig.AnonymousAuth = serverStatus.Server.AnonymousAuth;
                serverConfig.BasicAuth = serverStatus.Server.BasicAuth;
                serverConfig.Port = Int32.Parse(serverStatus.Server.Port);
                serverConfig.User = serverStatus.Server.User;
                serverConfig.Pwd = serverStatus.Server.Pwd;
                serverConfig.Dirs = serverStatus.Server.MailDir;


                char[] seps = new char[2];
                seps[0] = ',';
                seps[1] = ';';

                string[] filtersArr = serverStatus.Server.Filter.Split(seps);
                serverConfig.Filters = new System.Collections.ObjectModel.ObservableCollection<string>(filtersArr.ToList<String>());

                serverConfig.Rounds = serverStatus.Server.Rounds;
                serverConfig.CacheMailsUntilInByte = serverStatus.Server.CacheMailsUntilInByte;
                serverConfig.Connections = Int32.Parse(serverStatus.Server.Connections);
                serverConfig.EndlessSending = serverStatus.Server.EndlessSending;
                serverConfig.Recursive = serverStatus.Server.Recursive;
                serverConfig.MailAttributes = new MailAttributes();
                if (serverStatus.Server.Header.Count > 0)
                {
                    foreach (ServerSchemaHeader mySRVSchemaHeader in serverStatus.Server.Header)
                    {
                        MailAttributes.SHeader header = new MailAttributes.SHeader();
                        header.name = mySRVSchemaHeader.Name;
                        header.value = mySRVSchemaHeader.Value;
                        header.addHeaderOnce = MailAttributes.AttributeInsertMethod.change;
                        serverConfig.MailAttributes.Headers.Add(header);
                    }

                    //header
                }
                ServerSchemaSubject serverSchemaSubject = serverStatus.Server.Subject;
                MailAttributes.SSubject mySubject = new MailAttributes.SSubject();
                mySubject.subjectstring = serverSchemaSubject.SubjectExtensionValue;
                if (serverSchemaSubject.SubjectExtentAtBegin)
                {
                    mySubject.position = MailAttributes.HeaderPosition.begin;
                }
                else if (serverSchemaSubject.SubjectExtentAtEnd)
                {
                    mySubject.position = MailAttributes.HeaderPosition.end;
                }
                else if (serverSchemaSubject.SubjectNone)
                {
                    mySubject.position = MailAttributes.HeaderPosition.none;
                }
                serverConfig.SubjectMailName = serverStatus.Server.Subject.SubjectMailName;
                if (serverConfig.SubjectMailName)
                {
                    if (serverStatus.Server.Subject.SubjectMailNameBegin)
                    {
                        serverConfig.SubjectAddMailNamePosition = StringHelper.StringHelper.ESubjectAddMailNamePosition.Begin;
                    }
                    else if (serverStatus.Server.Subject.SubjectMailNameEnd)
                    {
                        serverConfig.SubjectAddMailNamePosition = StringHelper.StringHelper.ESubjectAddMailNamePosition.End;
                    }
                    else if (serverStatus.Server.Subject.SubjectMailNameReplace)
                    {
                        serverConfig.SubjectAddMailNamePosition = StringHelper.StringHelper.ESubjectAddMailNamePosition.Replace;
                    }
                }
                else
                {
                    serverConfig.SubjectAddMailNamePosition = StringHelper.StringHelper.ESubjectAddMailNamePosition.None;
                }
                
                serverConfig.MailAttributes.Subject = mySubject;


                serverConfig.Parsed = serverStatus.Server.Parsed;
                serverConfig.WaitBetweenMailsms = Int32.Parse(serverStatus.Server.WaitBetweenMailsms);
                serverConfig.SendOriginal = serverStatus.Server.SendOriginal;

                if (serverStatus.Server.NewMessageID || serverStatus.Server.DeleteMessageID)
                {
                    serverConfig.NewMessageID = true;
                }
                else
                {
                    serverConfig.NewMessageID = false;
                }
                //serverConfig.NewMessageID = serverStatus.Server.NewMessageID;
                serverConfig.FallBack = serverStatus.Server.FallBack;
                serverConfig.AddSenderToMail = serverStatus.Server.AddSenderToMail;
                serverConfig.AddRecipientToMail = serverStatus.Server.AddRecipientToMail;
                serverConfig.CodePageEnabled = serverStatus.Server.CodePageEnabled;
                serverConfig.CodePage = Int32.Parse(serverStatus.Server.CodePage);
                serverConfig.FallBack = serverStatus.Server.FallBack;
                //RecipientGroup
                serverConfig.RecipientGroup = serverStatus.Server.RecipientGroup;
                serverConfig.RecipientGroupStart = Int32.Parse(serverStatus.Server.RecipientGroupStart);
                serverConfig.RecipientGroupEnd = Int32.Parse(serverStatus.Server.RecipientGroupEnd);
                serverConfig.RecipientGroupDomain = serverStatus.Server.RecipientGroupDomain;
                serverConfig.UseRecipientGroup = serverStatus.Server.CHKUseRecipientGroup;

                serverConfig.SenderGroup = serverStatus.Server.SenderGroup;
                serverConfig.SenderGroupStart = Int32.Parse(serverStatus.Server.SenderGroupStart);
                serverConfig.SenderGroupEnd = Int32.Parse(serverStatus.Server.SenderGroupEnd);
                serverConfig.SenderGroupDomain = serverStatus.Server.SenderGroupDomain;
                serverConfig.UseSenderGroup = serverStatus.Server.CHKUseSenderGroup;

                serverConfig.SubjectMailCount = serverStatus.Server.Subject.SubjectMailCount;
                serverConfig.AddHeader = serverStatus.Server.AddHeader;

                serverConfig.UseNoneSSL = serverStatus.Server.NoSSL;
                serverConfig.UseTLS = serverStatus.Server.UseTLS;
                serverConfig.UseSMTPS = serverStatus.Server.UseSSLSecuredPort;

                serverConfig.IgnoreAllErrors = serverStatus.Server.IgnoreAllErrors;

                serverConfig.SendTimeout = Int32.Parse(serverStatus.Server.SendTimeout);
                serverConfig.ReceiveTimeout = Int32.Parse(serverStatus.Server.ReceiveTimeout);
                serverConfig.SendBuffer = Int32.Parse(serverStatus.Server.SendBuffer);
                serverConfig.ReceiveBuffer = Int32.Parse(serverStatus.Server.ReceiveBuffer);

                //KeyManager
                serverConfig.KeyManagerTest = serverStatus.Server.KeyManagerTest;
                serverConfig.KeyManagerUse = serverStatus.Server.KeyManagerUse;
                serverConfig.PrivateKeyType = serverStatus.Server.PrivateKeyType;
                serverConfig.PublicKeyType = serverStatus.Server.PublicKeyType;
                serverConfig.Get509CertificateRequest = serverStatus.Server.Get509CertificateRequest;
                serverConfig.GetAllPrivateKeysForEmailRequest = serverStatus.Server.GetAllPrivateKeysForEmailRequest;
                serverConfig.GetAllX509CertificatesForEmailRequest = serverStatus.Server.GetAllX509CertificatesForEmailRequest;
                serverConfig.GetOrCreatePrivateKeyAdvancedRequest = serverStatus.Server.GetOrCreatePrivateKeyAdvancedRequest;
                serverConfig.GetOrCreatePrivateKeyRequest = serverStatus.Server.GetOrCreatePrivateKeyRequest;
                serverConfig.GetPrivateKeyRequest = serverStatus.Server.GetPrivateKeyRequest;
                serverConfig.ImportX509CertificateRequest = serverStatus.Server.ImportX509CertificateRequest;
                serverConfig.Tenants = serverStatus.Server.Tenants;
                serverConfig.ImportFromTenant = serverStatus.Server.ImportFromTenant;

                serverConfig.MakeCyclicCertsForAllTenantsSame = serverStatus.Server.MakeCyclicCertsForAllTenantsSame;
                serverConfig.GetPrivateKeyFromEnvelopeRequest = serverStatus.Server.GetPrivateKeyFromEnvelopeRequest;
                serverConfig.GetX509CertificateForVerificationBySignedDataRequest = serverStatus.Server.GetX509CertificateForVerificationBySignedDataRequest;
                
            }
            return serverConfig;

        }

        public bool SetRunEnded(string key)
        {
            ServerStatus serverStatus = GetCorrectServer(key);
            if (serverStatus == null) return false;
            serverStatus.Run = false;
            return true;
        }

        public void start(string key)
        {
            lock (startSemaphore)
            {
                ServerStatus serverStatus = GetCorrectServer(key);
                ServerConfig serverConfig = GetCorrectServerConfig(key);
                if (!serverStatus.Run)
                {
                    serverStatus.Run = true;
                    serverStatus.Guid = key;
                    serverStatus.StartTime = DateTime.Now;
                    ulong maxMails = serverStatus.ServerImpl.FillQueue(serverConfig.Dirs.ToList<String>(), serverConfig.Recursive, serverConfig.Filters.ToList<String>(), (ulong)serverConfig.Connections, serverConfig.EndlessSending, key);
                    /*
                     * ToDo:Thread auslösen das alle Mails gesendet wurden. Das es fertig ist -> Thread.CurrentThread.Abort ausprobieren
                    if (maxMails <= 0)
                    {
                        serverStatus.Run = false;
                        serverStatus.ServerImpl.RequestStop();
                        AboardThreads(key);
                        return;
                    }
                     */
                    //event mit maximaler Anzahl der mails schmeissen
                    
                    Thread.Sleep(3);
                    //ThreadStart mimeSend = new ThreadStart(serverStatus.ServerImpl.SetMimeAndSend);
                    //Thread myThread = new Thread(mimeSend);
                    //myThread.Start();
                    if (serverConfig.KeyManagerUse && serverConfig.KeyManagerTest)
                    {
                        MailSend.Constants.KeyManager keyManager = new Constants.KeyManager();
                        keyManager.KeyManagerTest = serverConfig.KeyManagerTest;
                        keyManager.KeyManagerUse = serverConfig.KeyManagerUse;
                        keyManager.Tenants = serverConfig.Tenants;
                        keyManager.ImportX509CertificateRequest = serverConfig.ImportX509CertificateRequest;
                        keyManager.Get509CertificateRequest = serverConfig.Get509CertificateRequest;
                        keyManager.GetAllPrivateKeysForEmailRequest = serverConfig.GetAllPrivateKeysForEmailRequest;
                        keyManager.GetAllX509CertificatesForEmailRequest = serverConfig.GetAllX509CertificatesForEmailRequest;
                        keyManager.GetOrCreatePrivateKeyAdvancedRequest = serverConfig.GetOrCreatePrivateKeyAdvancedRequest;
                        keyManager.GetOrCreatePrivateKeyRequest = serverConfig.GetOrCreatePrivateKeyRequest;
                        keyManager.GetPrivateKeyRequest = serverConfig.GetPrivateKeyRequest;
                        keyManager.GetX509CertificateForVerificationBySignedDataRequest = serverConfig.GetX509CertificateForVerificationBySignedDataRequest;
                        keyManager.GetPrivateKeyFromEnvelopeRequest = serverConfig.GetPrivateKeyFromEnvelopeRequest;
                        keyManager.MakeCyclicCertsForAllTenantsSame = serverConfig.MakeCyclicCertsForAllTenantsSame;
                        keyManager.ImportFromTenant = serverConfig.ImportFromTenant;
                        keyManager.PrivateKeyType = serverConfig.PrivateKeyType;
                        keyManager.PublicKeyType = serverConfig.PublicKeyType;

                        serverStatus.ServerImpl.SetKeyManager(keyManager);
                        serverStatus.ServerImpl.SetKeyManagerAndSend();
                    }
                    else
                    {
                        serverStatus.ServerImpl.SetMimeAndSend();
                    }

                }
                else
                {
                    serverStatus.Run = false;
                    serverStatus.ServerImpl.RequestStop();
                }
            }


        }


        public void setConfig(string key)
        {
            //ToDo Binding erzeugen, am Besten direkt mit der Config ServerSchema
            bool frontend = (bool)Application.Current.Resources["FRONTEND"];
            if (frontend)
            {
                ServerStatus serverStatus = GetCorrectServer(key);
                ServerConfig serverConfig = GetCorrectServerConfig(key);
                //resToMailPanel1.lb


                serverConfig.MailTo = serverStatus.Server.MailTo;
                serverConfig.MailFrom = serverStatus.Server.MailFrom;
                if (Uri.CheckHostName(serverStatus.Server.Host) == UriHostNameType.IPv4 || Uri.CheckHostName(serverStatus.Server.Host) == UriHostNameType.IPv6)
                {
                    serverConfig.SmartHost = IPAddress.Parse(serverStatus.Server.Host);
                }
                else
                {
                    serverConfig.Host = serverStatus.Server.Host;
                }
                serverConfig.Port = Int32.Parse(serverStatus.Server.Port);
                serverConfig.User = serverStatus.Server.User;
                serverConfig.Pwd = serverStatus.Server.Pwd;
                serverConfig.Dirs = serverStatus.Server.MailDir;


                char[] seps = new char[2];
                seps[0] = ',';
                seps[1] = ';';

                string[] filtersArr = serverStatus.Server.Filter.Split(seps);
                serverConfig.Filters = new System.Collections.ObjectModel.ObservableCollection<string>(filtersArr.ToList<String>());

                serverConfig.Rounds = serverStatus.Server.Rounds;
                serverConfig.Connections = Int32.Parse(serverStatus.Server.Connections);
                serverConfig.EndlessSending = serverStatus.Server.EndlessSending;
                serverConfig.Recursive = serverStatus.Server.Recursive;
                if (serverStatus.Server.Header.Count > 0)
                {
                    foreach (ServerSchemaHeader mySRVSchemaHeader in serverStatus.Server.Header)
                    {
                        MailAttributes.SHeader header = new MailAttributes.SHeader();
                        header.name = mySRVSchemaHeader.Name;
                        header.value = mySRVSchemaHeader.Value;
                        serverConfig.MailAttributes.Headers.Add(header);

                    }

                    //header
                }
                ServerSchemaSubject serverSchemaSubject = serverStatus.Server.Subject;
                MailAttributes.SSubject mySubject = new MailAttributes.SSubject();
                mySubject.subjectstring = serverSchemaSubject.SubjectExtensionValue;
                if (serverSchemaSubject.SubjectExtentAtBegin)
                {
                    mySubject.position = MailAttributes.HeaderPosition.begin;
                }
                else if (serverSchemaSubject.SubjectExtentAtEnd)
                {
                    mySubject.position = MailAttributes.HeaderPosition.end;
                }
                else if (serverSchemaSubject.SubjectNone)
                {
                    mySubject.position = MailAttributes.HeaderPosition.none;
                }

                serverConfig.MailAttributes.Subject = mySubject;
                serverConfig.Parsed = serverStatus.Server.Parsed;
                serverConfig.WaitBetweenMailsms = Int32.Parse(serverStatus.Server.WaitBetweenMailsms);
                serverConfig.SendOriginal = serverStatus.Server.SendOriginal;
                if (serverStatus.Server.NewMessageID || serverStatus.Server.DeleteMessageID)
                {
                    serverConfig.NewMessageID = true;
                }
                else
                {
                    serverConfig.NewMessageID = false;
                }
                //serverConfig.NewMessageID = serverStatus.Server.NewMessageID;
                serverConfig.FallBack = serverStatus.Server.FallBack;
                serverConfig.RecipientGroup = serverStatus.Server.RecipientGroup;
                serverConfig.RecipientGroupStart = Int32.Parse(serverStatus.Server.RecipientGroupStart);
                serverConfig.RecipientGroupEnd = Int32.Parse(serverStatus.Server.RecipientGroupEnd);
                serverConfig.RecipientGroupDomain = serverStatus.Server.RecipientGroupDomain;
                serverConfig.SubjectMailCount = serverStatus.Server.Subject.SubjectMailCount;
            }


        }

        public void start2(string key)
        {
            ServerStatus serverStatus = GetCorrectServer(key);
            ServerConfig serverConfig = GetCorrectServerConfig(key);
            if (!serverStatus.Run)
            {
                serverStatus.Run = true;
                serverStatus.Guid = key;
                serverStatus.ServerImpl.FillQueue(serverConfig.Dirs.ToList<String>(), serverConfig.Recursive, serverConfig.Filters.ToList<String>(), (ulong)serverConfig.Connections, serverConfig.EndlessSending, key);
                serverStatus.ServerImpl.SetMimeAndSend();
            }
            else
            {
                serverStatus.Run = false;
            }


        }
        /*
        private void start2(string[] args)
        {

            CommandLine cmd = new CommandLine(args, true);
            Log myLog = new Log();
            //Server.ParseMailMessage();

            string mailTo = @"testuser1@jh1.local";
            string mailFrom = @"test@test.local";
            IPAddress smartHost = IPAddress.Parse("172.30.200.139");
            string host = "";
            int port = 25;
            string user = "";
            string pwd = "";
            string dir = @"c:\Mails\zwei";
            string filter = @"*.eml";
            ulong rounds = 1;
            int connections = 1;
            bool endlessSending = false;
            bool recursive = false;
            string subject = String.Empty;
            string headerKey = String.Empty;
            string headerValue = String.Empty;
            MailAttributes mailAttributes = null;
            bool parsed = true;
            int waitBetweenMailsms = 0;
            bool sendOriginal = false;
            bool newMessageID = false;
            bool fallBack = true;



            mailTo = System.Configuration.ConfigurationManager.AppSettings["mailto"];
            mailFrom = System.Configuration.ConfigurationManager.AppSettings["mailfrom"];
            port = Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["port"]);
            user = System.Configuration.ConfigurationManager.AppSettings["user"];
            pwd = System.Configuration.ConfigurationManager.AppSettings["pwd"];
            dir = System.Configuration.ConfigurationManager.AppSettings["maildir"];
            filter = System.Configuration.ConfigurationManager.AppSettings["filter"];
            rounds = UInt64.Parse(System.Configuration.ConfigurationManager.AppSettings["rounds"]);
            connections = Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["connections"]);
            endlessSending = Boolean.Parse(System.Configuration.ConfigurationManager.AppSettings["endlessSending"]);
            recursive = Boolean.Parse(System.Configuration.ConfigurationManager.AppSettings["recursive"]);
            parsed = Boolean.Parse(System.Configuration.ConfigurationManager.AppSettings["parsed"]);
            waitBetweenMailsms = Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["waitBetweenMails"]);
            sendOriginal = Boolean.Parse(System.Configuration.ConfigurationManager.AppSettings["sendOriginal"]);
            newMessageID = Boolean.Parse(System.Configuration.ConfigurationManager.AppSettings["newMessageID"]);
            fallBack = Boolean.Parse(System.Configuration.ConfigurationManager.AppSettings["fallback"]);

            subject = System.Configuration.ConfigurationManager.AppSettings["subject"];
            mailAttributes = CreateMailAttributes(mailAttributes);
            MailSend.MailAttributes.SSubject mailSubject = new MailAttributes.SSubject();
            mailSubject.position = MailAttributes.SubjectPosition.end;
            mailSubject.subjectstring = subject;
            mailAttributes.Subject = mailSubject;



            string tempHost = System.Configuration.ConfigurationManager.AppSettings["hostip"];
            if (!IPAddress.TryParse(tempHost, out smartHost))
            {
                UriHostNameType hostNameType = System.Uri.CheckHostName(tempHost);
                if (hostNameType != UriHostNameType.Unknown)
                {
                    host = tempHost;
                }
            }

            for (int i = 0; i <= 999999; i++)
            {
                string headerName = "header" + i.ToString();
                string tempHeader = System.Configuration.ConfigurationManager.AppSettings[headerName];
                if (String.IsNullOrEmpty(tempHeader))
                {
                    break;
                }
                string[] stringHeaderNameValue = tempHeader.Split(',');
                if (stringHeaderNameValue.Length < 2)
                {
                    break;
                }
                mailAttributes = CreateMailAttributes(mailAttributes);
                MailSend.MailAttributes.SHeader sheader = new MailAttributes.SHeader();

                sheader.name = stringHeaderNameValue[0];
                sheader.value = stringHeaderNameValue[1];
                if (stringHeaderNameValue.Length == 3)
                {
                    string howToAddHeader = stringHeaderNameValue[2];
                    if (howToAddHeader.Equals("change"))
                    {
                        sheader.addHeaderOnce = MailAttributes.AttributeInsertMethod.change;
                    }
                    else if (howToAddHeader.Equals("addOnce"))
                    {
                        sheader.addHeaderOnce = MailAttributes.AttributeInsertMethod.addOnce;
                    }
                    else if (howToAddHeader.Equals("addAlways"))
                    {
                        sheader.addHeaderOnce = MailAttributes.AttributeInsertMethod.addAlways;
                    }

                }
                else
                {
                    sheader.addHeaderOnce = MailAttributes.AttributeInsertMethod.change;
                }
                mailAttributes.Headers.Add(sheader);
            }


            if (cmd.Parameters.ContainsKey("mailto"))
            {
                mailTo = cmd.Parameters["mailto"];
            }
            if (cmd.Parameters.ContainsKey("mailfrom"))
            {
                mailFrom = cmd.Parameters["mailfrom"];
            }
            if (cmd.Parameters.ContainsKey("hostip"))
            {
                smartHost = IPAddress.Parse(cmd.Parameters["hostip"]);
            }
            if (cmd.Parameters.ContainsKey("port"))
            {
                port = Int32.Parse(cmd.Parameters["port"]);
            }
            if (cmd.Parameters.ContainsKey("user"))
            {
                user = cmd.Parameters["user"];
            }

            if (cmd.Parameters.ContainsKey("pwd"))
            {
                pwd = cmd.Parameters["pwd"];
            }

            if (cmd.Parameters.ContainsKey("maildir"))
            {
                dir = cmd.Parameters["maildir"];
            }

            if (cmd.Parameters.ContainsKey("filter"))
            {
                filter = cmd.Parameters["filter"];
            }

            if (cmd.Parameters.ContainsKey("rounds"))
            {
                rounds = UInt64.Parse(cmd.Parameters["rounds"]);
            }

            if (cmd.Parameters.ContainsKey("connections"))
            {
                connections = Int32.Parse(cmd.Parameters["connections"]);
            }

            if (cmd.Parameters.ContainsKey("endlessSending"))
            {
                endlessSending = Boolean.Parse(cmd.Parameters["endlessSending"]);
            }

            if (cmd.Parameters.ContainsKey("recursive"))
            {
                recursive = Boolean.Parse(cmd.Parameters["recursive"]);
            }
            if (cmd.Parameters.ContainsKey("subject"))
            {
                subject = cmd.Parameters["subject"];
                mailAttributes = CreateMailAttributes(mailAttributes);
                mailSubject = new MailAttributes.SSubject();
                mailSubject.position = MailAttributes.SubjectPosition.end;
                mailSubject.subjectstring = subject;
                mailAttributes.Subject = mailSubject;
            }
            if (cmd.Parameters.ContainsKey("headerKey"))
            {
                headerKey = cmd.Parameters["headerKey"];
            }
            if (cmd.Parameters.ContainsKey("headerValue"))
            {
                headerValue = cmd.Parameters["headerValue"];
                mailAttributes = CreateMailAttributes(mailAttributes);
                MailSend.MailAttributes.SHeader sheader = new MailAttributes.SHeader();
                sheader.name = headerKey;
                sheader.value = headerValue;
                mailAttributes.Headers.Add(sheader);
            }
            if (cmd.Parameters.ContainsKey("parsed"))
            {
                parsed = Boolean.Parse(cmd.Parameters["parsed"]);
            }
            if (cmd.Parameters.ContainsKey("waitBetweenMailsms"))
            {
                waitBetweenMailsms = Int32.Parse(cmd.Parameters["waitBetweenMailsms"]);
            }
            if (cmd.Parameters.ContainsKey("sendOriginal"))
            {
                sendOriginal = Boolean.Parse(cmd.Parameters["sendOriginal"]);
            }
            if (cmd.Parameters.ContainsKey("newMessageID"))
            {
                newMessageID = Boolean.Parse(cmd.Parameters["newMessageID"]);
            }
            if (cmd.Parameters.ContainsKey("fallBack"))
            {
                fallBack = Boolean.Parse(cmd.Parameters["fallBack"]);
            }





            Client server1 = new Client(smartHost, port, mailFrom, mailTo, user, pwd, rounds, connections, endlessSending, mailAttributes, parsed, waitBetweenMailsms, sendOriginal, newMessageID, fallBack);

            List<string> dirs = new List<string>();
            List<string> filters = new List<string>();
            filters.Add(filter);
            dirs.Add(dir);
            server1.FillQueue(dirs, recursive, filters);
            MailState mailState = server1.SetMimeAndSend();
            Console.WriteLine("Sent Mails: " + (mailState.CountWholeMailsSend - mailState.ErrorsWhileSendingMails));
            Console.WriteLine("Whole Mails in Queue: " + mailState.WholeMailsToSend);
            Console.WriteLine("Errors while mailsending: " + mailState.ErrorsWhileSendingMails);

            if (MailState.errorMails.Count > 0)
            {
                Console.Out.WriteLine("Mails which couldn't been sent: ");
                foreach (MessageWrapper msg in MailState.errorMails)
                {
                    Console.Out.WriteLine(msg.MessagePath);
                    Console.Out.WriteLine("reason:");
                    foreach (Exception ex in msg.Ex)
                    {
                        ex.Message.ToString();
                    }
                }
            }
            Log.logger.Info("Sent Mails: " + mailState.CountWholeMailsSend);
            Log.logger.Info("Whole Mails in Queue: " + mailState.WholeMailsToSend);

        }
        public static MailAttributes CreateMailAttributes(MailAttributes mailAttributes)
        {
            if (mailAttributes == null)
            {
                mailAttributes = new MailAttributes();
            }
            return mailAttributes;
        }

        public static void testUebergabe(int test, string teststring)
        {
            test++;
            teststring = "jan";
        }
         */
    }

}
