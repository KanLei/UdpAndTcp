
namespace Client.DecoratorPattern
{
    class DecorateMessage:Decorator
    {
        public override string GetMessage()
        {
            string msg= base.GetMessage();
            // 加密
            CryptoHelper helper = new CryptoHelper("ABCDEFGHIJKLMNOP");
            return helper.Encrypt(msg);
        }
    }
}
