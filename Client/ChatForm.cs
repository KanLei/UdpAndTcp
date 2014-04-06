using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Media;
using System.IO;
using System.Text.RegularExpressions;

namespace Client
{
    public partial class ChatForm : Form
    {
        private UdpUtils.Message OtherInfo;
        private UdpUtils.Message PersonalInfo;
        StringBuilder chatContent = new StringBuilder();

        public ChatForm(UdpUtils.Message otherInfo, UdpUtils.Message personalInfo)
        {
            InitializeComponent();

            // 注册收到文件提示事件
            UdpUtils.Client.ReceiveFileProgressNotify += ReceiveFile;

            OtherInfo = otherInfo;
            PersonalInfo = personalInfo;

            ShowUserInfo();
        }

        /// <summary>
        /// Display User Information
        /// </summary>
        public void ShowUserInfo()
        {
            #region  display other's information
            lblPeerName.Text = OtherInfo.FromUserName;
            lblPeerIP.Text = OtherInfo.IpAddress;
            lblPeerPort.Text = OtherInfo.Port.ToString();
            #endregion


            this.Text = string.Format("{0}:{1}", PersonalInfo.FromUserName, OtherInfo.FromUserName);
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            if (txtMessage.Text.Trim() == "")
            {
                SystemSounds.Exclamation.Play();
                this.ShakeForm();
            }
            else
            {
                sendMessage(txtMessage.Text);
            }
        }

        /// <summary>
        /// Send message to others
        /// </summary>
        /// <param name="msg"></param>
        public void sendMessage(string msg)
        {
            string sendMsg = string.Format("{0}:{1}\r\n", PersonalInfo.FromUserName, msg);
            richTextBoxChat.AppendText(sendMsg);
            richTextBoxChat.ScrollToCaret();
            chatContent.Append(sendMsg);

            UdpUtils.Message message = new UdpUtils.Message
            {
                FromUserName = PersonalInfo.FromUserName,
                ToUserName = lblPeerName.Text,
                Type = UdpUtils.MessageEnum.CHAT,
                Content = msg,
                SendTime = DateTime.Now
            };

            UdpUtils.Client.SendToClientAsync(lblPeerIP.Text, Convert.ToInt32(lblPeerPort.Text), message);
        }


        /// <summary>
        /// Receive and display message
        /// </summary>
        public void ShowNewMsg(UdpUtils.Message msg)
        {
            string data = string.Format("{0}:{1}\r\n", msg.FromUserName, msg.Content);

            this.Invoke(new MethodInvoker(() =>
            {
                richTextBoxChat.SelectionColor = Color.Purple;
                richTextBoxChat.AppendText(data);
                richTextBoxChat.ScrollToCaret();
                this.Activate();
            }));
            chatContent.Append(data);
        }

        /// <summary>
        /// 接收客户端发来的文件
        /// </summary>
        public void ReceiveFile(string percent)
        {
            this.Invoke(new MethodInvoker(() =>
            {
                lblReceive.Text = String.Format("正在接收文件...{0}", percent);
            }));
        }


        private void txtMessage_DragDrop(object sender, DragEventArgs e)
        {
            // 获取文件的路径
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            StringBuilder sb = new StringBuilder();
            foreach (string filePath in files)
            {
                sb.Append(filePath);
            }

            UdpUtils.Message message = new UdpUtils.Message
            {
                FromUserName = PersonalInfo.FromUserName,
                ToUserName = lblPeerName.Text,
                Type = UdpUtils.MessageEnum.FILE,
                SendTime = DateTime.Now
            };

            UdpUtils.Client.SendFileToClient(lblPeerIP.Text, Convert.ToInt32(lblPeerPort.Text), sb.ToString(), message);
        }

        private void txtMessage_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.All;
            else
                e.Effect = DragDropEffects.None;
        }

        /// <summary>
        /// Clear the input box
        /// </summary>
        private void btnClear_Click(object sender, EventArgs e)
        {
            txtMessage.Text = "";
            txtMessage.Focus();
        }


        /// <summary>
        /// prevent double-click maximum form
        /// </summary>
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0xa3)
            {
                return;
            }
            base.WndProc(ref m);
        }


        /// <summary>
        /// save chat content
        /// </summary>
        private void ChatForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            StreamWriter streamWrite;
            if (!File.Exists("Records.txt"))
            {
                using (FileStream fileStream = new FileStream("Records.txt", FileMode.Create, FileAccess.Write))
                {
                    streamWrite = new StreamWriter(fileStream, Encoding.Default);
                    streamWrite.WriteLine(chatContent);
                    streamWrite.Close();
                }
            }
            else
            {
                using (FileStream fileStream = new FileStream("Records.txt", FileMode.Append, FileAccess.Write))
                {
                    streamWrite = new StreamWriter(fileStream, Encoding.Default);
                    streamWrite.WriteLine(chatContent);
                    streamWrite.Close();
                }
            }
        }



    }
}
