using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using MailSendWPF.Configuration;

namespace MailSend
{
    public class Constants
    {
        public struct KeyManager
        {
            public bool KeyManagerUse;
            public bool KeyManagerTest;
            public ObservableCollection<ServerSchemaTenants> Tenants;
            public bool ImportX509CertificateRequest;
            public string ImportFromTenant;
            public bool Get509CertificateRequest;
            public bool GetAllPrivateKeysForEmailRequest;
            public bool GetAllX509CertificatesForEmailRequest;
            public bool GetOrCreatePrivateKeyAdvancedRequest;
            public bool GetOrCreatePrivateKeyRequest;
            public bool GetX509CertificateForVerificationBySignedDataRequest;
            public bool GetPrivateKeyFromEnvelopeRequest;
            public bool GetPrivateKeyRequest;
            public bool MakeCyclicCertsForAllTenantsSame;
            public string publicHost;
            public string privateHost;
            public bool useHttps;
            public string PublicKeyType;
            public string PrivateKeyType;
            public ulong CountRequests;

        };
        public enum LineBreak { Windows, Linux, OldMac };
        public static string sMessageID = "Message-ID";
        public static string sReplaceForMessageID = "xxxxxxxxxx";

        public static string sTab = "\t";
        public static string sLineBreakWindows = "\r\n";
        public static string sLineBreakLinuxMac = "\n";
        public static string sLineBreakOldMac = "\r";

        public static string sSpace = " ";

        public static string sMailFrom = "From:";
        public static string sMailTo = "To:";

        public static string sMailEncoding7bit = "7bit";
        public static string sMailEncoding8bit = "8bit";
        public static string sMailEncodingBinary = "binary";
        public static string sMailEncodingBase64 = "base64";
        public static string sMailEncodingQuotedPrintable = "quoted-printable";

        public static string sSMTPExtensionBinaryMIME = "BINARYMIME";
        public static string sSMTPExtension8BitMIME = "8BITMIME";
        public static string sSMTPExtensionCHUNKING = "CHUNKING";
        public static string sSMTPExtensionEHLO = "EHLO";

        public static string sSMTPExtensionCommandBDAT = "BDAT";

        public static string sExternalProgVar = "[prog]";
        public static string sMailWillBeGenerated = "Mail will be generated";
        public static string sChangedString = " *CHANGED*";
        public static string sVersionString = "MailSend V0.99beta4";

        public static List<t> RemoveDoubleItems<t>(List<t> list)
        {
            List<t> newList = new List<t>();
            Dictionary<t, string> keyList = new Dictionary<t, string>();

            foreach (t item in list)
            {
                if (!keyList.ContainsKey(item))
                {
                    keyList.Add(item, string.Empty);
                    newList.Add(item);
                }
            }

            return newList;
        } 

    }
}
