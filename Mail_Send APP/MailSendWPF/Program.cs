using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using StartUp;
using System.IO;
using System.Configuration;

namespace MailSend
{
    class Program
    {

        static void Main(string[] args)
        {
            CommandLine cmd = new CommandLine(args, true);


            // string testString = "hhh: ggg    " + Environment.NewLine + " hjhkjghujgzug " + Environment.NewLine + Environment.NewLine;
            //StringHelper.StringHelper.ChangeHeaderAttribute(testString, "hhh", "iii          ");
            //string testString = "hallo: fff \r\n\tfff\r\nmessage-id:" + Environment.NewLine + " hjhkjghujgzug ";
            //StringHelper.StringHelper.RemoveMessageID(testString);
            /*
            string testString2 = "hhh: ggg   ";
            StringHelper.StringHelper.ChangeHeaderAttribute(testString2, "hhh", "iii");
             */
            /*
            string testString3 = "";
            StringHelper.StringHelper.ChangeHeaderAttribute(testString3, "hhh", "iii");
             */
            //int pos = testString.IndexOf("h", 2, StringComparison.OrdinalIgnoreCase);

            /*
            FileStream fs = File.OpenRead("e-mail2.eml");
            StreamReader reader = new StreamReader(fs);
            String sMailMessage = reader.ReadToEnd();
            string headerString = StringHelper.StringHelper.GetHeaderDeleteBody(ref sMailMessage);
            string headerWithoutMessageID = StringHelper.StringHelper.RemoveUntilEndOfLine(Tools.sMessageID, headerString);
             */
            //headerString.Remove(

            /*
             * -hostip, -port, -mailto, -mailfrom, -user, -pwd, -maildir, -filter, -recursive, -rounds, -connections, -endlessSending, -recursive  
             * 
             */
            if (!cmd.Parameters.ContainsKey("hostip") || !cmd.Parameters.ContainsKey("port") || !cmd.Parameters.ContainsKey("mailto") ||
                !cmd.Parameters.ContainsKey("mailfrom") || !cmd.Parameters.ContainsKey("user") || !cmd.Parameters.ContainsKey("pwd") ||
                !cmd.Parameters.ContainsKey("maildir") || !cmd.Parameters.ContainsKey("filter") || !cmd.Parameters.ContainsKey("recursive"))
            //!cmd.Parameters.ContainsKey("hostip") || !cmd.Parameters.ContainsKey("hostip") || !cmd.Parameters.ContainsKey("hostip") || 
            //!cmd.Parameters.ContainsKey("hostip") || !cmd.Parameters.ContainsKey("hostip") || !cmd.Parameters.ContainsKey("hostip") || )
            {
                Console.WriteLine("-hostip=<IP of smarthost> -port=<Port> -mailto=<Mail receiver> -mailfrom=<mail sender> -user=<Username> -pwd=<password> -maildir=<absolute path To MailMessages> -filter=<filter e.g. *.eml> -connections=<0...x> -endlessSending=<true/false> -recursive=<true/false> -subject=<string> -headerKey=<string> -headerValue=<string> -parsed=<true/false> -waitBetweenMailsms=<int>");
            }

            Log myLog = new Log();
            //Server.ParseMailMessage();

            string mailTo = @"testuser1@jh1.local";
            string mailFrom = @"test@test.local";
            IPAddress smartHost = IPAddress.Parse("172.30.200.139");
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

            mailTo = ConfigurationSettings.AppSettings["mailto"];
            mailFrom = ConfigurationSettings.AppSettings["mailfrom"];
            port = Int32.Parse(ConfigurationSettings.AppSettings["port"]);
            user = ConfigurationSettings.AppSettings["user"];
            pwd = ConfigurationSettings.AppSettings["pwd"];
            dir = ConfigurationSettings.AppSettings["maildir"];
            filter = ConfigurationSettings.AppSettings["filter"];
            rounds = UInt64.Parse(ConfigurationSettings.AppSettings["rounds"]);
            connections = Int32.Parse(ConfigurationSettings.AppSettings["connections"]);
            endlessSending = Boolean.Parse(ConfigurationSettings.AppSettings["endlessSending"]);
            recursive = Boolean.Parse(ConfigurationSettings.AppSettings["recursive"]);
            parsed = Boolean.Parse(ConfigurationSettings.AppSettings["parsed"]);
            waitBetweenMailsms = Int32.Parse(ConfigurationSettings.AppSettings["waitBetweenMails"]);
            sendOriginal = Boolean.Parse(ConfigurationSettings.AppSettings["sendOriginal"]);
            newMessageID = Boolean.Parse(ConfigurationSettings.AppSettings["newMessageID"]);
            fallBack = Boolean.Parse(ConfigurationSettings.AppSettings["fallback"]);

            subject = ConfigurationSettings.AppSettings["subject"];
            mailAttributes = CreateMailAttributes(mailAttributes);
            MailSend.MailAttributes.SSubject mailSubject = new MailAttributes.SSubject();
            mailSubject.position = MailAttributes.SubjectPosition.end;
            mailSubject.subjectstring = subject;
            mailAttributes.Subject = mailSubject;



            string tempHost = ConfigurationSettings.AppSettings["hostip"];
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
                string tempHeader = ConfigurationSettings.AppSettings[headerName];
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
                mailSubject.position = MailAttributes.SubjectPosition.end;
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
            //string mailFrom = @"test@test.local";





            //int testint = 0;
            //string teststring = "";
            //testUebergabe(testint, teststring);
            //Console.Out.WriteLine(testint);
            Server server1 = new Server(smartHost, port, mailFrom, mailTo, user, pwd, rounds, connections, endlessSending, mailAttributes, parsed, waitBetweenMailsms, sendOriginal, newMessageID, fallBack);

            List<string> dirs = new List<string>();
            List<string> filters = new List<string>();
            filters.Add(filter);
            dirs.Add(dir);
            server1.FillQueue(dirs, recursive, filters);
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
