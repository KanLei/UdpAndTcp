using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UdpUtils
{
    /// <summary>
    /// use to diff different message
    /// </summary>
    public enum MessageEnum
    {
        /// <summary>
        /// 登录
        /// </summary>
        SIGN_IN,

        /// <summary>
        /// 退出
        /// </summary>
        SIGN_OUT,

        /// <summary>
        /// 聊天
        /// </summary>
        CHAT,

        /// <summary>
        /// 文件
        /// </summary>
        FILE
    }
}
