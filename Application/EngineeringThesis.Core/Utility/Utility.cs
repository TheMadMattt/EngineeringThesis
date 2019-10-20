using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace EngineeringThesis.Core.Utility
{
    public class Utility
    {
        public static bool IsTextNumeric(string text)
        {
            Regex regex = new Regex("[^0-9]+");
            return regex.IsMatch(text);
        }

        public static bool IsTextCurrency(string text)
        {
            Regex regex = new Regex("^[0-9]+\\.[0-9]{2}$");
            return regex.IsMatch(text);
        }
    }
}
