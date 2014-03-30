using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Client
{
    public partial class UserRegister : Form
    {
        public UserRegister()
        {
            InitializeComponent();
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            string userName = txtUserName.Text.Trim();
            string password = txtPassword.Text.Trim();
            string repeatPassword = txtRepeatPassword.Text.Trim();
            string mail = txtMail.Text.Trim();

            // 验证用户输入的信息是否合法
            if (VerifyUserInfo.Verify(userName, password, mail) == false)
            {
                MessageBox.Show("请检查输入的信息！", "信息不合法：");
                return;
            }
            else
            {
                // TODO:判断用户是否已经存在

                if (password.Equals(repeatPassword))
                {
                    UserInfo user = new UserInfo()
                    {
                        Name = userName,
                        Password = password,
                        Email = mail
                    };
                    OperateUserInfo operate = new OperateUserInfo();
                    bool record = operate.AddUserInfo(user);
                    if (record == true)
                    {
                        MessageBox.Show("注册成功！");
                    }
                }
                else
                    MessageBox.Show("密码不一致！", "错误：", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
