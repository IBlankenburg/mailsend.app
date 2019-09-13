using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MailSendWPF.DesignPattern;
using MailSend;
using System.Net;
using MailSendWPF.Configuration;

namespace MailSendWPF.UserControls
{
    /// <summary>
    /// Interaction logic for ServerControl.xaml
    /// </summary>
    public partial class ServerControl : UserControl, System.ComponentModel.INotifyPropertyChanged
    {
        private ulong tempMailCount = 0;
        private bool run = false;
        private Object startSemaphore = new Object();
        private int m_connections = -1;
        private List<LogMessage> logMessageList;
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        public virtual void OnPropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler handler = this.PropertyChanged;
            if ((handler != null))
            {
                handler(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
        object tempMailCountLock = new object();
        public ulong TempMailCount
        {
            get { return tempMailCount; }
            set
            {
                tempMailCount = value;
                this.OnPropertyChanged("TempMailCount");
            }
        }
        object tempErrorMailCountLock = new object();
        private ulong tempErrorMailCount = 0;
        public ulong TempErrorMailCount
        {
            get { return tempErrorMailCount; }
            set
            {
                tempErrorMailCount = value;
                this.OnPropertyChanged("TempErrorMailCount");
            }
        }

        //Beendete Threads werden hochgezählt. Muss am Ende gleich den Connections sein!
        private int m_ThreadsEnded = 0;
        private Object threadsEndedSemaphore = new Object();
        private Object mailsPlusSemaphore = new Object();

        public static DependencyProperty FromProperty;

        public string From
        {
            get { return (string)GetValue(FromProperty); }
            set { SetValue(FromProperty, value); }
        }

        public static volatile DependencyProperty MailCountProperty
            = DependencyProperty.Register("MailCount", typeof(ulong), typeof(ServerControl));

        private Object mailCountLock = new Object();
        public ulong MailCount
        {
            get { return (ulong)GetValue(MailCountProperty); }
            set
            {

                SetValue(MailCountProperty, value);

            }
        }

        public static readonly DependencyProperty IdProperty = DependencyProperty.Register("Id", typeof(string), typeof(ServerControl));
        private String id = String.Empty;

        public String Id
        {
            get { return (string)GetValue(IdProperty); }
            set { SetValue(IdProperty, value); }
        }

        public ServerControl()
        {
            InitializeComponent();
            //EventManager.RegisterClassHandler(typeof(Button), Button.ClickEvent, new RoutedEventHandler(MyClickHandler));
            EventManager.RegisterClassHandler(typeof(StartMailPanel), StartMailPanel.MailSendEvent, new RoutedEventHandler(MailSendHandler));
            Id = Guid.NewGuid().ToString();
            Console.WriteLine(Id);

            /*
            Binding fromBinding = new Binding();
            fromBinding.Source = txtFrom;
            fromBinding.Path = new PropertyPath("Text");
            fromBinding.Mode = BindingMode.TwoWay;
            serverControl.SetBinding(ServerControl.FromProperty, fromBinding);
             */

            /*
            Binding fromBinding = new Binding();
            fromBinding.Source = serverControl;
            fromBinding.Path = new PropertyPath("From");
            fromBinding.Mode = BindingMode.TwoWay;
            txtFrom.SetBinding(TextBox.TextProperty, fromBinding);
            */
            //From = "Hallo";
        }
        public void SetBindings(string Id)
        {
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
            ServerControl myServerControl = (ServerControl)mainWindow.GetServerControl(Id);

            Binding countBinding = new Binding();
            countBinding.Source = serverControl;
            countBinding.Path = new PropertyPath("TempMailCount");
            countBinding.Mode = BindingMode.OneWay;
            myServerControl.resToMailPanel1.txtActMails.SetBinding(TextBox.TextProperty, countBinding);
            myServerControl.resToMailPanel1.progressBar.SetBinding(ProgressBar.ValueProperty, countBinding);

        }
        public void MailSendHandler(object sender, RoutedEventArgs e)
        {
            lock (startSemaphore)
            {

                StartMailPanel startMailPanel = null;
                if (sender is StartMailPanel)
                {
                    startMailPanel = (StartMailPanel)sender;
                }
                MainWindow tempWin = (MainWindow)Application.Current.MainWindow;
                ServerControl tempServerControl = tempWin.GetServerControl(startMailPanel.Id);
                ServerSchema mySchema = (ServerSchema)tempServerControl.DataContext;
                if (!String.IsNullOrEmpty(tempServerControl.resToMailPanel1.txtServer.Text) && !String.IsNullOrEmpty(tempServerControl.resToMailPanel1.txtPort.Text) && !String.IsNullOrEmpty(tempServerControl.resToMailPanel1.txtFrom.Text) && ((!String.IsNullOrEmpty(tempServerControl.resToMailPanel1.txtTo.Text))) || (mySchema.CHKUseRecipientGroup && mySchema.CHKUseSenderGroup) || (!String.IsNullOrEmpty(tempServerControl.resToMailPanel1.txtTo.Text) && mySchema.CHKUseSenderGroup) || !String.IsNullOrEmpty(tempServerControl.resToMailPanel1.txtFrom.Text) && mySchema.CHKUseRecipientGroup)
                {


                    tempServerControl.startMailPanel1.startButton.IsEnabled = false;



                    //startMailPanel1.startButton.IsEnabled = false;
                    if (!tempServerControl.run)
                    {
                        tempServerControl.resToMailPanel1.IsEnabled = false;

                        tempServerControl.mailsPanel.IsEnabled = false;

                        m_connections = Int32.Parse(mySchema.Connections);
                        tempServerControl.run = true;
                        tempServerControl.mailingOutput.Items.Clear();
                        lock (tempErrorMailCountLock)
                        {
                            TempErrorMailCount = 0;
                        }
                        tempServerControl.resToMailPanel1.txtActMails.Text = "0";
                        tempServerControl.resToMailPanel1.progressBar.Value = 0;
                        TextBox statusBox = (TextBox)tempServerControl.statusBar.Items[0];
                        statusBox.Text = "";
                        tempServerControl.startMailPanel1.startButton.Content = "Stop";
                        ServerStatus serverStatus = Singleton<ServerSendManager>.Instance.InitialiseServer(tempServerControl.Id, (ServerSchema)tempServerControl.DataContext);
                        //serverStatus.StartTime = DateTime.Now;
                        //serverStatus.Server = (ServerSchema)this.DataContext;
                        serverStatus.ServerImpl.MailsendEvent += ServerImpl_MailsendEvent;//new Client.MailsendHandler(ServerImpl_MailsendEvent);
                        serverStatus.ServerImpl.MaxMailsToSendEvent += ServerImpl_MaxMailsToSendEvent;//new Client.MaxMailsToSendHandler(ServerImpl_MaxMailsToSendEvent);
                        serverStatus.ServerImpl.SendMailEndsEvent += ServerImpl_SendMailEndsEvent;//new Client.SendMailEndsHandler(ServerImpl_SendMailEndsEvent);
                        serverStatus.ServerImpl.BeforeMailsentEvent += ServerImpl_BeforeSentMailSendingEvent;
                        serverStatus.ServerImpl.RequestSendEvent += ServerImpl_RequestSendEvent;
                        serverStatus.ServerImpl.RequestSendFailedEvent += ServerImpl_RequestSendFailedEvent;
                        serverStatus.ServerImpl.StatusMessageEvent += ServerImpl_StatusMessageEvent;
                        //serverStatus.ServerImpl.
                        //public delegate void RequestSendHandler(MessageWrapper msg, MailState mailState);
                        //public delegate void RequestSendFailedHandler(MessageWrapper msg, MailState mailState);
                        Singleton<ServerSendManager>.Instance.start(tempServerControl.Id);

                        Console.WriteLine("cool!!!!!!!!!!");
                        tempServerControl.startMailPanel1.startButton.IsEnabled = true;
                        e.Handled = true;
                    }
                    else
                    {
                        //startMailPanel1.startButton.Content = "Start";
                        tempServerControl.run = false;
                        Singleton<ServerSendManager>.Instance.AboardThreads(tempServerControl.Id);
                    }
                }
                else
                {
                    tempServerControl.mailingOutput.Items.Add( new LogMessage("Missing Parameter"));
                }//end if auf Null abfragen

            }
            e.Handled = true;
        }

        public delegate void UpdateSendMailsEndCallback(MailState mailState);
        public void UpdateSendMailsEnd(MailState mailState)
        {
            string key = mailState.Id;
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
            ServerControl myServerControl = (ServerControl)mainWindow.GetServerControl(key);

            lock (threadsEndedSemaphore)
            {

                m_ThreadsEnded++;


                if (m_ThreadsEnded >= m_connections)
                {
                    //Alle Threads beendet
                    ServerStatus serverStatus = Singleton<ServerSendManager>.Instance.GetCorrectServer(mailState.Id);
                    serverStatus.EndTime = DateTime.Now;
                    TimeSpan sendDuration = serverStatus.EndTime.Subtract(serverStatus.StartTime);



                    string endState = "Sent: " + mailState.MailsSent + " Error: " + mailState.ErrorsWhileSendingMails + " Started: " + serverStatus.StartTime.ToString(@"HH\:mm\:ss") + " Duration: " + sendDuration.ToString(@"dd\.hh\:mm\:ss\.fffffff"); 
                    LogMessage myLogMessge = new LogMessage(endState);
                    myServerControl.mailingOutput.Items.Add(myLogMessge);
                    myServerControl.mailingOutput.SelectedItem = myLogMessge;
                    myServerControl.mailingOutput.ScrollIntoView(myLogMessge);

                    if (mailState.Ex.Count > 0)
                    {
                        foreach (Exception ex in mailState.Ex)
                        {
                            LogMessage myLogMessgeError = new LogMessage(mailState.Ex[0].Message);
                            myServerControl.mailingOutput.Items.Add(myLogMessgeError);
                            myServerControl.mailingOutput.SelectedItem = myLogMessgeError;
                            myServerControl.mailingOutput.ScrollIntoView(myLogMessgeError);

                        }
                    }
                    myServerControl.startMailPanel1.startButton.Content = "Start";
                    myServerControl.resToMailPanel1.IsEnabled = true;
                    myServerControl.mailsPanel.IsEnabled = true;
                    myServerControl.run = false;
                    myServerControl.startMailPanel1.startButton.IsEnabled = true;
                    
                    if (!Singleton<ServerSendManager>.Instance.SetRunEnded(mailState.Id))
                    {

                        Log.logger.Error("Run ended couldn't been set in ServerStatus");
                    }
                    m_ThreadsEnded = 0;

                }
            }
        }
        void ServerImpl_SendMailEndsEvent(MailState mailState)
        {
            //Alle threads welche sich beenden melden sich hier

            Dispatcher.BeginInvoke(new UpdateSendMailsEndCallback(UpdateSendMailsEnd), mailState);
        }


        public delegate void UpdateMaxMailsSendingCallback(MailState mailState);
        public void UpdateMaxMailsSending(MailState mailState)
        {
            string key = mailState.Id;


            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
            ServerControl myServerControl = (ServerControl)mainWindow.GetServerControl(key);
            myServerControl.resToMailPanel1.txtActMails.Text = "0";
            try
            {
                myServerControl.resToMailPanel1.progressBar.Maximum = mailState.WholeMailsToSend;
            }
            catch (Exception ex)
            {
            }
            myServerControl.resToMailPanel1.lblWholeMails.Text = mailState.WholeMailsToSend.ToString();
            //SetBindings(key);

        }
        public void ServerImpl_MaxMailsToSendEvent(MailState mailState)
        {
            Dispatcher.BeginInvoke(new UpdateMaxMailsSendingCallback(UpdateMaxMailsSending), mailState);
        }




        public delegate void UpdateMailSendingCallback(MessageWrapper msg, MailState mailState);

        public void UpdateMailSending(MessageWrapper msg, MailState mailState)
        {
            string key = mailState.Id;
            lock (tempMailCountLock)
            {
                TempMailCount++;
            }
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
            ServerControl myServerControl = (ServerControl)mainWindow.GetServerControl(key);
            //TextBox statusBox = (TextBox) myServerControl.statusBar.Items[0];
            if (mailState.EndlessSending)
            {
                try
                {
                    myServerControl.resToMailPanel1.progressBar.Maximum++;
                }
                catch (Exception ex) { }
            }
            try
            {
                myServerControl.resToMailPanel1.progressBar.Value++;
                myServerControl.resToMailPanel1.txtActMails.Text = myServerControl.resToMailPanel1.progressBar.Value.ToString();
            }
            catch (Exception ex)
            {
                myServerControl.resToMailPanel1.txtActMails.Text = "> " + myServerControl.resToMailPanel1.txtActMails.Text;
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
                myServerControl.mailingOutput.Items.Add(myLogMessage);
                myServerControl.mailingOutput.SelectedItem = myLogMessage;
                myServerControl.mailingOutput.ScrollIntoView(myLogMessage);
                //Fehler ist fertig geloggt, nun zurücksetzen, vielleicht klappt es ja beim nächsten mal
                msg.Corrupt = false;
            }
            


        }



        void ServerImpl_BeforeSentMailSendingEvent(MessageWrapper msg, MailState mailState)
        {
            //Aktualisierung der derzeit zu sendenden Mail

            Dispatcher.BeginInvoke(new BeforeMailSendingCallback(BeforeMailSending), msg, mailState);
        }



        public delegate void BeforeMailSendingCallback(MessageWrapper msg, MailState mailState);

        public void BeforeMailSending(MessageWrapper msg, MailState mailState)
        {
            string key = mailState.Id;
            
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
            ServerControl myServerControl = (ServerControl)mainWindow.GetServerControl(key);
            TextBox statusBox = (TextBox)myServerControl.statusBar.Items[0];
            statusBox.Text = msg.MessagePath;

        }

        public void ServerImpl_StatusMessageEvent(string message, string key)
        {
            Dispatcher.BeginInvoke(new StatusMessageCallback(StatusMessage), message, key);
        }

        public delegate void StatusMessageCallback(string message, string key);

        public void StatusMessage(string message, string key)
        {
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
            ServerControl myServerControl = (ServerControl)mainWindow.GetServerControl(key);
            LogMessage myLogMessage = new LogMessage(message);
            myServerControl.mailingOutput.Items.Add(myLogMessage);
            myServerControl.mailingOutput.SelectedItem = myLogMessage;
            myServerControl.mailingOutput.ScrollIntoView(myLogMessage);
            
            //TextBox statusBox = (TextBox)myServerControl.statusBar.Items[0];
            //statusBox.Text = message;

        }



        public void ServerImpl_MailsendEvent(MessageWrapper msg, MailState mailState)
        {
            /*
            lock (mailsPlusSemaphore)
            {
                TempMailCount++;


            }
             */
            Dispatcher.BeginInvoke(new UpdateMailSendingCallback(UpdateMailSending), msg, mailState);
        }


        public delegate void RequestSendingCallback(MessageWrapper msg, MailState mailState);

        public void RequestSending(MessageWrapper msg, MailState mailState)
        {
            //Methode wird vom Frontend ausgeführt

            string key = mailState.Id;
            lock (tempMailCountLock)
            {
                TempMailCount++;
            }
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
            ServerControl myServerControl = (ServerControl)mainWindow.GetServerControl(key);
            //TextBox statusBox = (TextBox) myServerControl.statusBar.Items[0];
            if (mailState.EndlessSending)
            {
                try
                {
                    myServerControl.resToMailPanel1.progressBar.Maximum++;
                }
                catch (Exception ex) { }
            }
            try
            {
                myServerControl.resToMailPanel1.progressBar.Value++;
                myServerControl.resToMailPanel1.txtActMails.Text = myServerControl.resToMailPanel1.progressBar.Value.ToString();
            }
            catch (Exception ex)
            {
                myServerControl.resToMailPanel1.txtActMails.Text = "> " + myServerControl.resToMailPanel1.txtActMails.Text;
            }
        }
        public void ServerImpl_RequestSendEvent(MessageWrapper msg, MailState mailState)
        {
            /*
            lock (mailsPlusSemaphore)
            {
                TempMailCount++;


            }
             */
            Dispatcher.BeginInvoke(new RequestSendingCallback(RequestSending), msg, mailState);
        }


        public delegate void RequestSendFailedCallback(MessageWrapper msg, MailState mailState);

        public void RequestSendFailed(MessageWrapper msg, MailState mailState)
        {
            //Methode wird vom Frontend ausgeführt

            string key = mailState.Id;
            lock (tempMailCountLock)
            {
                TempMailCount++;
            }
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
            ServerControl myServerControl = (ServerControl)mainWindow.GetServerControl(key);
            //string logMessage = msg.MessagePath;
            LogMessage myLogMessage = new LogMessage(msg.MessagePath);
            myServerControl.mailingOutput.Items.Add(myLogMessage);
            myServerControl.mailingOutput.SelectedItem = myLogMessage;
            myServerControl.mailingOutput.ScrollIntoView(myLogMessage);

        }
        public void ServerImpl_RequestSendFailedEvent(MessageWrapper msg, MailState mailState)
        {
            /*
            lock (mailsPlusSemaphore)
            {
                TempMailCount++;


            }
             */
            Dispatcher.BeginInvoke(new RequestSendFailedCallback(RequestSendFailed), msg, mailState);
        }
        static ServerControl()
        {
            FrameworkPropertyMetadata fromMetadata = new FrameworkPropertyMetadata();
            fromMetadata.BindsTwoWayByDefault = true;
            fromMetadata.DefaultUpdateSourceTrigger = UpdateSourceTrigger.Explicit;

            FromProperty = DependencyProperty.Register("From", typeof(string), typeof(UserControl), fromMetadata, new ValidateValueCallback(FromValidation));
        }
        static bool FromValidation(object from)
        {
            return true;
        }
    }
}
