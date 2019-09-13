using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Globalization;
using System.Windows.Data;
using MailSendWPF.Configuration;

namespace MailSendWPF.Windows
{
    class RecipientGroupValidationRule : ValidationRule
    {
        public override ValidationResult Validate(
   object value, CultureInfo cultureInfo)
        {

            BindingGroup bindingGroup = value as BindingGroup;
            if (bindingGroup == null)
                return new ValidationResult(false,
                  "CustomerObjectiveOrReasonValidationRule should only be used with a BindingGroup");

            if (bindingGroup.Items.Count == 1)
            {
                object item = bindingGroup.Items[0];
                ServerSchema mySchema = (ServerSchema)item;
                string name = mySchema.RecipientGroup;
                string start = mySchema.RecipientGroupStart;
                string end = mySchema.RecipientGroupEnd;
                string domain = mySchema.RecipientGroupDomain;
                int iStart;
                int iEnd;
                if (!Int32.TryParse(start, out iStart) || !Int32.TryParse(end, out iEnd))
                {
                    return new ValidationResult(false,
      "You must enter only numbers in RecipientGroupStart and RecipientGroupEnd");

                }
                if (iStart > iEnd)
                {
                    iEnd = iStart;
                }
                if (iEnd < 0 && iStart < 0)
                {
                    iStart = 0;
                    iEnd = 0;
                }
                if (iStart < 0)
                {
                    iStart = iEnd;
                }
                if (iEnd < 0)
                {
                    iEnd = iStart;
                }
                mySchema.RecipientGroupStart = iStart.ToString();
                mySchema.RecipientGroupEnd = iEnd.ToString();


                //Int32.TryParse
                //ActivityEditorViewModel viewModel = item as ActivityEditorViewModel;
                /*
                if (viewModel != null && viewModel.Activity != null &&
                  !viewModel.Activity.CustomerObjectiveOrReasonEntered())
                    return new ValidationResult(false,
                      "You must enter one of Customer, Objective, or Reason to a valid entry");
                 */
            }
            return ValidationResult.ValidResult;
        }

    }
}
