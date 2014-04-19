InstantMessaging
================

### 中文
     
     这是一个基于 UDP 和 Tcp 协议编写的局域网多人即时聊天软件。
     
####  主要功能

-  用户登陆/注册（验证）
-  请求登录服务器并获取在线用户
-  发起聊天请求
-  关闭聊天窗口，保存聊天信息

#### UtpUtils

将聊天功能封装到此 dll 中，客户端或者服务端只需简单的进行事件绑定，即可实现聊天，无需写更多的代码。

demo:

    /* 注册监听事件 */
    UdpUtils.Server.SignInNotify += ReceiveMessage;
    UdpUtils.Server.SignOutNotify += ReceiveMessage;

    /* 启动服务器 */
    UdpUtils.Server.StartServer(txtIPAddress.Text, Convert.ToInt32(txtPort.Text));

    private void ReceiveMessage(UdpUtils.Message message)
    {
       // 对接收到的消息进行处理      
    }


你可以在这里查看界面设计和草图：[*这里*](http://kanlei.github.io/project/2014/01/08/instant-messaging/)

---

### English

    This is a instant messaging base on Udp and Tcp protocol
    
#### main function

- user login/register(validation)
- login request and get online users' info
- send chat request to other users
- close chat form, save chat content

#### UtpUtils

All the chat stuff writed in the UtpUtils.dll，Client or Server does not nedd to write more code for chat. Just register the Event.

demo:

    /* Register listen event */
    UdpUtils.Server.SignInNotify += ReceiveMessage;
    UdpUtils.Server.SignOutNotify += ReceiveMessage;

    /* Start the server */
    UdpUtils.Server.StartServer(txtIPAddress.Text, Convert.ToInt32(txtPort.Text));

    private void ReceiveMessage(UdpUtils.Message message)
    {
       // Process the recevie message     
    }

You will see the GUI design and draft design: [*here*](http://kanlei.github.io/project/2014/01/08/instant-messaging/)
