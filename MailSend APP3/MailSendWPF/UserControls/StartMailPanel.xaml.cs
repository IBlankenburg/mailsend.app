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
using MailSendWPF.Windows;

namespace MailSendWPF.UserControls
{
    /// <summary>
    /// Interaction logic for StartMailPanel.xaml
    /// </summary>
    public partial class StartMailPanel : UserControl
    {
        public static readonly RoutedEvent MailSendEvent;
        public delegate void MailSendRoutedEventArgs(object myObject);
        static StartMailPanel()
        {
            MailSendEvent = EventManager.RegisterRoutedEvent("MailSend", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(StartMailPanel));
        }
        public event RoutedEventHandler MailSend
        {
            add { AddHandler(MailSendEvent, value); }
            remove { RemoveHandler(MailSendEvent, value); }
        }
        protected virtual void OnMailSend()
        {
            RaiseEvent(new RoutedEventArgs(MailSendEvent, this));
        }
        public static readonly DependencyProperty IdProperty = DependencyProperty.Register("Id", typeof(string), typeof(StartMailPanel));
        private String id = String.Empty;

        public String Id
        {
            get { return (string)GetValue(IdProperty); }
            set { SetValue(IdProperty, value); }
        }
        public StartMailPanel()
        {
            InitializeComponent();
           
            
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Console.Out.WriteLine("ID in StartMailsPanel: " + Id);
            RaiseEvent(new RoutedEventArgs(MailSendEvent, this));
            e.Handled = true;
            //MailSendWindow mailSendWindow = new MailSendWindow();
            //mailSendWindow.DataContext = this.DataContext;
            //mailSendWindow.Left = startButton.C
            //mailSendWindow.Show();
        }
    }
}
