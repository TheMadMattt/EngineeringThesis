using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Controls;

namespace EngineeringThesis.Core.Utility.Validators
{
    public class IsEmptyValidator: ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value != null)
            {
                if (!string.IsNullOrEmpty(value.ToString()))
                {
                    return new ValidationResult(true, null);
                }
            }
            return new ValidationResult(false, "Pole nie może być puste");
        }
    }
}
