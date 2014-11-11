using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Media;
using System.Threading;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Client
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();

            /* 注册客户端监听事件 */
            UdpUtils.Client.SignInNotify += ProcessMessage;
            UdpUtils.Client.SignOutNotify += ProcessMessage;
            UdpUtils.Client.ReceiveMessageNotify += ProcessMessage;
        }

        List<ChatForm> chatFormList = new List<ChatForm>();
        UserInfo userInfo;
        private const string note = "Client is starting...";

        /// <summary>
        /// login
        /// </summary>
        private void btnLogin_Click(object sender, EventArgs e)
        {
            string userName = txtUserName.Text.Trim();
            string password = txtPassword.Text.Trim();
            if (userName.IsNullOrWhiteSpace() || password.IsNullOrWhiteSpace())
            {
                this.ShakeForm();
                txtUserName.Text = txtPassword.Text = "";
                txtUserName.Focus();
                SystemSounds.Exclamation.Play();
                return;
            }

            lblShowConnMsg.Visible = pictureBox1.Visible = true;
            userInfo = new UserInfo { Name = userName, Password = password };
            Func<int> myFun = () =>
            {
                // validate input user info
                OperateUserInfo operate = new OperateUserInfo();
                Thread.Sleep(2000);
                return operate.ValidateUserInfo(userInfo);
            };
            myFun.BeginInvoke(CallBackMethod, myFun);
        }

        private void CallBackMethod(IAsyncResult target)
        {
            Func<int> myFun = (Func<int>)target.AsyncState;
            int record = myFun.EndInvoke(target);
            if (record > 0)
            {
                this.Invoke(new MethodInvoker(() =>
                {
                    panelLogin.Visible = false;  // hide panelLogin
                    this.Text = string.Format("{0}", userInfo.Name);
                }));
            }
            else
            {
                this.Invoke(new MethodInvoker(() =>
                {
                    this.ShakeForm();
                    txtUserName.Text = txtPassword.Text = "";
                    txtUserName.Focus();
                    SystemSounds.Exclamation.Play();
                    lblShowConnMsg.Visible = pictureBox1.Visible = false;
                }));
            }
        }

        /// <summary>
        /// register
        /// </summary>
        private void btnRegister_Click(object sender, EventArgs e)
        {
            UserRegister register = new UserRegister();
            register.ShowDialog();
        }

        /// <summary>
        /// Connect to the server
        /// </summary>
        private async void btnConnection_Click(object sender, EventArgs e)
        {
            btnConnection.Enabled = txtIPAddress.Enabled = txtPort.Enabled = false;

            UdpUtils.Message message = new UdpUtils.Message() { FromUserName = this.Text, Type = UdpUtils.MessageEnum.SIGN_IN };
            await UdpUtils.Client.SendToServerAsync(txtIPAddress.Text, Convert.ToInt32(txtPort.Text), message);

            this.Invoke(new MethodInvoker(() => { listViewOnlineUsers.Items.Add(note + "\r\n"); }));
        }


        /// <summary>
        /// process receive response message
        /// </summary>
        private void ProcessMessage(UdpUtils.Message msg)
        {
            switch (msg.Type)
            {
                case UdpUtils.MessageEnum.SIGN_IN:
                    this.Invoke(new MethodInvoker(() =>
                    {
                        listViewOnlineUsers.Items.Add(string.Format("{0}-{1}:{2}", msg.FromUserName, msg.IpAddress, msg.Port));
                    }));
                    break;
                case UdpUtils.MessageEnum.SIGN_OUT:
                    this.Invoke(new MethodInvoker(() =>
                    {
                        for (int i = 0; i < listViewOnlineUsers.Items.Count; i++)
                        {
                            if (listViewOnlineUsers.Items[i].Text.Trim() == string.Format("{0}-{1}:{2}", msg.FromUserName, msg.IpAddress, msg.Port))
                            {
                                listViewOnlineUsers.Items.RemoveAt(i);
                                break;
                            }
                        }
                    }));
                    break;
                case UdpUtils.MessageEnum.CHAT:
                    string message = string.Format("{0}:{1}", msg.ToUserName, msg.FromUserName);
                    ChatForm form = chatFormList.Find(chatForm => chatForm.Text.Trim() == message.Trim());
                    if (form != null)
                    {
                        form.ShowNewMsg(msg);
                    }
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// close form
        /// </summary>
        private async void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            UdpUtils.Message message = new UdpUtils.Message() { FromUserName = this.Text, Type = UdpUtils.MessageEnum.SIGN_OUT };
            await UdpUtils.Client.SendToServerAsync(txtIPAddress.Text, Convert.ToInt32(txtPort.Text), message);
        }

        /// <summary>
        /// double-click the user info to chat
        /// </summary>
        private void listViewOnlineUsers_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            string otherInfo = listViewOnlineUsers.SelectedItems[0].Text.Trim();
            if (otherInfo == note) return;

            Match match = Regex.Match(otherInfo, @"^(\w+)-([0-9]{1,3}.[0-9]{1,3}.[0-9]{1,3}.[0-9]{1,3}):([0-9]+)$");

            UdpUtils.Message otherProfile = new UdpUtils.Message()
            {
                FromUserName = match.Groups[1].Value,
                IpAddress = match.Groups[2].Value,
                Port = Convert.ToInt32(match.Groups[3].Value)
            };

            if (otherInfo != note)
            {
                UdpUtils.Message myProfile = new UdpUtils.Message()
                {
                    FromUserName = this.Text,
                    IpAddress = "",
                    Port = 0
                };

                ChatForm form = chatFormList.Find(f => f.Text == string.Format("{0}:{1}", this.Text, otherProfile.FromUserName));

                if (form != null) form.Activate();
                else
                {
                    ChatForm chat = new ChatForm(otherProfile, myProfile);
                    chatFormList.Add(chat);
                    chat.Show();
                }
            }
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
    }
}
