using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextFileParser
{
    public class StringHelper
    {
        /// <summary>
        /// Efficiently remove special characters. Faster than regex.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string RemoveSpecialCharacters(string str)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in str)
            {
                if (c >= '0' && c <= '9' || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z'))
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }
    }
}
