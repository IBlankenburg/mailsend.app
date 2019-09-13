using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace MailSendWPF.Windows
{
    class PauseValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            int val;
            if (Int32.TryParse(value.ToString(), out val))
            {
                if (val < 0 || val > 2147483647 )
                {
                    return new ValidationResult(false, "only number between 0 and 2 147 483 647");
                }
            }
            else
            {
                return new ValidationResult(false, "only number between 0 and 2 147 483 647");
            }
            return ValidationResult.ValidResult;
        }
    }
}
