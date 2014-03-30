using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client.DecoratorPattern
{
    class Decorator : Message
    {
        protected Message component;

        // 装饰
        public void Decorate(Message component)
        {
            this.component = component;
        }

        public override string GetMessage()
        {
            if (component != null)
            {
                message = component.GetMessage();
            }
            return message;
        }
    }
}
