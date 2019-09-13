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
using System.IO;
using System.Collections;
using System.Diagnostics;
using MailSend;
using MailSendWPF.Configuration;


namespace MailSendWPF.UserControls
{
    /// <summary>
    /// Interaction logic for MailsPanel.xaml
    /// </summary>
    public partial class MailsPanel : UserControl
    {
        public MailsPanel()
        {
            InitializeComponent();

        }
        public static readonly DependencyProperty IdProperty = DependencyProperty.Register("Id", typeof(string), typeof(MailsPanel));
        private String id = String.Empty;

        public String Id
        {
            get { return (string)GetValue(IdProperty); }
            set { SetValue(IdProperty, value); }
        }
        private bool m_onceLoaded = false;
        private ulong CountMails(string[] filesDirs)
        {
            ulong mailCount = 0;
            ServerSchema myServerSchema = (ServerSchema)this.DataContext;
            foreach (string fileName in filesDirs)
            {
                if (File.Exists(fileName))
                {
                    mailCount++;
                }
                else if (Directory.Exists(fileName))
                {
                    if ((bool)chkRecursive.IsChecked)
                    {
                        //mySearchOption = SearchOption.AllDirectories;
                        long files = Directory.GetFiles(fileName, myServerSchema.Filter, SearchOption.AllDirectories).LongLength;
                        mailCount = mailCount + (ulong)files;
                    }
                    else
                    {
                        long files = Directory.GetFiles(fileName, myServerSchema.Filter, SearchOption.TopDirectoryOnly).LongLength;
                        mailCount = mailCount + (ulong)files;
                    }
                }
            }
            return mailCount;            
        }

        private void addFile_Click(object sender, RoutedEventArgs e)
        {
            //ToDo: Fehler muss über sender gehen sonst kann es dem MailServer nciht zugeordnet werden
            Button dialogButton = null;
            if (sender is Button)
            {
                dialogButton = (Button)sender;
            }
            ServerSchema myServerSchema = (ServerSchema)dialogButton.DataContext;//this.DataContext;
            var fileDialog = new System.Windows.Forms.OpenFileDialog();
            fileDialog.InitialDirectory = myServerSchema.MailFilePath;
            fileDialog.Multiselect = true;
            fileDialog.CheckFileExists = true;
            fileDialog.CheckPathExists = true;
            fileDialog.Filter = myServerSchema.Filter + "|" + myServerSchema.Filter;
            System.Windows.Forms.DialogResult result = fileDialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                //DirectoryInfo dir = new DirectoryInfo(fileDialog.FileName);
                //dir.
                myServerSchema.MailFilePath = fileDialog.FileName;
                String[] fileNames = fileDialog.FileNames;
                //ServerSchema serv = (ServerSchema)this.DataContext;
                foreach (String filename in fileNames)
                {
                    myServerSchema.MailDir.Add(filename);
                }
                ulong allMails = UInt64.Parse(txtValMails.Text);
                myServerSchema.MailsCount = (allMails + (ulong)fileNames.Length);
                lstMails.GetBindingExpression(ListBox.ItemsSourceProperty).UpdateTarget();
            }
        }

        private void addFolder_Click(object sender, RoutedEventArgs e)
        {
            ServerSchema myServerSchema = null;
            if (sender is Button)
            {
                Button folderButton = (Button)sender;
                myServerSchema = (ServerSchema) folderButton.DataContext;
            }
            var folderDialog = new System.Windows.Forms.FolderBrowserDialog();
            folderDialog.SelectedPath = myServerSchema.MailFolderPath;
            folderDialog.Description = "Please choose Mail Directory";
            System.Windows.Forms.DialogResult result = folderDialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                String fileName = folderDialog.SelectedPath;
                //myServerSchema = (ServerSchema)this.DataContext;
                myServerSchema.MailFolderPath = fileName;
                if (Directory.Exists(fileName))
                {
                    if ((bool)chkRecursive.IsChecked)
                    {
                        long files = -1;
                        try
                        {
                            files = Directory.GetFiles(fileName, myServerSchema.Filter, SearchOption.AllDirectories).LongLength;
                        }
                        catch (Exception ex)
                        {
                        }
                        if (files > 0)
                        {
                            ulong allMails = UInt64.Parse(txtValMails.Text);
                            myServerSchema.MailDir.Add(fileName);
                            myServerSchema.MailsCount = (allMails + (ulong)files);
                        }
                    }
                    else
                    {
                        long files = Directory.GetFiles(fileName, myServerSchema.Filter, SearchOption.TopDirectoryOnly).LongLength;
                        if (files > 0)
                        {
                            ulong allMails = UInt64.Parse(txtValMails.Text);
                            myServerSchema.MailDir.Add(fileName);
                            myServerSchema.MailsCount = (allMails + (ulong)files);
                        }
                    }
                }

                lstMails.GetBindingExpression(ListBox.ItemsSourceProperty).UpdateTarget();
            }
        }

        private void lstMails_Drop(object sender, DragEventArgs e)
        {
            // Dateien aus den gezogenen Daten auslesen
            ServerSchema myServerSchema = (ServerSchema)lstMails.DataContext;
            SearchOption mySearchOption = SearchOption.TopDirectoryOnly;
            string[] filesNames = (string[])e.Data.GetData(DataFormats.FileDrop,
               false);
            foreach (string fileName in filesNames)
            {
                // FileInfo-Objekt erzeugen
                FileInfo fi = new FileInfo(fileName);
                if (fi.Exists)
                {

                    myServerSchema.MailDir.Add(fileName);
                    ulong allMails = UInt64.Parse(txtValMails.Text);
                    myServerSchema.MailsCount = (allMails + 1);

                    // Wenn es sich nicht um einen Ordner handelt: FileInfo-Objekt der
                    // Liste anfügen
                    //this.fileListBox.Items.Add(fi);
                }
                else if (Directory.Exists(fileName))
                {
                    //Directory


                    if ((bool)chkRecursive.IsChecked)
                    {
                        mySearchOption = SearchOption.AllDirectories;
                        long files = -1;
                        try
                        {
                            files = Directory.GetFiles(fileName, myServerSchema.Filter, mySearchOption).LongLength;
                        }
                        catch (Exception ex)
                        {
                        }
                        if (files > 0)
                        {
                            ulong allMails = UInt64.Parse(txtValMails.Text);
                            myServerSchema.MailDir.Add(fileName);
                            myServerSchema.MailsCount = (allMails + (ulong)files);
                        }
                    }
                    else
                    {
                        long files = Directory.GetFiles(fileName, myServerSchema.Filter, mySearchOption).LongLength;
                        if (files > 0)
                        {
                            ulong allMails = UInt64.Parse(txtValMails.Text);
                            myServerSchema.MailDir.Add(fileName);
                            myServerSchema.MailsCount = (allMails + (ulong)files);
                        }
                    }
                }
            }
            lstMails.GetBindingExpression(ListBox.ItemsSourceProperty).UpdateTarget();

        }

        private void lstMails_DragEnter(object sender, DragEventArgs e)
        {
            // Überprüfen, ob Dateien oder Ordner gezogen werden
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effects = DragDropEffects.Copy;
            }
            else
            {
                e.Effects = DragDropEffects.Copy;
            }

        }

        private void lstMails_KeyDown(object sender, KeyEventArgs e)
        {
            
            if (e.Key == Key.Delete)
            {
                ListBox listBox = (ListBox)sender;
                ServerSchema serverSchema = (ServerSchema)listBox.DataContext;
                IList selList = listBox.SelectedItems;
                StringItem[] fileIndex = new StringItem[selList.Count];
                selList.CopyTo(fileIndex, 0);
                foreach (StringItem file in fileIndex)
                {
                    serverSchema.MailDir.Remove(file.Item);
                    serverSchema = (ServerSchema)listBox.DataContext;
                }

               serverSchema.MailsCount = CountMails(serverSchema.MailDir.ToArray<String>());
               lstMails.GetBindingExpression(ListBox.ItemsSourceProperty).UpdateTarget();
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (!m_onceLoaded)
            {
                /*
                ServerSchema myServerSchema = (ServerSchema)this.DataContext;
                ulong allMails = UInt64.Parse(txtValMails.Text);
                allMails = allMails + CountMails(myServerSchema.MailDir.ToArray<String>());
                txtValMails.Text = (allMails).ToString();
                e.Handled = true;
                m_onceLoaded = true;
                 */ 
            }
        }

        private void UserControl_Initialized(object sender, EventArgs e)
        {
         
        }

        private void chkRecursive_Click(object sender, RoutedEventArgs e)
        {
            ServerSchema serverSchema = (ServerSchema)this.DataContext;
            serverSchema.MailsCount = CountMails(serverSchema.MailDir.ToArray<String>());
        }

        private void lstMails_SourceUpdated(object sender, DataTransferEventArgs e)
        {

        }

        private void lstMails_TargetUpdated(object sender, DataTransferEventArgs e)
        {
            /*
            ServerSchema mySchema = (ServerSchema) this.DataContext;
            lstMails.DataContext = this.DataContext;
            Binding binding = new Binding("MailDir");
            binding.Converter = new StringConverter();
            binding.Source = mySchema;
            lstMails.SetBinding(ItemsControl.ItemsSourceProperty, binding);
            e.Handled = true;
            */
        }

        private void lstMails_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ListBox listBox = (ListBox)sender;
            StringItem item = (StringItem) listBox.SelectedItem;
            if (item != null)
            {
                ServerSchema schema = (ServerSchema)listBox.DataContext;
                string fileName = schema.ExternalProgram;

                if (File.Exists(item.Item) && !String.IsNullOrEmpty(fileName))
                {
                    fileName = fileName.Trim();
                    /*
                    if (fileName.Contains(Tools.sExternalProgVar))
                    {
                        fileName.Replace(Tools.sExternalProgVar, item.Item);
                    }
                    else
                    {
                        fileName = fileName.TrimEnd(' ');
                        fileName = fileName + " " + item.Item.Trim();
                    }
                   */

                    Process p = new Process();

                    p.StartInfo.FileName = fileName;
                    p.StartInfo.Arguments = item.Item.Trim();


                    try
                    {
                        p.Start();
                    }
                    catch (FileNotFoundException ex)
                    {
                        MessageBox.Show("File not found: " + p.StartInfo.FileName, "File not found", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message + " " + p.StartInfo.FileName, "Unknown Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }
    }
}
