using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MailSendWPF.DesignPattern;
using System.Windows;
using MailSend;
using System.Windows.Threading;
using MailSendWPF.UserControls;

namespace MailSendWPF
{
    class ServerControlCommandLine
    {
        private ulong tempMailCount = 0;
        private Object startSemaphore = new Object();
        private int m_connections = -1;

        object tempErrorMailCountLock = new object();
        private ulong tempErrorMailCount = 0;
        public ulong TempErrorMailCount
        {
            get { return tempErrorMailCount; }
            set
            {
                tempErrorMailCount = value;
                //this.OnPropertyChanged("TempErrorMailCount");
            }
        }

        object tempMailCountLock = new object();
        public ulong TempMailCount
        {
            get { return tempMailCount; }
            set
            {
                tempMailCount = value;
                //this.OnPropertyChanged("TempMailCount");
            }
        }
        private Object threadsEndedSemaphore = new Object();
        //Beendete Threads werden hochgezählt. Muss am Ende gleich den Connections sein!
        private int m_ThreadsEnded = 0;
        public string m_key = string.Empty;

        ServerStatus m_serverStatus = null;

        public ServerControlCommandLine(string key)
        {
            m_key = key;
        }

        public void MailSendHandler(object sender, RoutedEventArgs e)
        {
            lock (startSemaphore)
            {

                //StartMailPanel startMailPanel = null;
                /*
                if (sender is StartMailPanel)
                {
                    startMailPanel = (StartMailPanel)sender;
                }
                 */ 
                //MainWindow tempWin = (MainWindow)Application.Current.MainWindow;
                //ServerControl tempServerControl = tempWin.GetServerControl(startMailPanel.Id);
                //ServerSchema mySchema = (ServerSchema)tempServerControl.DataContext;
                //if (!String.IsNullOrEmpty(tempServerControl.resToMailPanel1.txtServer.Text) && !String.IsNullOrEmpty(tempServerControl.resToMailPanel1.txtPort.Text) && !String.IsNullOrEmpty(tempServerControl.resToMailPanel1.txtFrom.Text) && ((!String.IsNullOrEmpty(tempServerControl.resToMailPanel1.txtTo.Text)) || (!String.IsNullOrEmpty(mySchema.RecipientGroup))))
                //{


                    //tempServerControl.startMailPanel1.startButton.IsEnabled = false;



                    //startMailPanel1.startButton.IsEnabled = false;
                    //if (!tempServerControl.run)
                    //{
                        //tempServerControl.resToMailPanel1.IsEnabled = false;

                        //tempServerControl.mailsPanel.IsEnabled = false;

                       
                        //tempServerControl.run = true;
                        //tempServerControl.mailingOutput.Items.Clear();
                        lock (tempErrorMailCountLock)
                        {
                            TempErrorMailCount = 0;
                        }
                        //tempServerControl.resToMailPanel1.txtActMails.Text = "0";
                        //tempServerControl.resToMailPanel1.progressBar.Value = 0;
                        //TextBox statusBox = (TextBox)tempServerControl.statusBar.Items[0];
                        //statusBox.Text = "";
                        //tempServerControl.startMailPanel1.startButton.Content = "Stop";
                        m_serverStatus = new ServerStatus();
                        
                        //serverStatus = Singleton<ServerSendManager>.Instance.InitialiseServerCommandLine(m_key, serverStatus);
                        m_serverStatus.StartTime = DateTime.Now;
                        //serverStatus.Server = (ServerSchema)this.DataContext;
            m_serverStatus.ServerImpl.MailsendEvent += ServerImpl_MailsendEvent;//new Client.MailsendHandler(ServerImpl_MailsendEvent);
                        m_serverStatus.ServerImpl.MaxMailsToSendEvent += ServerImpl_MaxMailsToSendEvent;//new Client.MaxMailsToSendHandler(ServerImpl_MaxMailsToSendEvent);
                        m_serverStatus.ServerImpl.SendMailEndsEvent += ServerImpl_SendMailEndsEvent;//new Client.SendMailEndsHandler(ServerImpl_SendMailEndsEvent);
                //ToDo: uncomment downunder Implementing Events and start!!!!!!!!!!!!!
                        //m_serverStatus.ServerImpl.BeforeMailsentEvent += ServerImpl_BeforeSentMailSendingEvent;
                        //Singleton<ServerSendManager>.Instance.start(tempServerControl.Id);

                        //Console.WriteLine("cool!!!!!!!!!!");
                        //tempServerControl.startMailPanel1.startButton.IsEnabled = true;
                        e.Handled = true;
                    //}
                    //else
                    //{
                        //startMailPanel1.startButton.Content = "Start";
                        //tempServerControl.run = false;
                        //Singleton<ServerSendManager>.Instance.AboardThreads(tempServerControl.Id);
                    //}
                //}
                //else
                //{
                    //tempServerControl.mailingOutput.Items.Add("Missing Parameter");
                //}//end if auf Null abfragen

            }
            e.Handled = true;
        }



        void ServerImpl_SendMailEndsEvent(MailState mailState)
        {
            //Alle threads welche sich beenden melden sich hier

            UpdateSendMailsEnd(mailState);
        }
        public delegate void UpdateSendMailsEndCallback(MailState mailState);
        public void UpdateSendMailsEnd(MailState mailState)
        {
            string key = mailState.Id;
            //MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
            //ServerControl myServerControl = (ServerControl)mainWindow.GetServerControl(key);

            lock (threadsEndedSemaphore)
            {

                m_ThreadsEnded++;


                if (m_ThreadsEnded >= m_connections)
                {
                    //Alle Threads beendet
                    //ServerStatus serverStatus = Singleton<ServerSendManager>.Instance.GetCorrectServer(mailState.Id);
                    m_serverStatus.EndTime = DateTime.Now;
                    TimeSpan sendDuration = m_serverStatus.EndTime.Subtract(m_serverStatus.StartTime);



                    string endState = "Sent: " + mailState.MailsSent + " Error: " + mailState.ErrorsWhileSendingMails + " Duration: " + sendDuration.ToString(@"dd\.hh\:mm\:ss\.fff");
                    LogMessage myLogMessge = new LogMessage(endState);
                    //myServerControl.mailingOutput.Items.Add(myLogMessge);
                    //myServerControl.mailingOutput.SelectedItem = myLogMessge;
                    //myServerControl.mailingOutput.ScrollIntoView(myLogMessge);

                    if (mailState.Ex.Count > 0)
                    {
                        foreach (Exception ex in mailState.Ex)
                        {
                            LogMessage myLogMessgeError = new LogMessage(mailState.Ex[0].Message);
                            //myServerControl.mailingOutput.Items.Add(myLogMessgeError);
                            //myServerControl.mailingOutput.SelectedItem = myLogMessgeError;
                            //myServerControl.mailingOutput.ScrollIntoView(myLogMessgeError);

                        }
                    }
                    //myServerControl.startMailPanel1.startButton.Content = "Start";
                    //myServerControl.resToMailPanel1.IsEnabled = true;
                    //myServerControl.mailsPanel.IsEnabled = true;
                    //myServerControl.run = false;
                    //myServerControl.startMailPanel1.startButton.IsEnabled = true;

                    if (!Singleton<ServerSendManager>.Instance.SetRunEnded(mailState.Id))
                    {

                        Log.logger.Error("Run ended couldn't been set in ServerStatus");
                    }
                    m_ThreadsEnded = 0;

                }
            }
        }


        public void ServerImpl_MaxMailsToSendEvent(MailState mailState)
        {
            UpdateMaxMailsSending(mailState);
        }

        public delegate void UpdateMaxMailsSendingCallback(MailState mailState);
        public void UpdateMaxMailsSending(MailState mailState)
        {
            string key = mailState.Id;


            //MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
            //ServerControl myServerControl = (ServerControl)mainWindow.GetServerControl(key);
           // myServerControl.resToMailPanel1.txtActMails.Text = "0";
            try
            {
                //myServerControl.resToMailPanel1.progressBar.Maximum = mailState.WholeMailsToSend;
            }
            catch (Exception ex)
            {
            }
            //myServerControl.resToMailPanel1.lblWholeMails.Text = mailState.WholeMailsToSend.ToString();
            //SetBindings(key);

        }


        public void ServerImpl_MailsendEvent(MessageWrapper msg, MailState mailState)
        {
            /*
            lock (mailsPlusSemaphore)
            {
                TempMailCount++;


            }
             */
            //Dispatcher.BeginInvoke(new UpdateMailSendingCallback(UpdateMailSending), msg, mailState);
           // Dispatcher.BeginInvoke(new UpdateMailSendingCallback(UpdateMailSending), msg, mailState);
            UpdateMailSending(msg, mailState);
        }

        public delegate void UpdateMailSendingCallback(MessageWrapper msg, MailState mailState);

        public void UpdateMailSending(MessageWrapper msg, MailState mailState)
        {
            string key = mailState.Id;
            lock (tempMailCountLock)
            {
                TempMailCount++;
            }
            //MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
            //ServerControl myServerControl = (ServerControl)mainWindow.GetServerControl(key);
            //TextBox statusBox = (TextBox) myServerControl.statusBar.Items[0];
            if (mailState.EndlessSending)
            {
                try
                {
                    //myServerControl.resToMailPanel1.progressBar.Maximum++;
                }
                catch (Exception ex) { }
            }
            try
            {
                //myServerControl.resToMailPanel1.progressBar.Value++;
                //myServerControl.resToMailPanel1.txtActMails.Text = myServerControl.resToMailPanel1.progressBar.Value.ToString();
            }
            catch (Exception ex)
            {
               // myServerControl.resToMailPanel1.txtActMails.Text = "> " + myServerControl.resToMailPanel1.txtActMails.Text;
            }
            /*
            if (!msg.Corrupt)
            {
                if (mailState.CountWholeMailsSend < 300000)
                {
                    myServerControl.mailingOutput.Items.Add(msg.MessagePath);
                }
            }
             */


            //if (mailState.CountWholeMailsSend < 300000)

            if (msg.Corrupt)
            {
                lock (tempErrorMailCountLock)
                {
                    TempErrorMailCount++;
                }
                //string endState = "Sent: " + mailState.MailsSent + " Error: " + mailState.ErrorsWhileSendingMails;
                //myServerControl.mailingOutput.Items.Add(new LogMessage(endState));
                string logMessage = TempErrorMailCount + ".Error: " + msg.MessagePath + " " + msg.Ex[msg.Ex.Count - 1].Message.TrimEnd('\r', '\n', '\n');//.Replace(Environment.NewLine, "");
                LogMessage myLogMessage = new LogMessage(logMessage);
                //myServerControl.mailingOutput.Items.Add(myLogMessage);
                //myServerControl.mailingOutput.SelectedItem = myLogMessage;
                //myServerControl.mailingOutput.ScrollIntoView(myLogMessage);
                //Fehler ist fertig geloggt, nun zurücksetzen, vielleicht klappt es ja beim nächsten mal
                msg.Corrupt = false;
            }



        }

    }
}
