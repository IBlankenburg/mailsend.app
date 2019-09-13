using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Reflection;
using MailSendWPF.Interfaces;


namespace MailSendWPF.Server
{
    /*
     * RegistryKey OurKey = Registry.CurrentUser;
 OurKey = OurKey.OpenSubKey(@"SOFTWARE\Microsoft\Office\");
 foreach (string Keyname in OurKey.GetSubKeyNames())
 {
     if (Keyname == "11.0")
     {
         foundOffice = true;
         break;
     }
     else if (Keyname == "12.0")
     {
         foundOffice = true;
         break;
     }
 }
     */
    public class LoadServers
    {
        [ImportMany(typeof(IServer), AllowRecomposition = true)]
        public List<IServer> IServerList { get; set; }
        DirectoryCatalog _catalog = null;
        string serverPath = string.Empty;
        public LoadServers()
        {
            serverPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Server");
        }
    }
}
