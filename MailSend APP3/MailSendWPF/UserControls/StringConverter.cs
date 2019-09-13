using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Globalization;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using MailSend;

namespace MailSendWPF.UserControls
{
    public class StringConverter : IValueConverter
    {
        
        public object Convert(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            if (value == null) return null;
            ObservableCollection<String> stringCol = (ObservableCollection<String>)value;
            ObservableCollection<StringItem> wrappedElementsCol = new ObservableCollection<StringItem>();
            if (stringCol.Count < 1)
            {
                wrappedElementsCol.Add(new StringItem(Constants.sMailWillBeGenerated));
                return wrappedElementsCol;
            }
            if (stringCol.Count == 1)
            {
                string myString = stringCol[0];
                if (myString.CompareTo(Constants.sMailWillBeGenerated) == 0)
                {
                    wrappedElementsCol.Add(new StringItem(myString));
                    return wrappedElementsCol;
                }
            }
            
            foreach (String item in stringCol)
            {
                wrappedElementsCol.Add(new StringItem(item));
            }
            return wrappedElementsCol;//new StringItem();
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
