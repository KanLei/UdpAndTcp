using System;
using System.Windows.Forms;
using Decorator = Client.DecoratorPattern;
using System.Linq;

namespace Client
{
    static class Extension
    {
        /// <summary>
        /// shake window form when come across input error
        /// </summary>
        public static void ShakeForm(this Form form)
        {
            for (int i = 0; i < 4; i++)
            {
                Enumerable.Range(0, 4).ToList().ForEach(x => { form.Left += x; System.Threading.Thread.Sleep(10); });
                Enumerable.Range(0, 4).ToList().ForEach(y => { form.Left -= y; System.Threading.Thread.Sleep(10); });
            }
        }

        public static bool IsNullOrWhiteSpace(this string str)
        {
            return String.IsNullOrWhiteSpace(str);
        }

        #region  Encrypt/Decrypt

        /// <summary>
        /// Encrypt Message Content
        /// </summary>
        public static string Encrypt(this string value)
        {
            Decorator.Message cryptoMsg = new Decorator.Message { message = value };
            Decorator.DecorateMessage decoratorMessage = new Decorator.DecorateMessage();
            decoratorMessage.Decorate(cryptoMsg);
            return decoratorMessage.GetMessage();
        }

        /// <summary>
        /// Decrypt Message Content
        /// </summary>
        public static string Decrypt(this string value)
        {
            Decorator.CryptoHelper helper = new Decorator.CryptoHelper("ABCDEFGHIJKLMNOP");
            return helper.Decrypt(value);
        }
        #endregion
    }
}
