using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Windows.Data;

namespace MailSendWPF.UserControls
{
    class RecipientGroupTextToBoolConverter : IValueConverter
    {
        
    
        
        public object Convert(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            if (value == null) return null;
            bool groupEnabled = (bool)value;

            /*
            string recipientGroupString = (string)value;
            if (!String.IsNullOrEmpty(recipientGroupString))
            {
                return false;
            }
             */
            return !groupEnabled;
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
    

