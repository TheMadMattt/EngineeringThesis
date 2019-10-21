using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Controls;

namespace EngineeringThesis.Core.Utility.Validators
{
    public class IsGreaterThanZeroValidator : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var number = double.TryParse(value.ToString(), out var doubleNumber);

            if (doubleNumber > 0)
            {
                return new ValidationResult(true, null);
            }

            return new ValidationResult(false, "Wartość musi być większa od 0");
        }
    }
}
