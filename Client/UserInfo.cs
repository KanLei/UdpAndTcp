using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client
{
    [Serializable]
    public class UserInfo
    {
        public string Name { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }

        public UserInfo()
        {

        }
    }
}
