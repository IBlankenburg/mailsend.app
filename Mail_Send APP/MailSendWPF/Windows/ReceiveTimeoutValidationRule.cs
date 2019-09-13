using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace MailSendWPF.Windows
{
    class ReceiveTimeoutValidationRule:ValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            int val;
            if (Int32.TryParse(value.ToString(), out val))
            {
                if (val < 0 || val > 999999999)
                {
                    return new ValidationResult(false, "Timeout between 0 and 999999999");
                }
            }
            else
            {
                return new ValidationResult(false, "Timeout between 0 and 999999999 only numbers");
            }
            return ValidationResult.ValidResult;
        }
    }
}
