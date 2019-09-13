using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using MailSend;
using System.Net;
using StartUp;
using System.Windows.Controls;
using System.IO;
using System.Windows.Media;
using System.Windows.Data;
using MailSendWPF.UserControls;
using System.ComponentModel;
using MailSendWPF.Configuration;



namespace MailSendWPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        [STAThread]
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            
            
            Application.Current.Resources.Add("SAVE", false);

            // e.Args = string[] mit argumenten
            bool frontend = true;
            string configFileName = "Servers.xml";

            //prod.ItemsElementName.Add();
            //prod.ItemsElementName.Add();
            //prod.Items.Add("Apfelsine");
            //prod.Items.Add("1.0");

            frontend = Boolean.Parse(System.Configuration.ConfigurationManager.AppSettings["frontend"]);
            Application.Current.Resources.Add("FRONTEND", frontend);
            string configFile = System.Configuration.ConfigurationManager.AppSettings["configurationFile"];
            if (configFile == null)
            {
                configFile = Directory.GetCurrentDirectory();
                configFile = configFile + "\\" + configFileName;



            }
            Servers servs = null;
            //ToDo: frontend=false
            //frontend = false;
            if (frontend)
            {

                try
                {
                    servs = null;
                    Exception localEx = new Exception();
                    Boolean configOK = Servers.LoadFromFile(configFile, out servs, out localEx);
                    if (!configOK)
                    {
                        configFile = Directory.GetCurrentDirectory() + "\\" + configFileName;
                        configOK = Servers.LoadFromFile(configFile, out servs, out localEx);
                        if (!configOK)
                        {
                            if (File.Exists(configFile))
                            {
                                string newName = configFile + (Guid.NewGuid().ToString());
                                File.Move(configFile, newName);
                            }
                            //Keine Config gefunden erstelle neue
                            servs = initialiseConfig(configFile);
                        }



                    }

                }
                catch (Exception ex)
                {
                }
                Application.Current.Resources.Add("CONFIGFILE", configFile);
                //prod.SaveToFile("produkt.xml", out ex);
                MainWindow wnd = new MainWindow(servs, this);

                //RowDefinition rowDef1 = new RowDefinition();
                // rowDef1.Height = GridLength.Auto;
                //wnd.mainGrid.RowDefinitions.Add(rowDef1);

                //servs.ServerSchema[0].MailFrom = "TEST";

                //servCtrl.DataContext = servs.ServerSchema[0];


                /*
                Binding fromBinding = new Binding();
                fromBinding.NotifyOnSourceUpdated = true;
                fromBinding.NotifyOnTargetUpdated = true;
                fromBinding.NotifyOnValidationError = true;
                fromBinding.Source = servs.ServerSchema[0];
                fromBinding.Path = new PropertyPath("mailFrom");
                fromBinding.Mode = BindingMode.TwoWay;
                servCtrl.resToMailPanel1.txtFrom.SetBinding(TextBox.TextProperty, fromBinding);
                */
                //binding.Source = servs.ServerSchema[0];

                //servs.ServerSchema[0].
                //servCtrl.txtFrom.SetBinding(
                ServerControl servCtrl = null;
                Separator sep = null;
                bool logging = false;
                foreach (ServerSchema server in servs.ServerSchema)
                {
                    if (!logging)
                    {
                        if (server.Logging)
                        {
                            //ToDo: Initialising Logging must be able to disable/enable
                            Log myLog = new Log();
                            logging = true;
                        }
                    }
                    if (string.IsNullOrEmpty(server.DisplayName))
                    {
                        server.DisplayName = server.SmartHost;
                    }
                    //Erzeuge Control für alle Server
                    servCtrl = new ServerControl();
                    servCtrl.DataContext = server;
                    sep = new Separator();
                    if (servs.OneServer)
                    {
                        wnd.ShowServerChooser(true);

                        if (server.Visible)
                        {
                            wnd.cmbChooseServer.SelectedItem = server;
                            servCtrl.Visibility = Visibility.Visible;
                            sep.Visibility = Visibility.Collapsed;
                        }
                        else
                        {
                            servCtrl.Visibility = Visibility.Collapsed;
                            sep.Visibility = Visibility.Collapsed;
                        }
                    }
                    else
                    {
                        wnd.ShowServerChooser(false);
                    }

                    wnd.stackServers.Children.Add(servCtrl);

                    sep.Width = servCtrl.Width;
                    sep.Height = 10;
                    sep.BorderThickness = new Thickness(10, 10, 10, 10);

                    sep.BorderBrush = Brushes.Black;
                    wnd.stackServers.Children.Add(sep);
                    server.PropertyChanged += Model_PropertyChanged;
                    server.Subject.PropertyChanged += Model_PropertyChanged;

                }
                wnd.stackServers.Children.Remove(sep);



                /*
                Grid.SetRow(servCtrl, 0);
                Grid.SetColumn(servCtrl, 0);
                wnd.mainGrid.Children.Add(servCtrl);
                */

                //RowDefinition rowDef2 = new RowDefinition();
                //rowDef2.Height = GridLength.Auto;
                //wnd.mainGrid.RowDefinitions.Add(rowDef2);


                //ServerControl servCtrl2 = new ServerControl();
                //wnd.stackServers.Children.Add(servCtrl2);


                //ServerControl servControl = (ServerControl)wnd.stackServers.Children[0];
                //servControl.DataContext = servs.ServerSchema[0];
                //Grid.SetRow(servCtrl2, 1);
                //Grid.SetColumn(servCtrl2, 0);
                //wnd.mainGrid.Children.Add(servCtrl2);



                //ServerControl servCTRRL = (ServerControl)wnd.stackServers.Children[0];
                //servCTRRL.From = "Hallo Katze";
                //servCTRRL.
                wnd.ShowDialog();
            }//end if Frontend
            else
            {
                //Kommandozeile
                //ServerControl muss initialisiert werden



                try
                {
                    servs = null;
                    Exception localEx = new Exception();
                    Boolean configOK = Servers.LoadFromFile(configFile, out servs, out localEx);
                    if (!configOK)
                    {
                        configFile = Directory.GetCurrentDirectory() + "\\" + configFileName;
                        configOK = Servers.LoadFromFile(configFile, out servs, out localEx);
                        if (!configOK)
                        {
                            if (File.Exists(configFile))
                            {
                                string newName = configFile + (Guid.NewGuid().ToString());
                                File.Move(configFile, newName);
                            }
                            //Keine Config gefunden erstelle neue
                            servs = initialiseConfig(configFile);
                        }
                    }
                }
                catch (Exception ex)
                {
                }
                Application.Current.Resources.Add("CONFIGFILE", configFile);
                MainWindow wnd = new MainWindow(servs, this);

                ServerControl servCtrl = null;
                Separator sep = null;
                bool logging = false;
                foreach (ServerSchema server in servs.ServerSchema)
                {
                    if (!logging)
                    {
                        if (server.Logging)
                        {
                            //ToDo: Initialising Logging must be able to disable/enable
                            Log myLog = new Log();
                            logging = true;
                        }
                    }

                    //Erzeuge Control für alle Server
                    servCtrl = new ServerControl();
                    servCtrl.DataContext = server;
                    sep = new Separator();
                    if (servs.OneServer)
                    {
                        wnd.ShowServerChooser(true);

                        if (server.Visible)
                        {
                            wnd.cmbChooseServer.SelectedItem = server;
                            servCtrl.Visibility = Visibility.Visible;
                            sep.Visibility = Visibility.Collapsed;
                        }
                        else
                        {
                            servCtrl.Visibility = Visibility.Collapsed;
                            sep.Visibility = Visibility.Collapsed;
                        }
                    }
                    else
                    {
                        wnd.ShowServerChooser(false);
                    }

                    wnd.stackServers.Children.Add(servCtrl);

                    sep.Width = servCtrl.Width;
                    sep.Height = 10;
                    sep.BorderThickness = new Thickness(10, 10, 10, 10);

                    sep.BorderBrush = Brushes.Black;
                    wnd.stackServers.Children.Add(sep);
                    server.PropertyChanged += Model_PropertyChanged;
                    server.Subject.PropertyChanged += Model_PropertyChanged;

                }
                wnd.stackServers.Children.Remove(sep);
                //wnd.ShowDialog();
            }
            System.Environment.Exit(0);
        }

        public void Model_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            string changedString = " *CHANGED*";

            bool save = (bool)Application.Current.Resources["SAVE"];
            Application.Current.Resources["SAVE"] = false;
            //Application.Current.MainWindow.FontWeight = FontWeights.Bold;
            if (!Application.Current.MainWindow.Title.Contains(changedString))
            {
                Application.Current.MainWindow.Title = Application.Current.MainWindow.Title + changedString;
                
            }
            //bool save2 = (bool)Application.Current.Resources["SAVE"];

        }

        private Servers initialiseConfig(string configFile)
        {
            Servers servs = new Servers();
            ServerSchema serverSchema = new ServerSchema();
            serverSchema.Visible = true;
            servs.ServerSchema.Add(serverSchema);
            return servs;
        }

        private void start(string[] args)
        {

            CommandLine cmd = new CommandLine(args, true);
            Log myLog = new Log();
            //Server.ParseMailMessage();

            string mailTo = @"testuser1@jh1.local";
            string mailFrom = @"test@test.local";
            IPAddress smartHost = null;//IPAddress.Parse("172.30.200.139");
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
            mailSubject.position = MailAttributes.HeaderPosition.end;
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
                mailSubject.position = MailAttributes.HeaderPosition.end;
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





            //Client server1 = new Client(smartHost, port, mailFrom, mailTo, user, pwd, rounds, connections, endlessSending, mailAttributes, parsed, waitBetweenMailsms, sendOriginal, newMessageID, fallBack, null, false, false, true, "", 0, false, 0, "", 0, 0, "", false);

            List<string> dirs = new List<string>();
            List<string> filters = new List<string>();
            filters.Add(filter);
            dirs.Add(dir);
            //server1.FillQueue(dirs, recursive, filters, (ulong)connections, endlessSending);
            /*
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
             */ 

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

    }
}
