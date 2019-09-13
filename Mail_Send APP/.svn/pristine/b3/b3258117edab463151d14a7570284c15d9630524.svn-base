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
using MailSendWPF.UserControls;
using MailSend;
using MailSendWPF.Configuration;

namespace MailSendWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Servers m_Servers = null;
        private App m_Application = null;
        public MainWindow()
        {
            Application.Current.Resources["SAVE"] = false;
            InitializeComponent();
            
        }
        public MainWindow(Servers servs, App app)
        {
            Application.Current.Resources["SAVE"] = false;
            InitializeComponent();
            m_Servers = servs;
            m_Application = app;
        }
        public ServerControl GetServerControl(string key)
        {
            for (int i = 0; i < this.stackServers.Children.Count; i++)
            {
                if (this.stackServers.Children[i] is ServerControl)
                {
                    ServerControl tempServerControl = (ServerControl)this.stackServers.Children[i];
                    if (tempServerControl.Id.Equals(key))
                    {
                        return tempServerControl;
                    }
                }
            }
            return null;
        }

        private void btnAddServer_Click(object sender, RoutedEventArgs e)
        {

            ServerControl servCtrl = new ServerControl();
            Separator sep = new Separator();
            if (m_Servers.OneServer)
            {
                sep.Visibility = System.Windows.Visibility.Collapsed;
            }
            sep.Width = servCtrl.Width;
            sep.Height = 10;
            sep.BorderThickness = new Thickness(10, 10, 10, 10);
            sep.BorderBrush = Brushes.Black;
            this.stackServers.Children.Add(sep);
            ServerSchema newServerSchema = new ServerSchema();
            m_Servers.ServerSchema.Add(newServerSchema);
            servCtrl.DataContext = newServerSchema;
            servCtrl.Visibility = Visibility.Visible;
            this.stackServers.Children.Add(servCtrl);
            newServerSchema.Visible = true;
            newServerSchema.PropertyChanged += m_Application.Model_PropertyChanged;
            cmbChooseServer.SelectedItem = newServerSchema;

            if (m_Servers.OneServer)
            {

                for (int i = 0; i < this.stackServers.Children.Count; i++)
                {
                    
                    
                    if (this.stackServers.Children[i] is ServerControl)
                    {
                        ServerControl serverControl = (ServerControl)this.stackServers.Children[i];
                        if (!serverControl.Equals(servCtrl))
                        {
                            this.stackServers.Children[i].Visibility = System.Windows.Visibility.Collapsed;
                        }
                        ServerSchema serverSchema = (ServerSchema)serverControl.DataContext;
                        if (!serverSchema.Equals(newServerSchema))
                        {
                            serverSchema.Visible = false;
                        }
                    }
                    else if (this.stackServers.Children[i] is Separator)
                    {
                        Separator tempSeparator = (Separator)this.stackServers.Children[i];
                        if (!tempSeparator.Equals(sep))
                        {
                            tempSeparator.Visibility = System.Windows.Visibility.Collapsed;
                        }
                    }
                }
            }
            else
            {
                ShowServerChooser(false);
                m_Servers.OneServer = false;
                for (int i = 0; i < this.stackServers.Children.Count; i++)
                {
                    if (this.stackServers.Children[i] is ServerControl)
                    {
                        ServerControl tempServerControl = (ServerControl)this.stackServers.Children[i];
                        if (tempServerControl.Visibility == System.Windows.Visibility.Collapsed)
                        {
                            tempServerControl.Visibility = System.Windows.Visibility.Visible;
                        }
                    }
                    else if (this.stackServers.Children[i] is Separator)
                    {
                        Separator tempSeparator = (Separator)this.stackServers.Children[i];
                        if (tempSeparator.Visibility == System.Windows.Visibility.Collapsed)
                        {
                            tempSeparator.Visibility = System.Windows.Visibility.Visible;
                        }
                    }
                    /*
                    if (this.stackServers.Children[i] is ServerControl)
                    {
                        ServerControl serverControl = (ServerControl)this.stackServers.Children[i];
                        ServerSchema serverSchema = (ServerSchema)serverControl.DataContext;
                        serverSchema.Visible = false;
                    }
                     */
                }
            }
            /*
            ServerSchema servSchema = (ServerSchema) this.DataContext;
            Console.Out.WriteLine(servSchema.MailFrom);
            Console.Out.WriteLine("Width " + stackServers.DesiredSize.Width);
            ServerControl stackPanel = (ServerControl)stackServers.Children[0];
            StackPanel sp = (StackPanel)stackPanel.FindName("outerStackPanel");
            Console.Out.WriteLine("Width OuterStackpanel: " + sp.DesiredSize.Width.ToString());
            if (m_Servers != null)
            {
                m_Servers.SaveToFile("Servers2.xml");
            }

            ListBox mailsListBox = (ListBox)stackPanel.FindName("lstMails");
            //mailsListBox.It

            Window1 wnd1 = new Window1();
            wnd1.InitializeComponent();
            wnd1.ShowDialog();
            */
        }

        private void save_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem)
            {

                FocusManager.SetFocusedElement(this, (MenuItem)sender);
            }
            if (sender is Button)
            {
                FocusManager.SetFocusedElement(this, (Button)sender);
            }

            bool focus = Application.Current.MainWindow.Focus();
            string changedString = Constants.sChangedString;//" *CHANGED*";
            string saveFilePath = (string)Application.Current.Resources["CONFIGFILE"];
            m_Servers.SaveToFile(saveFilePath);

            Application.Current.MainWindow.Title = (string)Application.Current.Resources["DISPLAYNAME"] + " " + Constants.sVersionString;//Application.Current.MainWindow.Title.Replace(changedString, "");
            Application.Current.Resources["SAVE"] = false;
            FocusManager.SetFocusedElement((ScrollViewer)mainWindow.scrollServers, (ScrollViewer)mainWindow.scrollServers);
            mainWindow.Focus();
            ((ScrollViewer)mainWindow.scrollServers).Focus();
            //FocusManager.

        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            FocusManager.SetFocusedElement(this, (MenuItem)sender);
            //FocusManager.SetFocusedElement((ScrollViewer)mainWindow.scrollServers, (ScrollViewer)mainWindow.scrollServers);
        }

        private void mnuItemHilfe_Click(object sender, RoutedEventArgs e)
        {
            //FocusManager.SetFocusedElement(this, (MenuItem)sender);
            //FocusManager.SetFocusedElement((ScrollViewer)mainWindow.scrollServers, (ScrollViewer)mainWindow.scrollServers);
        }

        private void mnuItemFile_Click(object sender, RoutedEventArgs e)
        {
            //FocusManager.SetFocusedElement(this, (MenuItem)sender);
            //FocusManager.SetFocusedElement((ScrollViewer)mainWindow.scrollServers, (ScrollViewer)mainWindow.scrollServers);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            FocusManager.SetFocusedElement(this, (Window)sender);
            string changedString = " *CHANGED*";
            bool save = (bool)Application.Current.Resources["SAVE"];
            if (save)
            {
                WPF_Dialogs.Dialogs.SaveDialog s = new WPF_Dialogs.Dialogs.SaveDialog();

                WPF_Dialogs.EDialogResult result = s.showDialog();
                if (result == WPF_Dialogs.EDialogResult.Yes)
                {
                    string saveFilePath = (string)Application.Current.Resources["CONFIGFILE"];
                    m_Servers.SaveToFile(saveFilePath);
                }
                else if (result == WPF_Dialogs.EDialogResult.Cancel)
                {
                    e.Cancel = true;
                    Application.Current.MainWindow.Title = Application.Current.MainWindow.Title.Replace(changedString, "");

                }
            }
        }

        private void mnuItemEnd_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.Close();
        }

        private void btnList_Click(object sender, RoutedEventArgs e)
        {
            ShowServerChooser(false);
            m_Servers.OneServer = false;

            //this.stackServers
            for (int i = 0; i < this.stackServers.Children.Count; i++)
            {
                if (this.stackServers.Children[i] is ServerControl)
                {
                    ServerControl tempServerControl = (ServerControl)this.stackServers.Children[i];
                    if (tempServerControl.Visibility == System.Windows.Visibility.Collapsed)
                    {
                        tempServerControl.Visibility = System.Windows.Visibility.Visible;
                    }
                }
            }

        }

        public void ShowServerChooser(bool show)
        {

            Binding fromBinding = new Binding();
            fromBinding.Source = m_Servers.ServerSchema;
            //fromBinding.Path = new PropertyPath("SmartHost");
            fromBinding.Mode = BindingMode.OneWay;
            cmbChooseServer.SetBinding(ComboBox.ItemsSourceProperty, fromBinding);
            if (show)
            {
                //mainWindow.Height = 390;
                //scrollServers.Height = 340;
                //mainWindow.Height = 420;
                mainWindow.Height = 600;
                //scrollServers.Height = 370;
                scrollServers.Height = 400;
                cmbChooseServer.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                mainWindow.Height = 730;
                scrollServers.Height = 680;
                cmbChooseServer.Visibility = System.Windows.Visibility.Collapsed;
            }

        }

        private void btnDropDown_Click(object sender, RoutedEventArgs e)
        {
            ShowServerChooser(true);
            m_Servers.OneServer = true;
            //int visible = -1;
            for (int i = 0; i < this.stackServers.Children.Count; i++)
            {
                if (this.stackServers.Children[i] is ServerControl)
                {
                    ServerControl tempServerControl = (ServerControl)this.stackServers.Children[i];
                    ServerSchema serverSchema = (ServerSchema)tempServerControl.DataContext;
                    if (serverSchema.Visible)
                    {
                        //visible = i;
                        tempServerControl.Visibility = System.Windows.Visibility.Visible;
                        cmbChooseServer.SelectedItem = serverSchema;
                        Application.Current.Resources["DISPLAYNAME"] = serverSchema.DisplayName;
                       
                    }
                    else
                    {
                        tempServerControl.Visibility = System.Windows.Visibility.Collapsed;
                    }
                    /*
                    if (tempServerControl.serverExPander.IsExpanded)
                    {
                        
                    }
                    else
                    {
                        tempServerControl.Visibility = System.Windows.Visibility.Collapsed;
                    }
                    
                    if (tempServerControl.Visibility == System.Windows.Visibility.Collapsed)
                    {
                        tempServerControl.Visibility = System.Windows.Visibility.Visible;
                    }
                     */
                }
                else if (this.stackServers.Children[i] is Separator)
                {
                    Separator sep = (Separator)this.stackServers.Children[i];
                    sep.Visibility = System.Windows.Visibility.Collapsed;
                }
            }
        }

        public void SetVisibleServer(ServerSchema server)
        {
            for (int i = 0; i < this.stackServers.Children.Count; i++)
            {
                if (this.stackServers.Children[i] is ServerControl)
                {
                    ServerControl tempServerControl = (ServerControl)this.stackServers.Children[i];
                    ServerSchema serverSchema = (ServerSchema)tempServerControl.DataContext;
                    if (serverSchema.Equals(server))
                    {
                        tempServerControl.Visibility = System.Windows.Visibility.Visible;
                        serverSchema.Visible = true;
                        //Application.Current.Resources["SAVE"] = false;
                        bool saved = (bool)Application.Current.Resources["SAVE"];
                        if (saved)
                        {
                            Application.Current.MainWindow.Title = serverSchema.DisplayName + " " + Constants.sVersionString;
                        }
                        else
                        {
                            Application.Current.MainWindow.Title = serverSchema.DisplayName + " " + Constants.sVersionString + Constants.sChangedString;
                        }
                        //cmbChooseServer.SelectedItem = serverSchema;
                    }
                    else
                    {
                        tempServerControl.Visibility = System.Windows.Visibility.Collapsed;
                        serverSchema.Visible = false;
                    }

                }
                else if (this.stackServers.Children[i] is Separator)
                {
                    Separator sep = (Separator)this.stackServers.Children[i];
                    sep.Visibility = System.Windows.Visibility.Collapsed;
                }
            }
        }

        private void cmbChooseServer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            ServerSchema selectedServer = (ServerSchema)comboBox.SelectedItem;
            SetVisibleServer(selectedServer);
        }

       

        private void btnDeleteServer_Click(object sender, RoutedEventArgs e)
        {
            ServerSchema myServerSchema = (ServerSchema)cmbChooseServer.SelectedItem;

            if (m_Servers.ServerSchema.Count > 1)
            {
                for (int i = 0; i < this.stackServers.Children.Count; i++)
                {
                    if (this.stackServers.Children[i] is ServerControl)
                    {
                        ServerControl myServerControl = (ServerControl)this.stackServers.Children[i];
                        ServerSchema compServerSchema = (ServerSchema)myServerControl.DataContext;
                        if (myServerSchema.Equals(compServerSchema))
                        {
                            if (i - 1 >= 0)
                            {
                                if (this.stackServers.Children[i - 1] is Separator)
                                {
                                    this.stackServers.Children.RemoveAt(i - 1);
                                }
                            }
                            if (this.stackServers.Children[i-1] is ServerControl)
                            {
                                this.stackServers.Children.RemoveAt(i-1);
                            }
                        }
                    }
                }
                m_Servers.ServerSchema.Remove(myServerSchema);
                cmbChooseServer.SelectedItem = m_Servers.ServerSchema[0];
            }
            
            /*
            ServerControl servCtrl = new ServerControl();
            Separator sep = new Separator();
            if (m_Servers.OneServer)
            {
                sep.Visibility = System.Windows.Visibility.Collapsed;
            }
            sep.Width = servCtrl.Width;
            sep.Height = 10;
            sep.BorderThickness = new Thickness(10, 10, 10, 10);
            sep.BorderBrush = Brushes.Black;
            this.stackServers.Children.Add(sep);
            ServerSchema newServerSchema = new ServerSchema();
            m_Servers.ServerSchema.Add(newServerSchema);
            servCtrl.DataContext = newServerSchema;
            servCtrl.Visibility = Visibility.Visible;
            this.stackServers.Children.Add(servCtrl);
            newServerSchema.Visible = true;
            newServerSchema.PropertyChanged += m_Application.Model_PropertyChanged;
            cmbChooseServer.SelectedItem = newServerSchema;
            */
        }
    }
}
