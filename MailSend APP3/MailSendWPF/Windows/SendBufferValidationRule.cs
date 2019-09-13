using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace MailSendWPF.Windows
{
    class SendBufferValidationRule:ValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            int val;
            if (Int32.TryParse(value.ToString(), out val))
            {
                if (val < 64 || val > 999999999)
                {
                    return new ValidationResult(false, "Buffer between 64 and 999999999");
                }
            }
            else
            {
                return new ValidationResult(false, "Buffer between 64 and 999999999 only numbers");
            }
            return ValidationResult.ValidResult;
        }
    }
}
