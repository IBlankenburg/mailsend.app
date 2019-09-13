using System;
using System.Text;
using MailSend;
using MailSendWPF.Tools;

namespace StringHelper
{
    public class StringHelper
    {
        /*
         * TODO: ICh suche immer da Ende des Headerattributes nach dem Zeilenumbruch. Theoretisch kann es auch ohne Zeilenumbruch enden wenn man den Header anderst vom Body entfernt und wieder hinzufügt.
         * Der Letzte Eintrag sollte also immer als Ende des Headerattributes angesehen werden wenn der gesamte Header dort endet und kein Zeilenumbruch kommt. Dafür müssen dann beim hinzufürgen des Headers an den Body, der Body zwei Zeilenumbrüche enthalten.
         * */

        /*
         * ToDo: Actung!!!! Der Index an den Schleifenabbruchbedingungen könnte Falsch sein, Bitte prüfen!! < oder <= auf die Länge!!!
         * 
         */

        /*
         * ToDo: FindEndOfHeader mit FindEndOfHeaderWithCharCompare Ersetzen und auf das Unterschiedliche Headerende achten!!!
         */

        public enum CompareMethods
        {
            Text,
            Binary
        }

        public enum ESubjectAddMailNamePosition
        {
            None,
            Begin,
            End,
            Replace
        }


        public static string ReplaceText(string Expression,
            string SearchText,
            string ReplaceText,
            CompareMethods Method)
        {
            string result;
            int position;
            string temp;

            if (Method == CompareMethods.Text)
            {
                result = "";
                SearchText = SearchText.ToUpper();
                temp = Expression.ToUpper();
                position = temp.IndexOf(SearchText);
                while (position >= 0)
                {
                    result = result + Expression.Substring(0, position) + ReplaceText;
                    Expression = Expression.Substring(position + SearchText.Length);
                    temp = temp.Substring(position + SearchText.Length);
                    position = temp.IndexOf(SearchText);
                }
                result = result + Expression;
            }
            else
            {
                result = Expression.Replace(SearchText, ReplaceText);
            }

            return result;
        }

        public static string ReplaceTextBObsolete(string Expression,
            string SearchText,
            string ReplaceText,
            CompareMethods Method)
        {
            StringBuilder result;
            int position;
            string temp;

            if (Method == CompareMethods.Text)
            {
                result = new StringBuilder();
                SearchText = SearchText.ToUpper();
                temp = Expression.ToUpper();
                position = temp.IndexOf(SearchText);
                while (position >= 0)
                {
                    result.Append(Expression.Substring(0, position) + ReplaceText);
                    Expression = Expression.Substring(position + SearchText.Length);
                    temp = temp.Substring(position + SearchText.Length);
                    position = temp.IndexOf(SearchText);
                }
                result.Append(Expression);
                return result.ToString();
            }
            else
            {
                return Expression.Replace(SearchText, ReplaceText);
            }
        }

        public static byte[] GetHeaderDeleteBodyObsolete(byte[] searchByte, out byte[] body)
        {
            byte[] header = null;

            for (int i = 0; i < searchByte.Length; i++)
            {
                if ((i + 3) >= searchByte.LongLength)
                {
                    //Kein Header vorhanden
                    body = searchByte;
                    return null;
                }
                if (searchByte[i] == 13 && searchByte[i + 1] == 10 && searchByte[i + 2] == 13 && searchByte[i + 3] == 10)
                {
                    //Header gefunden
                    header = new byte[i + 1];
                    body = new byte[searchByte.Length - (i + 2)];
                    //ToDo:Hier könnte ein Fehler drin sein wenn der Header mit \n\n endet oder so...
                    Buffer.BlockCopy(searchByte, 0, header, 0, i + 1);
                    Buffer.BlockCopy(searchByte, i + 2, body, 0, (searchByte.Length - (i + 2)));
                    return header;

                    //Array.Copy(searchByte, header
                }
            }
            body = searchByte;
            return null;
        }


        public static byte[] GetHeaderAndBodyPosition(byte[] searchByte, out int body, int startSearchPos = 0)
        {
            byte[] header = null;
            if (startSearchPos > searchByte.Length)
            {
                body = 0;
                return null;
            }
            for (int i = startSearchPos; i < searchByte.Length; i++)
            {
                if ((i + 3) >= searchByte.LongLength)
                {
                    //ToDo: Prüfung müsste eigentlich auf i+1 gehen da 10 10 möglich ist. Dann müsste eine OutOfIndexException abgefangen werden.
                    //Kein Header vorhanden
                    body = 0;
                    return null;
                }
                if ((searchByte[i] == 13 && searchByte[i + 1] == 10 && searchByte[i + 2] == 13 && searchByte[i + 3] == 10))
                {
                    //Header gefunden
                    header = new byte[i + 2];
                    body = (i + 2);
                    Buffer.BlockCopy(searchByte, 0, header, 0, i + 2);
                    return header;
                }
                else if (searchByte[i] == 10 && searchByte[i + 1] == 10)
                {
                    //Header gefunden
                    header = new byte[i + 1];
                    body = (i + 1);
                    Buffer.BlockCopy(searchByte, 0, header, 0, i + 1);
                    return header;
                }
                else if (searchByte[i] == 13 && searchByte[i + 1] == 13)
                {
                    //Header gefunden
                    header = new byte[i + 1];
                    body = (i + 1);
                    Buffer.BlockCopy(searchByte, 0, header, 0, i + 1);
                    return header;
                }


            }
            body = 0;
            return null;
        }

        //ToDo Refactoring - Niedere Prio - Wird nur für KMS benutzt
        public static byte[] GetBody(int bodyPosition, byte[] searchByte)
        {
            if ((searchByte[bodyPosition] == 13 && searchByte[bodyPosition + 1] == 10))
            {
                bodyPosition = bodyPosition + 2;
            }
            else
            {
                return null;
            }
            int bodyCount = searchByte.Length - bodyPosition;
            byte[] body = new byte[bodyCount];
            Buffer.BlockCopy(searchByte, bodyPosition, body, 0, bodyCount);
            return body;

        }

        //Fügt einen Header einem Body Element hinzu
        public static byte[] AddHeaderToBodyByte(byte[] header, byte[] oldMessage, int bodyPosition)
        {
            if (header == null) return oldMessage;
            if (bodyPosition < 0) return oldMessage;
            int bodyLength = oldMessage.Length - bodyPosition;
            byte[] newMessage = new byte[header.Length + bodyLength];
            Buffer.BlockCopy(header, 0, newMessage, 0, header.Length);
            Buffer.BlockCopy(oldMessage, bodyPosition, newMessage, header.Length, bodyLength);
            /*
            if (!body.StartsWith(Environment.NewLine, StringComparison.OrdinalIgnoreCase))
            {
                body.Remove(0, Environment.NewLine.Length);
            }
             */
            /*
            if (!header.EndsWith(Environment.NewLine, StringComparison.OrdinalIgnoreCase))
            {
                header = header + Environment.NewLine;
            }
             */
            return newMessage;
        }
        public static byte[] StringToByteArray(string str)
        {
            System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
            return enc.GetBytes(str);
        }

        public static string ByteArrayToString(byte[] arr)
        {
            System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
            return enc.GetString(arr);
        }

        //Wird nicht benutzt
        //Gibt Header ohne Leerzeile zurück
        public static string GetHeaderDeleteBodyObsolete(ref string searchString)
        {
            int indexBlankLine = searchString.IndexOf(Environment.NewLine + Environment.NewLine, StringComparison.OrdinalIgnoreCase);
            if (indexBlankLine < 0)
            {
                //Header nicht gefunden
                return null;
            }
            string headerWithoutBlankLine = searchString.Substring(0, indexBlankLine + Environment.NewLine.Length);
            searchString = searchString.Remove(0, indexBlankLine + Environment.NewLine.Length);
            return headerWithoutBlankLine;

        }
        //Wird nicht benutzt
        //Fügt einen Header einem Body Element hinzu
        public static string AddHeaderToBodyObsolete(string header, string body)
        {
            if (!body.StartsWith(Environment.NewLine, StringComparison.OrdinalIgnoreCase))
            {
                body.Remove(0, Environment.NewLine.Length);
            }
            if (!header.EndsWith(Environment.NewLine, StringComparison.OrdinalIgnoreCase))
            {
                header = header + Environment.NewLine;
            }
            String newMessage = header + body;
            return newMessage;
        }
        //Entfernt die Message ID
        public static string RemoveMessageID(string searchString)
        {
            return RemoveUntilEndOfHeader(MailSend.Constants.sMessageID, searchString);
        }

        //ToDo Refactoring - Nicht so wichtig - Wird nciht benutzt
        //Entfernt eine Zeile
        public static string RemoveUntilEndOfLine(string pattern, string searchString)
        {
            int beginPattern = searchString.IndexOf(pattern, StringComparison.OrdinalIgnoreCase);
            if (beginPattern == -1)
            {
                return searchString;
            }
            int endLine = (searchString.IndexOf(Environment.NewLine, beginPattern, StringComparison.OrdinalIgnoreCase));
            if (endLine == -1)
            {
                return searchString;
            }
            endLine = endLine + Environment.NewLine.Length;
            string stringWithoutLine = searchString.Remove(beginPattern, endLine - beginPattern);
            return stringWithoutLine;
        }


        //Entfernt Header Felder. Entfernt auch mehrzeilige Header.
        public static string RemoveUntilEndOfHeader(string pattern, string searchString)
        {
            int beginPattern = 0;

            beginPattern = FindBeginOfHeader(searchString, pattern);
            //searchString.IndexOf(pattern, beginPattern, StringComparison.OrdinalIgnoreCase);


            //int beginPattern2 = searchString.IndexOf("Received", StringComparison.OrdinalIgnoreCase);
            if (beginPattern == -1)
            {
                return searchString;
            }
            Constants.LineBreak lineBreak = Constants.LineBreak.Windows;
            int endLine = FindEndOfHeaderWithCharCompare(searchString, beginPattern, ref lineBreak);

            if (endLine == -1)
            {
                return searchString;
            }
            //Wird nicht mehr benötigt da sowieso immer auf das Ende des Headers verwiesen wird
            //endLine = endLine + Environment.NewLine.Length;
            string stringWithoutLine = searchString.Remove(beginPattern, endLine - beginPattern);

            return stringWithoutLine;
        }

        //Entfernt Header Felder. Entfernt auch mehrzeilige Header.
        public static string RemoveUntilEndOfHeaderObsolete(string pattern, string searchString)
        {
            bool msgIDFound = false;
            int beginPattern = 0;
            do
            {
                beginPattern = searchString.IndexOf(pattern, beginPattern, StringComparison.OrdinalIgnoreCase);
                if (beginPattern == -1)
                {
                    break;
                }
                if (beginPattern == 0)
                {
                    msgIDFound = true;
                    break;
                }
                string stringBeforeFoundHeader = searchString.Substring(beginPattern - 1, 1);
                if ((stringBeforeFoundHeader == "\r") || (stringBeforeFoundHeader == "\n"))
                {
                    msgIDFound = true;
                    break;
                }
                beginPattern++;

            } while (!msgIDFound && beginPattern != -1 && (beginPattern < searchString.Length));

            //int beginPattern2 = searchString.IndexOf("Received", StringComparison.OrdinalIgnoreCase);
            if (beginPattern == -1 && !msgIDFound)
            {
                return searchString;
            }

            int endLine = searchString.IndexOf(Environment.NewLine, beginPattern, StringComparison.OrdinalIgnoreCase);

            if (endLine == -1)
            {
                return searchString;
            }
            //Es muss geprüft werden ob der Header über mehrere Zeilen geht.
            bool endLineFound = false;
            do
            {
                int oldEndLine = endLine;
                endLine = endLine + Environment.NewLine.Length;
                if ((searchString.Length <= endLine) || (searchString.Length < (endLine + Constants.sSpace.Length)) || (searchString.Length < (endLine + Constants.sTab.Length)))
                {
                    endLine = oldEndLine;
                    break;
                }

                if (searchString.Substring(endLine, Constants.sSpace.Length).Equals(Constants.sSpace, StringComparison.OrdinalIgnoreCase) ||
                    searchString.Substring(endLine, Constants.sTab.Length).Equals(Constants.sTab, StringComparison.OrdinalIgnoreCase))
                {
                    endLine = searchString.IndexOf(Environment.NewLine, endLine, StringComparison.OrdinalIgnoreCase);
                    //Wenn bei der Message-ID kein NewLine kommt wird NUR Message-ID ersetzt. Der Header ist dann fehlerhaft und ich lasse ihn auch fehlerhaft.
                    if (endLine < 0)
                    {
                        endLine = oldEndLine;
                        endLineFound = true;
                    }
                }
                else
                {
                    endLine = oldEndLine;
                    endLineFound = true;
                }

            } while (!endLineFound);
            endLine = endLine + Environment.NewLine.Length;
            string stringWithoutLine = searchString.Remove(beginPattern, endLine - beginPattern);

            return stringWithoutLine;
        }

        public static int LengthOfHeader(Constants.LineBreak lineBreak)
        {
            if (lineBreak == Constants.LineBreak.Windows)
            {
                return Constants.sLineBreakWindows.Length;
            }
            else if (lineBreak == Constants.LineBreak.Linux)
            {
                return Constants.sLineBreakLinuxMac.Length;
            }
            else if (lineBreak == Constants.LineBreak.OldMac)
            {
                return Constants.sLineBreakOldMac.Length;
            }
            else
            {
            }
            return 0;
        }
        //Überschreibt Header-Values, wenn keine gefunden fügt er sie hinzu
        public static string OverWriteHeaderAttributes(string header, string attributeName, string attributeValue)
        {
            BuildCorrectHeaderAttributeName(ref attributeName);
            //attributeValue = attributeValue.TrimStart(' ');
            int startRemove = FindBeginOfHeader(header, attributeName);

            if (startRemove == -1)
            {
                AddHeader(ref header, attributeName, attributeValue);
            }
            else
            {
                do
                {
                    startRemove = startRemove + attributeName.Length;
                    Constants.LineBreak lineBreak = Constants.LineBreak.Windows;
                    int endLine = FindEndOfHeaderWithCharCompare(header, startRemove, ref lineBreak);
                    if (endLine == -1)
                    {
                        return header;
                    }
                    endLine = endLine - LengthOfHeader(lineBreak);
                    //Es muss geprüft werden ob der Header über mehrere Zeilen geht.

                    header = header.Remove(startRemove, endLine - startRemove);
                    header = header.Insert(startRemove, " " + attributeValue);
                    endLine = (++startRemove);
                    startRemove = FindBeginOfHeader(header, attributeName, endLine);
                } while (startRemove != -1);
            }
            return header;
        }


        //Überschreibt Header-Values, wenn keine gefunden fügt er sie hinzu
        public static string OverWriteHeaderAttributesObsolete(string header, string attributeName, string attributeValue)
        {
            //ToDo Refactoring
            attributeValue = attributeValue.TrimStart(' ');
            attributeName = attributeName.Trim();
            if (!attributeName.EndsWith(":"))
            {
                attributeName = attributeName + ":";

            }
            int startRemove = (header.IndexOf(attributeName, StringComparison.OrdinalIgnoreCase));
            if (startRemove == -1)
            {
                AddHeader(ref header, attributeName, attributeValue);
            }
            else
            {
                do
                {
                    startRemove = startRemove + attributeName.Length;
                    int endLine = header.IndexOf(Environment.NewLine, startRemove, StringComparison.OrdinalIgnoreCase);

                    if (endLine == -1)
                    {
                        return header;
                    }
                    //Es muss geprüft werden ob der Header über mehrere Zeilen geht.
                    bool endLineFound = false;
                    do
                    {
                        int oldEndLine = endLine;
                        endLine = endLine + Environment.NewLine.Length;
                        if ((header.Length <= endLine) || (header.Length < (endLine + Constants.sSpace.Length)) || (header.Length < (endLine + Constants.sTab.Length)))
                        {
                            endLine = oldEndLine;
                            break;
                        }

                        if (header.Substring(endLine, Constants.sSpace.Length).Equals(Constants.sSpace, StringComparison.OrdinalIgnoreCase) ||
                            header.Substring(endLine, Constants.sTab.Length).Equals(Constants.sTab, StringComparison.OrdinalIgnoreCase))
                        {
                            endLine = header.IndexOf(Environment.NewLine, endLine, StringComparison.OrdinalIgnoreCase);
                            //Wenn kein NewLine kommt wird NUR bis zum ersten NewLine ersetzt. Der Header ist dann fehlerhaft und ich lasse ihn auch fehlerhaft.
                            if (endLine < 0)
                            {
                                endLine = oldEndLine;
                                endLineFound = true;
                            }
                        }
                        else
                        {
                            endLine = oldEndLine;
                            endLineFound = true;
                        }

                    } while (!endLineFound);

                    header = header.Remove(startRemove, endLine - startRemove);
                    header = header.Insert(startRemove, " " + attributeValue);
                    endLine = (++startRemove);
                    startRemove = (header.IndexOf(attributeName, endLine, StringComparison.OrdinalIgnoreCase));
                } while (startRemove != -1);
            }
            return header;
        }

        //Ändert das Subject Kann Text am Anfang oder am Ende des Headers einfügen
        public static string ChangeSubject(string header, string attributeValue, MailSend.MailAttributes.HeaderPosition subjectPos, bool maxSubjectLen = false, int maxLen = 256)
        {
            return StringHelper.ChangeAttributes(header, "Subject:", attributeValue, subjectPos, maxSubjectLen, maxLen);
        }

        public static int FindBeginOfHeader(string header, string attributeName, int startSearchPosition = 0)
        {
            int headerStartPosition = startSearchPosition;
            bool startOfHeaderFound = false;

            do
            {
                headerStartPosition = (header.IndexOf(attributeName, headerStartPosition, StringComparison.OrdinalIgnoreCase));
                if (headerStartPosition == -1)
                {
                    return -1;
                }
                /*
                if (headerStartPosition == -1)
                {
                    break;
                }
                 */
                if (headerStartPosition == 0)
                {
                    startOfHeaderFound = true;
                    break;
                }
                string stringBeforeFoundHeader = header.Substring(headerStartPosition - 1, 1);
                if ((stringBeforeFoundHeader == "\r") || (stringBeforeFoundHeader == "\n"))
                {
                    startOfHeaderFound = true;
                    break;
                }
                if (!startOfHeaderFound)
                {
                    headerStartPosition++;
                }
            } while (!startOfHeaderFound && headerStartPosition < header.Length);

            return headerStartPosition;
        }

        public static int FindEndOfLine(string header, int posStartHeader, ref Constants.LineBreak lineBreak)
        {
            //Find the end of the line
            int endLineCount = posStartHeader;
            bool endLineFound = false;

            do
            {
                if (header[endLineCount] == '\n')
                {
                    endLineFound = true;
                    lineBreak = Constants.LineBreak.Linux;
                    //endLineCount = endLineCount + Tools.sLineBreakLinuxMac.Length;
                    break;
                }
                else if (header[endLineCount] == '\r')
                {
                    if (header[++endLineCount] == '\n')
                    {
                        endLineFound = true;
                        lineBreak = Constants.LineBreak.Windows;
                        endLineCount = endLineCount + Constants.sLineBreakWindows.Length - 1;
                        break;

                    }
                }
                //Muss als letztes kommen
                if (header[endLineCount] == '\r')
                {
                    if (!(header[++endLineCount] == '\n'))
                    {
                        endLineFound = true;
                        lineBreak = Constants.LineBreak.OldMac;
                        //endLineCount = endLineCount + Tools.sLineBreakOldMac.Length;
                        break;
                    }
                }
                if (!endLineFound)
                {
                    endLineCount++;
                }
            } while (!endLineFound && endLineCount < header.Length);
            if (endLineFound)
            {
                return endLineCount;
            }
            return -1;
        }
        //ACHTUNG FindEndOfHeader und FindEndOfHeaderWithCharCompare unterscheiden sich!!! 
        //Bei FindEndOfHeader wird das Ende des Headers da angezeigt wo der Zeilenumbruch beginnt
        //Bei FindEndOfHeaderWithCharCompare wird das Ende des Headers da angezeigt wo der Zeilenumbruch endet!!!!
        public static int FindEndOfHeaderWithCharCompare(string header, int posStartHeader, ref Constants.LineBreak lineBreak)
        {
            bool endOfHeaderFound = false;
            int endOfHeaderCount = posStartHeader;
            int oldEndOfHeader = -1;
            do
            {

                endOfHeaderCount = FindEndOfLine(header, endOfHeaderCount, ref lineBreak);

                if (endOfHeaderCount == -1)
                {
                    if (oldEndOfHeader > posStartHeader)
                    {
                        return oldEndOfHeader;
                    }
                    else
                    {
                        return -1;
                    }
                }
                else
                {
                    oldEndOfHeader = endOfHeaderCount;
                }

                if ((header.Length <= endOfHeaderCount) || (header.Length < (endOfHeaderCount + Constants.sSpace.Length)) || (header.Length < (endOfHeaderCount + Constants.sTab.Length)))
                {
                    //Header gefunden, da Header zu Ende
                    return endOfHeaderCount;
                }

                if (header.Substring(endOfHeaderCount, Constants.sSpace.Length).Equals(Constants.sSpace, StringComparison.OrdinalIgnoreCase) ||
                   header.Substring(endOfHeaderCount, Constants.sTab.Length).Equals(Constants.sTab, StringComparison.OrdinalIgnoreCase))
                {
                    //Mehrzeiliger Header
                    //Suche wieder nach dem Ende der Neuen Zeile
                    endOfHeaderCount++;
                    continue;
                }
                else
                {
                    //Header gefunden
                    //Der Header geht in der neuen Zeile nicht mehr weiter
                    //Achtung der Header zeigt auf das Ende der Zeile, daher auch die Art des Zeilenumbruchs beachten, z.B. "\r\n" oder "\n" oder "\r"!!!!!!
                    endOfHeaderCount = oldEndOfHeader;

                    //ToDo TEST Nachher entfernen
                    string tempHeaderAttribute = header.Substring(posStartHeader, endOfHeaderCount - posStartHeader);
                    //Ende Test Nachher entfernen

                    endOfHeaderFound = true;
                }
                /*
                if (endOfHeaderFound)
                {
                    Log.logger.Debug("EndOfHeaderCount " + endOfHeaderCount);
                }
                 */
                if (!endOfHeaderFound)
                {
                    endOfHeaderCount++;
                }
            } while (!endOfHeaderFound && endOfHeaderCount < header.Length);


            if (!endOfHeaderFound)
            {
                return -1;
            }


            return endOfHeaderCount;
        }


        public static int SearchByte(byte[] target, byte[] pattern, out int foundEndPos, int startPos = 0)
        {
            int pos = startPos;
            int merker = 0;
            int actPos = pos;
            do
            {
                //actPos = pos;
                if (target[actPos] == pattern[merker])
                {
                    if (merker == pattern.Length)
                    {
                        //Pattern gefunden
                        foundEndPos = actPos;
                        return pos;
                    }
                    merker++;
                }
                else
                {
                    merker = 0;
                    pos++;
                    actPos = pos;
                }

                actPos++;
            } while (merker != pattern.Length && actPos <= target.Length && pos <= target.Length);
            foundEndPos = -1;
            return -1;
        }

        public static string GetFirstHeaderAttributeAndValue(string header, string attributeName)
        {
            int beginOfHeader = FindBeginOfHeader(header, attributeName);
            Constants.LineBreak lineBreak = Constants.LineBreak.Windows;
            int endOfHeader = FindEndOfHeaderWithCharCompare(header, beginOfHeader, ref lineBreak);
            endOfHeader = endOfHeader - LengthOfHeader(lineBreak);
            string headerAttributeAndValue = header.Substring(beginOfHeader, endOfHeader - beginOfHeader);
            return headerAttributeAndValue;
        }

        public static string GetHeaderValue(string header, string attributeName)
        {
            BuildCorrectHeaderAttributeName(ref attributeName);
            string headerAttributeAndValue = GetFirstHeaderAttributeAndValue(header, attributeName);
            string headerValue = headerAttributeAndValue.Substring(attributeName.Length + Constants.sSpace.Length);
            return headerValue;
        }

        //ACHTUNG FindEndOfHeader und FindEndOfHeaderWithCharCompare unterscheiden sich!!! 
        //Bei FindEndOfHeader wird das Ende des Headers da angezeigt wo der Zeilenumbruch beginnt
        //Bei FindEndOfHeaderWithCharCompare wird das Ende des Headers da angezeigt wo der Zeilenumbruch endet!!!!
        public static int FindEndOfHeaderObsolete(string header, int posStartHeader)
        {
            int endLine = -1;
            Constants.LineBreak lineBreak = new Constants.LineBreak();
            //Hier wird das Ende der HeaderZeile gesucht
            endLine = header.IndexOf(Constants.sLineBreakLinuxMac, posStartHeader, StringComparison.OrdinalIgnoreCase);

            if (endLine == -1)
            {
                //ToDo: Mit \n und \r kompatibel machen.
                return -1;
            }
            /*
            if (endLine == 0)
            {
                //Fehler
            }
             */
            if (header[--endLine] == '\r')
            {
                //Windows Zeilenumbruch
                lineBreak = Constants.LineBreak.Windows;
            }
            else
            {
                //Linux Zeilenumbruch Darf laut RFC xyz nicht sein
                lineBreak = Constants.LineBreak.Linux;
            }

            //Es muss geprüft werden ob der Header über mehrere Zeilen geht.
            bool endLineFound = false;
            do
            {
                int oldEndLine = endLine;
                if (lineBreak == Constants.LineBreak.Windows)
                {

                    endLine = endLine + Constants.sLineBreakWindows.Length;
                }
                else
                {
                    endLine = endLine + Constants.sLineBreakLinuxMac.Length;
                }
                if ((header.Length <= endLine) || (header.Length < (endLine + Constants.sSpace.Length)) || (header.Length < (endLine + Constants.sTab.Length)))
                {
                    endLine = oldEndLine;
                    break;
                }

                if (header.Substring(endLine, Constants.sSpace.Length).Equals(Constants.sSpace, StringComparison.OrdinalIgnoreCase) ||
                    header.Substring(endLine, Constants.sTab.Length).Equals(Constants.sTab, StringComparison.OrdinalIgnoreCase))
                {
                    endLine = header.IndexOf(Environment.NewLine, endLine, StringComparison.OrdinalIgnoreCase);
                    //Wenn kein NewLine kommt wird NUR bis zum ersten NewLine ersetzt. Der Header ist dann fehlerhaft und ich lasse ihn auch fehlerhaft.
                    if (endLine < 0)
                    {
                        endLine = oldEndLine;
                        endLineFound = true;
                    }
                }
                else
                {
                    endLine = oldEndLine;
                    endLineFound = true;
                }

            } while (!endLineFound && endLine < header.Length);

            return endLine;
        }

        public static void BuildCorrectHeaderAttributeName(ref string attributeName)
        {
            //attributeValue = attributeValue.TrimStart(' ');
            attributeName = attributeName.Trim();
            if (!attributeName.EndsWith(":"))
            {
                attributeName = attributeName + ":";

            }

        }
        //Ändert Header Attribute. Auch mehrzeilige Header Kann Text am Anfang oder am Ende des Headers einfügen
        public static string ChangeAttributes(string header, string attributeName, string attributeValue, MailSend.MailAttributes.HeaderPosition subjectPos, bool maxAttributeLen = false, int maxLen = 256)
        {
            BuildCorrectHeaderAttributeName(ref attributeName);

            int startRemove = -1;


            startRemove = FindBeginOfHeader(header, attributeName);
            if (startRemove == -1)
            {
                AddHeader(ref header, attributeName, attributeValue);
                return header;
            }

            int endLine = -1;
            do
            {
                startRemove = startRemove + attributeName.Length;

                Constants.LineBreak lineBreak = Constants.LineBreak.Windows;
                endLine = FindEndOfHeaderWithCharCompare(header, startRemove, ref lineBreak);
                if (endLine == -1)
                {
                    return header;
                }
                endLine = endLine - LengthOfHeader(lineBreak);


                //header = header.Remove(startRemove, endLine - startRemove);
                string subject = header.Substring(startRemove, endLine - startRemove);
                if (subjectPos == MailAttributes.HeaderPosition.end)
                {
                    if (maxAttributeLen)
                    {
                        if (subject.Length + attributeValue.Length <= maxLen)
                        {
                            header = header.Remove(startRemove, subject.Length);
                            subject = subject + attributeValue;
                            //subject = attributeValue;
                        }
                    }
                    else
                    {
                        header = header.Remove(startRemove, subject.Length);
                        subject = subject + attributeValue;
                        //subject = attributeValue;
                    }

                    header = header.Insert(startRemove, subject);
                }
                else if (subjectPos == MailAttributes.HeaderPosition.begin)
                {
                    if (maxAttributeLen)
                    {
                        if (subject.Length + attributeValue.Length <= maxLen)
                        {
                            header = header.Remove(startRemove, subject.Length);
                            attributeValue = attributeValue + subject;
                            subject = attributeValue;
                        }
                    }
                    else
                    {
                        header = header.Remove(startRemove, subject.Length);
                        attributeValue = attributeValue + subject;
                        subject = attributeValue;
                    }
                    header = header.Insert(startRemove, subject);
                }

                //header = header.Insert(startRemove, " " + subject);
                endLine = (++startRemove);
                startRemove = FindBeginOfHeader(header, attributeName, endLine);
            } while (startRemove != -1 && endLine < header.Length);

            return header;
        }



        public static void AddHeader(ref string sHeader, string headerName, string headerValue)
        {
            if (sHeader == String.Empty)
            {
                sHeader = MakeHeaderLine(headerName, headerValue) + Constants.sLineBreakWindows;
                return;
            }
            else
            {
                bool endsWith = sHeader.EndsWith(Constants.sLineBreakWindows + Constants.sLineBreakWindows, StringComparison.OrdinalIgnoreCase);
                if (endsWith)
                {
                    sHeader = sHeader.Remove(sHeader.Length - 1 - Constants.sLineBreakWindows.Length);
                }

                endsWith = sHeader.EndsWith(Constants.sLineBreakWindows, StringComparison.OrdinalIgnoreCase);
                if (endsWith)
                {
                    sHeader = sHeader + MakeHeaderLine(headerName, headerValue) + Constants.sLineBreakWindows;
                    return;
                }
                /*
            else
            {
                sHeader = sHeader + Tools.sLineBreakWindows + MakeHeaderLine(headerName, headerValue) + Tools.sLineBreakWindows;
                return;
            }
            */



                endsWith = sHeader.EndsWith(Constants.sLineBreakLinuxMac + Constants.sLineBreakLinuxMac, StringComparison.OrdinalIgnoreCase);
                if (endsWith)
                {
                    sHeader = sHeader.Remove(sHeader.Length - 1 - Constants.sLineBreakLinuxMac.Length);
                }

                endsWith = sHeader.EndsWith(Constants.sLineBreakLinuxMac, StringComparison.OrdinalIgnoreCase);
                if (endsWith)
                {
                    sHeader = sHeader + MakeHeaderLine(headerName, headerValue) + Constants.sLineBreakLinuxMac;
                    return;
                }
                /*
                else
                {
                    sHeader = sHeader + Tools.sLineBreakLinuxMac + MakeHeaderLine(headerName, headerValue) + Tools.sLineBreakLinuxMac;
                }
                 */

                endsWith = sHeader.EndsWith(Constants.sLineBreakOldMac + Constants.sLineBreakOldMac, StringComparison.OrdinalIgnoreCase);
                if (endsWith)
                {
                    sHeader = sHeader.Remove(sHeader.Length - 1 - Constants.sLineBreakOldMac.Length);
                }

                endsWith = sHeader.EndsWith(Constants.sLineBreakOldMac, StringComparison.OrdinalIgnoreCase);
                if (endsWith)
                {
                    sHeader = sHeader + MakeHeaderLine(headerName, headerValue) + Constants.sLineBreakOldMac;
                    return;
                }
                /*
            else
            {
                sHeader = sHeader + Tools.sLineBreakOldMac + MakeHeaderLine(headerName, headerValue) + Tools.sLineBreakOldMac;
                return;
            }
            */
                sHeader = sHeader + Constants.sLineBreakWindows + MakeHeaderLine(headerName, headerValue) + Constants.sLineBreakWindows;

            }



        }

        //ToDo Refactoring - erledigt
        public static void AddHeaderObsolete(ref string sHeader, string headerName, string headerValue)
        {
            if (sHeader == String.Empty)
            {
                sHeader = MakeHeaderLine(headerName, headerValue) + Environment.NewLine;
            }
            else
            {


                bool endsWith = sHeader.EndsWith(Environment.NewLine + Environment.NewLine, StringComparison.OrdinalIgnoreCase);
                if (!endsWith)
                {
                    endsWith = sHeader.EndsWith(Environment.NewLine, StringComparison.OrdinalIgnoreCase);
                    if (endsWith)
                    {
                        sHeader = sHeader + MakeHeaderLine(headerName, headerValue) + Environment.NewLine;
                    }
                    else
                    {
                        sHeader = sHeader + Environment.NewLine + MakeHeaderLine(headerName, headerValue) + Environment.NewLine;
                    }

                }
                else
                {
                    string headerLine = MakeHeaderLine(headerName, headerValue);
                    //ToDo implement if header ends with two LineBreaks
                }

            }



        }


        public static string ChangeAttributesObsolete(string header, string attributeName, string attributeValue, MailSend.MailAttributes.HeaderPosition subjectPos, bool maxAttributeLen = false, int maxLen = 256)
        {
            //attributeValue = attributeValue.TrimStart(' ');
            attributeName = attributeName.Trim();
            if (!attributeName.EndsWith(":"))
            {
                attributeName = attributeName + ":";

            }

            int startRemove = (header.IndexOf(attributeName, StringComparison.OrdinalIgnoreCase));
            if (startRemove == -1)
            {
                AddHeader(ref header, attributeName, attributeValue);
            }
            else
            {
                do
                {
                    startRemove = startRemove + attributeName.Length;
                    int endLine = header.IndexOf(Environment.NewLine, startRemove, StringComparison.OrdinalIgnoreCase);

                    if (endLine == -1)
                    {
                        //ToDo: Mit \n und \r kompatibel machen.
                        return header;
                    }
                    //Es muss geprüft werden ob der Header über mehrere Zeilen geht.
                    bool endLineFound = false;
                    do
                    {
                        int oldEndLine = endLine;
                        endLine = endLine + Environment.NewLine.Length;
                        if ((header.Length <= endLine) || (header.Length < (endLine + Constants.sSpace.Length)) || (header.Length < (endLine + Constants.sTab.Length)))
                        {
                            endLine = oldEndLine;
                            break;
                        }

                        if (header.Substring(endLine, Constants.sSpace.Length).Equals(Constants.sSpace, StringComparison.OrdinalIgnoreCase) ||
                            header.Substring(endLine, Constants.sTab.Length).Equals(Constants.sTab, StringComparison.OrdinalIgnoreCase))
                        {
                            endLine = header.IndexOf(Environment.NewLine, endLine, StringComparison.OrdinalIgnoreCase);
                            //Wenn kein NewLine kommt wird NUR bis zum ersten NewLine ersetzt. Der Header ist dann fehlerhaft und ich lasse ihn auch fehlerhaft.
                            if (endLine < 0)
                            {
                                endLine = oldEndLine;
                                endLineFound = true;
                            }
                        }
                        else
                        {
                            endLine = oldEndLine;
                            endLineFound = true;
                        }

                    } while (!endLineFound);

                    //header = header.Remove(startRemove, endLine - startRemove);
                    string subject = header.Substring(startRemove, endLine - startRemove);
                    if (subjectPos == MailAttributes.HeaderPosition.end)
                    {
                        if (maxAttributeLen)
                        {
                            if (subject.Length + attributeValue.Length <= maxLen)
                            {
                                header = header.Remove(startRemove, subject.Length);
                                subject = subject + attributeValue;
                                //subject = attributeValue;
                            }
                        }
                        else
                        {
                            header = header.Remove(startRemove, subject.Length);
                            subject = subject + attributeValue;
                            //subject = attributeValue;
                        }

                        header = header.Insert(startRemove, subject);
                    }
                    else if (subjectPos == MailAttributes.HeaderPosition.begin)
                    {
                        if (maxAttributeLen)
                        {
                            if (subject.Length + attributeValue.Length <= maxLen)
                            {
                                header = header.Remove(startRemove, subject.Length);
                                attributeValue = attributeValue + subject;
                                subject = attributeValue;
                            }
                        }
                        else
                        {
                            header = header.Remove(startRemove, subject.Length);
                            attributeValue = attributeValue + subject;
                            subject = attributeValue;
                        }
                        header = header.Insert(startRemove, subject);
                    }

                    //header = header.Insert(startRemove, " " + subject);
                    endLine = (++startRemove);
                    startRemove = (header.IndexOf(attributeName, endLine, StringComparison.OrdinalIgnoreCase));
                } while (startRemove != -1);
            }
            return header;
        }

        /*
        public static void AddHeader(ref string sHeader, string headerName, string headerValue)
        {
            if (sHeader == String.Empty)
            {
                sHeader = MakeHeaderLine(headerName, headerValue) + Environment.NewLine;
            }
            else
            {


                bool endsWith = sHeader.EndsWith(Environment.NewLine + Environment.NewLine, StringComparison.OrdinalIgnoreCase);
                if (!endsWith)
                {
                    endsWith = sHeader.EndsWith(Environment.NewLine, StringComparison.OrdinalIgnoreCase);
                    if (endsWith)
                    {
                        sHeader = sHeader + MakeHeaderLine(headerName, headerValue) + Environment.NewLine;
                    }
                    else
                    {
                        sHeader = sHeader + Environment.NewLine + MakeHeaderLine(headerName, headerValue) + Environment.NewLine;
                    }

                }
                else
                {
                    string headerLine = MakeHeaderLine(headerName, headerValue);
                    //ToDo implement if header ends with two LineBreaks
                }

            }


        }
        */




        public static string MakeHeaderLine(string headerKey, string headerValue)
        {
            headerKey = headerKey.Trim();
            headerValue = headerValue.TrimStart(' ');
            if (!headerKey.EndsWith(":"))
            {
                headerKey = headerKey + ":";

            }

            string headerLine = headerKey + " " + headerValue;
            return headerLine;
        }

        public static int FindEndOfLineNew(string header, int posStartHeader, ref Constants.LineBreak lineBreak)
        {
            //Find the end of the line
            int endLineCount = posStartHeader;
            bool endLineFound = false;

            do
            {
                if (header[endLineCount] == '\n')
                {
                    endLineFound = true;
                    lineBreak = Constants.LineBreak.Linux;
                    //endLineCount = endLineCount + Tools.sLineBreakLinuxMac.Length;
                    break;
                }
                else if (header[endLineCount] == '\r')
                {
                    if (header.Length < header[endLineCount + 1])
                    {
                        endLineFound = true;
                        lineBreak = Constants.LineBreak.OldMac;
                        break;
                    }
                    if (header[++endLineCount] == '\n')
                    {
                        endLineFound = true;
                        lineBreak = Constants.LineBreak.Windows;
                        endLineCount = endLineCount + Constants.sLineBreakWindows.Length - 1;
                        break;

                    }
                }
                //Muss als letztes kommen
                if (header[endLineCount] == '\r')
                {
                    if (header.Length < header[endLineCount + 1])
                    {
                        endLineFound = true;
                        lineBreak = Constants.LineBreak.OldMac;
                        break;
                    }
                    if (!(header[++endLineCount] == '\n'))
                    {
                        endLineFound = true;
                        lineBreak = Constants.LineBreak.OldMac;
                        //endLineCount = endLineCount + Tools.sLineBreakOldMac.Length;
                        break;
                    }
                }
                if (!endLineFound)
                {
                    endLineCount++;
                }
            } while (!endLineFound && endLineCount < header.Length);
            if (endLineFound)
            {
                return endLineCount;
            }
            return -1;
        }

        //ACHTUNG FindEndOfHeader und FindEndOfHeaderWithCharCompare unterscheiden sich!!! 
        //Bei FindEndOfHeader wird das Ende des Headers da angezeigt wo der Zeilenumbruch beginnt
        //Bei FindEndOfHeaderWithCharCompare wird das Ende des Headers da angezeigt wo der Zeilenumbruch endet!!!!
        public static int FindEndOfHeaderWithCharCompareNew(Node<object> parentMailNode, string header, int posStartHeader, ref Constants.LineBreak lineBreak)
        {
            bool endOfHeaderFound = false;
            int endOfHeaderCount = posStartHeader;
            int oldEndOfHeader = -1;
            do
            {

                endOfHeaderCount = FindEndOfLineNew(header, endOfHeaderCount, ref lineBreak);

                if (endOfHeaderCount == -1)
                {
                    if (oldEndOfHeader > posStartHeader)
                    {
                        return oldEndOfHeader;
                    }
                    else
                    {
                        return -1;
                    }
                }
                else
                {
                    oldEndOfHeader = endOfHeaderCount;
                }

                if ((header.Length <= endOfHeaderCount) || (header.Length < (endOfHeaderCount + Constants.sSpace.Length)) || (header.Length < (endOfHeaderCount + Constants.sTab.Length)))
                {
                    //Header gefunden, da Header zu Ende
                    return endOfHeaderCount;
                }

                if (header.Substring(endOfHeaderCount, Constants.sSpace.Length).Equals(Constants.sSpace, StringComparison.OrdinalIgnoreCase) ||
                   header.Substring(endOfHeaderCount, Constants.sTab.Length).Equals(Constants.sTab, StringComparison.OrdinalIgnoreCase))
                {
                    //Mehrzeiliger Header
                    //Suche wieder nach dem Ende der Neuen Zeile
                    endOfHeaderCount++;
                    continue;
                }
                else
                {
                    //Header gefunden
                    //Der Header geht in der neuen Zeile nicht mehr weiter
                    //Achtung der Header zeigt auf das Ende der Zeile, daher auch die Art des Zeilenumbruchs beachten, z.B. "\r\n" oder "\n" oder "\r"!!!!!!
                    endOfHeaderCount = oldEndOfHeader;

                    //ToDo TEST Nachher entfernen
                    string headerNameValue = header.Substring(posStartHeader, endOfHeaderCount - posStartHeader);
                    //Ende Test Nachher entfernen

                    endOfHeaderFound = true;
                    MailNode<object> headerNameValueNode = new MailNode<object>(headerNameValue, parentMailNode, MailTypes.eMailTypes.HeaderNameValue);
                   parentMailNode.Childrens.Add(headerNameValueNode);
                   SplitHeaderAndValue(headerNameValueNode, headerNameValue);
                }
                /*
                if (endOfHeaderFound)
                {
                    Log.logger.Debug("EndOfHeaderCount " + endOfHeaderCount);
                }
                 */
                if (!endOfHeaderFound)
                {
                    endOfHeaderCount++;
                }
            } while (!endOfHeaderFound && endOfHeaderCount < header.Length);


            if (!endOfHeaderFound)
            {
                return -1;
            }


            return endOfHeaderCount;
        }
        public static void SplitHeaderAndValue(MailNode<object> headerNodeParent, string headerPart)
        {
            int headerNameEndIndex = headerPart.IndexOf(':');
            string headerName = headerPart.Substring(0, (headerNameEndIndex + 1));
            string headerValue = headerPart.Substring(headerNameEndIndex, headerPart.Length - headerNameEndIndex);
            MailNode<object> headerNameNode = new MailNode<object>(headerName, headerNodeParent, MailTypes.eMailTypes.HeaderName);
            MailNode<object> headerValueNode = new MailNode<object>(headerValue, headerNodeParent, MailTypes.eMailTypes.HeaderValue);
            headerNodeParent.Childrens.Add(headerNameNode);
            headerNodeParent.Childrens.Add(headerValueNode);

        }
        public static void CreateHeaderTree(string header, Node<object> node, ref Constants.LineBreak lineBreak, int posStartHeader = 0)
        {
            MailNode<object> mainHeaderMailNode = new MailNode<object>(header, node, MailTypes.eMailTypes.MainHeader);
            int endOfHeaderCount = posStartHeader;
            do
            {
                endOfHeaderCount = FindEndOfHeaderWithCharCompareNew(mainHeaderMailNode, header, endOfHeaderCount, ref lineBreak);
            } while (endOfHeaderCount < header.Length);
        }


    }
}
