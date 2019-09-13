using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace MailSendWPF.Windows
{
    class ConnectionValidationRule:ValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            int val;
            if (Int32.TryParse(value.ToString(), out val))
            {
                if (val < 1 || val > 20)
                {
                    return new ValidationResult(false, "Connections between 1 and 20");
                }
            }
            else
            {
                return new ValidationResult(false, "Connections between 1 and 20 only numbers");
            }
            return ValidationResult.ValidResult;
        }
    }
}
