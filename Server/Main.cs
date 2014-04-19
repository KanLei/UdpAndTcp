using System;
using System.Windows.Forms;

namespace Server
{
    public partial class ServerForm : Form
    {
        public ServerForm()
        {
            InitializeComponent();

            richTextBoxUserRecords.Visible = false;
            btnStop.Enabled = false;

            /* 注册监听事件 */
            UdpUtils.Server.SignInNotify += ReceiveMessage;
            UdpUtils.Server.SignOutNotify += ReceiveMessage;

            
        }

        /// <summary>
        /// 接收客户端的请求，并显示请求内容
        /// </summary>
        private void ReceiveMessage(UdpUtils.Message message)
        {
            this.Invoke(new MethodInvoker(() =>
            {
                richTextBoxUserRecords.AppendText(string.Format("{0}:{1}-{2}-{3}",
                    message.Type, message.FromUserName, message.IpAddress, message.Port));
                richTextBoxUserRecords.AppendText(Environment.NewLine);
            }));
        }


        #region ButtonClick

        /// <summary>
        /// Start listen
        /// </summary>
        private void btnStart_Click(object sender, EventArgs e)
        {
            btnStart.Enabled = false;
            btnStop.Enabled = true;
            txtIPAddress.Enabled = false;
            txtPort.Enabled = false;
            ShowOnlineUsers();

            /* 启动服务器 */
            UdpUtils.Server.StartServer(txtIPAddress.Text, Convert.ToInt32(txtPort.Text));

            richTextBoxUserRecords.AppendText("Server is starting...\r\n");
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            btnStop.Enabled = false;
            btnStart.Enabled = true;
            txtIPAddress.Enabled = true;
            txtPort.Enabled = true;
            richTextBoxUserRecords.AppendText("Server is stoped...\r\n");
        }

        /// <summary>
        ///  prevent double-click maximum
        /// </summary>
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0xa3)
            {
                return;
            }
            base.WndProc(ref m);
        }

        private void btnConfig_Click(object sender, EventArgs e)
        {
            btnConfig.Dock = DockStyle.Top;
            btnOnlineUser.Dock = DockStyle.Bottom;
            richTextBoxUserRecords.Visible = false;
            groupBox1.Visible = true;
        }

        private void btnOnlineUser_Click(object sender, EventArgs e)
        {
            ShowOnlineUsers();
        }

        /// <summary>
        /// Display online users
        /// </summary>
        private void ShowOnlineUsers()
        {
            btnConfig.Dock = DockStyle.Top;
            btnOnlineUser.BringToFront();
            btnOnlineUser.Dock = DockStyle.Top;
            groupBox1.Visible = false;
            richTextBoxUserRecords.Visible = true;
        }
        #endregion
    }
}
