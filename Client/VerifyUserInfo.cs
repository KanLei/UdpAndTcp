using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Client
{
    class VerifyUserInfo
    {
        public static bool Verify(string userName, string password, string email)
        {
            // validate username
            if (Regex.IsMatch(userName, @"^\w{1,15}$", RegexOptions.IgnoreCase) == true)
            {
                MatchCollection collection = Regex.Matches(userName, @"\w{1,10}");
                foreach (Match m in collection)
                {
                    string str = m.Value;
                }
                // validate password
                if (Regex.IsMatch(password, @"^\w{1,15}$", RegexOptions.IgnoreCase) == true)
                {
                    // validate email
                    if (Regex.IsMatch(email, @"\b\w+[\w.]*@[\w.]+\.\w+", RegexOptions.IgnoreCase) == true)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
