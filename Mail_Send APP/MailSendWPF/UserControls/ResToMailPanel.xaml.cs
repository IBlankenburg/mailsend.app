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
using MailSend;
using MailSendWPF.Configuration;

namespace MailSendWPF.UserControls
{
    /// <summary>
    /// Interaction logic for ResToMailPanel.xaml
    /// </summary>
    public partial class ResToMailPanel : UserControl
    {
        public ResToMailPanel()
        {
            InitializeComponent();
        }
        public static readonly DependencyProperty IdProperty = DependencyProperty.Register("Id", typeof(string), typeof(ResToMailPanel));
        private String id = String.Empty;

        public String Id
        {
            get { return (string)GetValue(IdProperty); }
            set { SetValue(IdProperty, value); }
        }
        private void btnOptions_Click(object sender, RoutedEventArgs e)
        {
            Button advancedButton = (Button)sender;
            OptionWindow optionWindow = new OptionWindow("", (ServerSchema)advancedButton.DataContext);
            //optionWindow.DataContext = advancedButton.DataContext;
            bool? result = optionWindow.ShowDialog();
            e.Handled = true;
            //optionWindow = null;

        }

        private void btnSwitch_Click(object sender, RoutedEventArgs e)
        {
            string mailSender = txtFrom.Text;
            string mailRecipient = txtTo.Text;
            string temp = String.Empty;
            if (mailRecipient.Contains(","))
            {
                int pos = mailRecipient.IndexOf(',');
                string tempMail = mailRecipient.Substring(0, pos);
                string restRecipients = mailRecipient.Substring(pos, mailRecipient.Length - pos);
                mailRecipient = mailSender + restRecipients;
                mailSender = tempMail;
            }
            else if (mailRecipient.Contains(";"))
            {
                int pos = mailRecipient.IndexOf(';');
                string tempMail = mailRecipient.Substring(0, pos);
                string restRecipients = mailRecipient.Substring(pos, mailRecipient.Length - pos);
                mailRecipient = mailSender + restRecipients;
                mailSender = tempMail;
            }
            else
            {
                string tempMail = mailRecipient;
                mailRecipient = mailSender;
                mailSender = tempMail;
            }
            txtFrom.Text = mailSender;
            txtTo.Text = mailRecipient;

        }

        private void txtServer_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox) sender;
            ServerSchema serverSchema = (ServerSchema) textBox.DataContext;
            if (String.IsNullOrEmpty(txtDisplayName.Text))
            {
                txtDisplayName.Text = txtServer.Text;
                serverSchema.DisplayName = txtServer.Text;
            }
        }

        private void txtDisplayName_TargetUpdated(object sender, DataTransferEventArgs e)
        {
            //Application.Current.Windows[0].Title = "0000000000000";
        }

        private void txtDisplayName_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            Application.Current.Resources["DISPLAYNAME"] = textBox.Text;
            ServerSchema serverSchema = (ServerSchema)textBox.DataContext;
            //Application.Current.MainWindow.Title = Application.Current.Windows[0].Title;
            //Application.Current.Resources["SAVE"] = false;
            bool saved = (bool)Application.Current.Resources["SAVE"];
            if (!saved)
            {
                Application.Current.MainWindow.Title = serverSchema.DisplayName + " " + Constants.sVersionString;
            }
            else
            {
                Application.Current.MainWindow.Title = serverSchema.DisplayName + " " + Constants.sVersionString + Constants.sChangedString;
            }
            //Application.Current.Windows[0].Title = "0000000000000";
        }

        /*
        private void txtDisplayName_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            ServerSchema serverSchema = (ServerSchema)textBox.DataContext;
            serverSchema.DisplayName = textBox.Text;

        }
         */ 
    }
}
