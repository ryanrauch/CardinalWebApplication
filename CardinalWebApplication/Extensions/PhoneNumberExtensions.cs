using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CardinalWebApplication.Extensions
{
    public static class PhoneNumberExtensions
    {
        public static String RemoveNonNumeric(this String input)
        {
            string result = string.Empty;
            foreach (char c in input)
            {
                if (Char.IsNumber(c))
                    result += c;
            }
            return result;
        }
    }
}
