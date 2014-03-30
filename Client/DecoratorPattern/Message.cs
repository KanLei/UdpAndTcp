using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client.DecoratorPattern
{
    class Message
    {
        public string message { get; set; }

        public virtual string GetMessage()
        {
            return message;
        }
    }
}
