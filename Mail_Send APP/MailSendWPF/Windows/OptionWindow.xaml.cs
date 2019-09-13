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
using System.Windows.Shapes;
using System.Globalization;
using System.Collections;
using MailSend;
using MailSendWPF.Configuration;

namespace MailSendWPF.Windows
{
    /// <summary>
    /// Interaction logic for OptionWindow.xaml
    /// </summary>
    public partial class OptionWindow : Window
    {
        public OptionWindow(string key, ServerSchema serverSchema)
        {
            InitializeComponent();
            this.DataContext = serverSchema;
            SetInitialMailingMethod(serverSchema);

        }
        private bool initial = false;
        private void rbAnonymous_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton button = (RadioButton)sender;
            ServerSchema myServerSchema = (ServerSchema)button.DataContext;
            myServerSchema.AnonymousAuth = true;
            myServerSchema.BasicAuth = false;


        }

        private void rbAnonymous_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void rbAnonymous_Unchecked(object sender, RoutedEventArgs e)
        {
            ServerSchema schema = (ServerSchema)((RadioButton)sender).DataContext;
        }

        private void btnAddHeader_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(txtHeaderName.Text) && (!String.IsNullOrEmpty(txtHeaderValue.Text)))
            {
                bool headerNameStillExists = false;
                ServerSchema srv = (ServerSchema)this.DataContext;
                foreach (ServerSchemaHeader schemaHeader in srv.Header)
                {
                    if (schemaHeader.Name.Equals(txtHeaderName.Text))
                    {
                        //HeaderName existiert -> Ändere Headervalue
                        schemaHeader.Value = txtHeaderValue.Text;
                        headerNameStillExists = true;
                        break;
                    }
                }
                if (!headerNameStillExists)
                {
                    ServerSchemaHeader header = new ServerSchemaHeader();
                    header.Name = txtHeaderName.Text;
                    header.Value = txtHeaderValue.Text;
                    srv.Header.Add(header);
                }
            }
        }

        private void btnRemoveHeader_Click(object sender, RoutedEventArgs e)
        {
            IList selectedHeaderElements = headerName.SelectedItems;



            while (selectedHeaderElements.Count > 0)
            {
                ServerSchema srv = (ServerSchema)headerName.DataContext;
                System.Collections.ObjectModel.ObservableCollection<ServerSchemaHeader> colHeader = srv.Header;
                bool removeOK = false;
                do
                {
                    if (selectedHeaderElements.Count <= 0)
                    {
                        break;
                    }
                    ServerSchemaHeader toRemove = (ServerSchemaHeader)selectedHeaderElements[0];
                    removeOK = colHeader.Remove(toRemove);
                } while (removeOK);

            }
        }

        private void btnAddTenant_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(txtTenantDisplayName.Text) && (!String.IsNullOrEmpty(txtTenantValue.Text)))
            {
                bool headerNameStillExists = false;
                ServerSchema srv = (ServerSchema)this.DataContext;
                foreach (ServerSchemaTenants schemaTenant in srv.Tenants)
                {
                    if (schemaTenant.TenantDisplayName.Equals(txtTenantDisplayName.Text))
                    {
                        //HeaderName existiert -> Ändere Headervalue
                        schemaTenant.TenantValue = txtTenantValue.Text;
                        headerNameStillExists = true;
                        break;
                    }
                }
                if (!headerNameStillExists)
                {
                    ServerSchemaTenants tenants = new ServerSchemaTenants();
                    tenants.TenantDisplayName = txtTenantDisplayName.Text;
                    tenants.TenantValue = txtTenantValue.Text;
                    srv.Tenants.Add(tenants);
                }
            }
        }

        private void btnRemoveTenant_Click(object sender, RoutedEventArgs e)
        {
            IList selectedHeaderElements = tenantDisplayName.SelectedItems;



            while (selectedHeaderElements.Count > 0)
            {
                ServerSchema srv = (ServerSchema)tenantDisplayName.DataContext;
                System.Collections.ObjectModel.ObservableCollection<ServerSchemaTenants> colHeader = srv.Tenants;
                bool removeOK = false;
                do
                {
                    if (selectedHeaderElements.Count <= 0)
                    {
                        break;
                    }
                    ServerSchemaTenants toRemove = (ServerSchemaTenants)selectedHeaderElements[0];
                    removeOK = colHeader.Remove(toRemove);
                } while (removeOK);

            }


            /*
            {
                
                System.Collections.ObjectModel.ObservableCollection<ServerSchemaHeader> colHeader = srv.Header;
                
                foreach (ServerSchemaHeader header in selectedHeaderElements)
                {
                    bool removeOK = false;
                    do {
                        removeOK = colHeader.Remove(header);
                    } while (removeOK);
                    selectedHeaderElements = headerName.SelectedItems;
                    if (selectedHeaderElements.Count <= 0)
                    {
                        //break;
                    }
                }
                
            }
             */
        }
        private void OnCommitBindingGroup(object sender, EventArgs e)
        {
            recipientGroupBox.BindingGroup.CommitEdit();
        }

        private void OnCommitBindingGroupSender(object sender, EventArgs e)
        {
            senderGroupBox.BindingGroup.CommitEdit();
        }

        private void headerName_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void headerName_GotFocus(object sender, RoutedEventArgs e)
        {
            ListBox lstBox = (ListBox)sender;
            IList list = lstBox.SelectedItems;

        }

        private void headerName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox lstBox = (ListBox)sender;
            IList list = lstBox.SelectedItems;
            if (list.Count == 1)
            {
                ServerSchemaHeader header = (ServerSchemaHeader)list[0];
                txtHeaderName.Text = header.Name;
                txtHeaderValue.Text = header.Value;

            }

        }

        private void tenantName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox lstBox = (ListBox)sender;
            IList list = lstBox.SelectedItems;
            if (list.Count == 1)
            {
                ServerSchemaTenants tenant = (ServerSchemaTenants)list[0];
                txtTenantDisplayName.Text = tenant.TenantDisplayName;
                txtTenantValue.Text = tenant.TenantValue;

            }

        }

        private void rbSendOriginal_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton button = (RadioButton)sender;
            ServerSchema tmpServerSchema = (ServerSchema)button.DataContext;
            SetMailingMethod(tmpServerSchema);

        }

        private void rbSendOriginal_Unchecked(object sender, RoutedEventArgs e)
        {
            RadioButton button = (RadioButton)sender;
            ServerSchema tmpServerSchema = (ServerSchema)button.DataContext;


        }

        private void chkFallBack_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        private void chkFallBack_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void rbParseHeader_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton button = (RadioButton)sender;
            ServerSchema tmpServerSchema = (ServerSchema)button.DataContext;
            SetMailingMethod(tmpServerSchema);

        }

        private void rbOriginalMessage_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton button = (RadioButton)sender;
            ServerSchema tmpServerSchema = (ServerSchema)button.DataContext;
            SetMailingMethod(tmpServerSchema);

        }

        private void rbParseHeader_Unchecked(object sender, RoutedEventArgs e)
        {
            RadioButton button = (RadioButton)sender;
            ServerSchema tmpServerSchema = (ServerSchema)button.DataContext;

        }

        private void rbOriginalMessage_Unchecked(object sender, RoutedEventArgs e)
        {
            RadioButton button = (RadioButton)sender;
            ServerSchema tmpServerSchema = (ServerSchema)button.DataContext;


        }
        private void SetInitialMailingMethod(ServerSchema tmpServerSchema)
        {

            initial = true;
            rbAnonymous.IsChecked = tmpServerSchema.AnonymousAuth;
            rbBasic.IsChecked = tmpServerSchema.BasicAuth;
            rbSubjectNone.IsChecked = tmpServerSchema.Subject.SubjectNone;
            rbSubjectBegin.IsChecked = tmpServerSchema.Subject.SubjectExtentAtBegin;
            rbSubjectEnd.IsChecked = tmpServerSchema.Subject.SubjectExtentAtEnd;

            rbSubjectMailNameBegin.IsChecked = tmpServerSchema.Subject.SubjectMailNameBegin;
            rbSubjectMailNameEnd.IsChecked = tmpServerSchema.Subject.SubjectMailNameEnd;
            rbSubjectMailNameReplace.IsChecked = tmpServerSchema.Subject.SubjectMailNameReplace;

            rbNoneSSL.IsChecked = tmpServerSchema.NoSSL;
            rbTLS.IsChecked = tmpServerSchema.UseTLS;
            rbSMTPS.IsChecked = tmpServerSchema.UseSSLSecuredPort;


            //Parsing mit Lumisoft
            if (!tmpServerSchema.SendOriginal && tmpServerSchema.Parsed)
            {
                rbOriginalMessage.IsChecked = false;
                //rbDeleteMessageID.IsChecked = false;
                //rbNewMessageID.IsChecked = true;
                rbSendOriginal.IsChecked = true;
            }

            if (tmpServerSchema.SendOriginal && !tmpServerSchema.DeleteMessageID && !tmpServerSchema.Parsed)
            {
                rbOriginalMessage.IsChecked = true;
                //rbDeleteMessageID.IsChecked = false;
                rbSendOriginal.IsChecked = false;
                //rbOriginalMessageID.IsChecked = true;
            }
            if (tmpServerSchema.SendOriginal && tmpServerSchema.DeleteMessageID && !tmpServerSchema.Parsed)
            {
                //rbDeleteMessageID.IsChecked = true;
                rbParseHeader.IsChecked = true;
                rbSendOriginal.IsChecked = false;
            }
            initial = false;
        }
        private void SetMailingMethod(ServerSchema tmpServerSchema)
        {
            if (!initial)
            {
                if ((bool)rbSendOriginal.IsChecked)
                {

                    //Parsing und FallBack
                    tmpServerSchema.SendOriginal = false;
                    tmpServerSchema.NewMessageID = true;
                    tmpServerSchema.Parsed = true;
                    //rbNewMessageID.IsChecked = true;
                }
                if ((bool)rbParseHeader.IsChecked)
                {
                    tmpServerSchema.SendOriginal = true;
                    tmpServerSchema.DeleteMessageID = true;
                    tmpServerSchema.Parsed = false;
                    //rbDeleteMessageID.IsChecked = true;
                }
                if ((bool)rbOriginalMessage.IsChecked)
                {
                    tmpServerSchema.SendOriginal = true;
                    tmpServerSchema.DeleteMessageID = false;
                    tmpServerSchema.Parsed = false;
                    tmpServerSchema.OriginalMessageID = true;
                    tmpServerSchema.NewMessageID = false;
                    //rbOriginalMessageID.IsChecked = true;
                }
            }
        }

        private void optionTabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //TabControl tabControl = (TabControl)sender;
            //ServerSchema tmpServerSchema = (ServerSchema)tabControl.DataContext;

        }

        private void rbBasic_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton button = (RadioButton)sender;
            ServerSchema myServerSchema = (ServerSchema)button.DataContext;
            myServerSchema.AnonymousAuth = false;
            myServerSchema.BasicAuth = true;
        }

        private void rbSubjectNone_Click(object sender, RoutedEventArgs e)
        {
            RadioButton button = (RadioButton)sender;
            ServerSchema tmpServerSchema = (ServerSchema)button.DataContext;

            tmpServerSchema.Subject.SubjectNone = true;
            tmpServerSchema.Subject.SubjectExtentAtBegin = false;
            tmpServerSchema.Subject.SubjectExtentAtEnd = false;
        }

        private void rbSubjectBegin_Click(object sender, RoutedEventArgs e)
        {
            RadioButton button = (RadioButton)sender;
            ServerSchema tmpServerSchema = (ServerSchema)button.DataContext;

            tmpServerSchema.Subject.SubjectNone = false;
            tmpServerSchema.Subject.SubjectExtentAtBegin = true;
            tmpServerSchema.Subject.SubjectExtentAtEnd = false;

        }

        private void rbSubjectEnd_Click(object sender, RoutedEventArgs e)
        {
            RadioButton button = (RadioButton)sender;
            ServerSchema tmpServerSchema = (ServerSchema)button.DataContext;

            tmpServerSchema.Subject.SubjectNone = false;
            tmpServerSchema.Subject.SubjectExtentAtBegin = false;
            tmpServerSchema.Subject.SubjectExtentAtEnd = true;

        }

        private void txtRecipientGroupStart_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (recipientGroupBox.BindingGroup.CommitEdit())
            {
                e.Cancel = false;
            }
            else
            {
                e.Cancel = true;
            }

        }

        private void rbSubjectMailNameBegin_Click(object sender, RoutedEventArgs e)
        {
            RadioButton button = (RadioButton)sender;
            ServerSchema tmpServerSchema = (ServerSchema)button.DataContext;
            tmpServerSchema.Subject.SubjectMailNameBegin = true;
            tmpServerSchema.Subject.SubjectMailNameEnd = false;
            tmpServerSchema.Subject.SubjectMailNameReplace = false;
        }

        private void rbSubjectMailNameEnd_Click(object sender, RoutedEventArgs e)
        {
            RadioButton button = (RadioButton)sender;
            ServerSchema tmpServerSchema = (ServerSchema)button.DataContext;
            tmpServerSchema.Subject.SubjectMailNameBegin = false;
            tmpServerSchema.Subject.SubjectMailNameEnd = true;
            tmpServerSchema.Subject.SubjectMailNameReplace = false;

        }

        private void rbSubjectMailNameReplace_Click(object sender, RoutedEventArgs e)
        {
            RadioButton button = (RadioButton)sender;
            ServerSchema tmpServerSchema = (ServerSchema)button.DataContext;
            tmpServerSchema.Subject.SubjectMailNameBegin = false;
            tmpServerSchema.Subject.SubjectMailNameEnd = false;
            tmpServerSchema.Subject.SubjectMailNameReplace = true;

        }

        private void chkLogging_Click(object sender, RoutedEventArgs e)
        {
            CheckBox button = (CheckBox)sender;
            ServerSchema tmpServerSchema = (ServerSchema)button.DataContext;
            if (button.IsChecked == true)
            {
                Log myLog = new Log();
                //log4net.Config.XmlConfigurator.Configure(); 
                //log4net.Config.XmlConfigurator.Configure(
            }
            else
            {
                Log.logger.Logger.Repository.ResetConfiguration();
                log4net.LogManager.Shutdown();
                //log4net.Config.


            }
        }

        private void rbNoneSSL_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton button = (RadioButton)sender;
            ServerSchema tmpServerSchema = (ServerSchema)button.DataContext;
            tmpServerSchema.NoSSL = true;
            tmpServerSchema.UseTLS = false;
            tmpServerSchema.UseSSLSecuredPort = false;

        }

        private void rbTLS_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton button = (RadioButton)sender;
            ServerSchema tmpServerSchema = (ServerSchema)button.DataContext;
            tmpServerSchema.NoSSL = false;
            tmpServerSchema.UseTLS = true;
            tmpServerSchema.UseSSLSecuredPort = false;

        }

        private void rbSMTPS_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton button = (RadioButton)sender;
            ServerSchema tmpServerSchema = (ServerSchema)button.DataContext;
            tmpServerSchema.NoSSL = false;
            tmpServerSchema.UseTLS = false;
            tmpServerSchema.UseSSLSecuredPort = true;

        }

        private void optionDialogClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }





    }
}
