namespace Server
{
    partial class ServerForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ServerForm));
            this.labIPAddress = new System.Windows.Forms.Label();
            this.txtIPAddress = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.lblPort = new System.Windows.Forms.Label();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.richTextBoxUserRecords = new System.Windows.Forms.RichTextBox();
            this.btnConfig = new System.Windows.Forms.Button();
            this.btnOnlineUser = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // labIPAddress
            // 
            this.labIPAddress.AutoSize = true;
            this.labIPAddress.Location = new System.Drawing.Point(17, 29);
            this.labIPAddress.Name = "labIPAddress";
            this.labIPAddress.Size = new System.Drawing.Size(20, 13);
            this.labIPAddress.TabIndex = 0;
            this.labIPAddress.Text = "IP:";
            // 
            // txtIPAddress
            // 
            this.txtIPAddress.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtIPAddress.Location = new System.Drawing.Point(45, 25);
            this.txtIPAddress.Name = "txtIPAddress";
            this.txtIPAddress.Size = new System.Drawing.Size(96, 20);
            this.txtIPAddress.TabIndex = 2;
            this.txtIPAddress.Text = "127.0.0.1";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnStop);
            this.groupBox1.Controls.Add(this.btnStart);
            this.groupBox1.Controls.Add(this.lblPort);
            this.groupBox1.Controls.Add(this.labIPAddress);
            this.groupBox1.Controls.Add(this.txtPort);
            this.groupBox1.Controls.Add(this.txtIPAddress);
            this.groupBox1.Location = new System.Drawing.Point(12, 52);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(245, 104);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "配置信息";
            // 
            // btnStop
            // 
            this.btnStop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStop.Location = new System.Drawing.Point(153, 56);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(75, 25);
            this.btnStop.TabIndex = 3;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnStart
            // 
            this.btnStart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStart.Location = new System.Drawing.Point(153, 24);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 25);
            this.btnStart.TabIndex = 2;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // lblPort
            // 
            this.lblPort.AutoSize = true;
            this.lblPort.Location = new System.Drawing.Point(8, 62);
            this.lblPort.Name = "lblPort";
            this.lblPort.Size = new System.Drawing.Size(29, 13);
            this.lblPort.TabIndex = 1;
            this.lblPort.Text = "Port:";
            // 
            // txtPort
            // 
            this.txtPort.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPort.Location = new System.Drawing.Point(44, 59);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(97, 20);
            this.txtPort.TabIndex = 3;
            this.txtPort.Text = "8500";
            // 
            // richTextBoxUserRecords
            // 
            this.richTextBoxUserRecords.BackColor = System.Drawing.Color.Black;
            this.richTextBoxUserRecords.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBoxUserRecords.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.richTextBoxUserRecords.ForeColor = System.Drawing.Color.White;
            this.richTextBoxUserRecords.Location = new System.Drawing.Point(0, 23);
            this.richTextBoxUserRecords.Name = "richTextBoxUserRecords";
            this.richTextBoxUserRecords.ReadOnly = true;
            this.richTextBoxUserRecords.Size = new System.Drawing.Size(267, 177);
            this.richTextBoxUserRecords.TabIndex = 5;
            this.richTextBoxUserRecords.Text = "";
            // 
            // btnConfig
            // 
            this.btnConfig.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnConfig.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnConfig.Location = new System.Drawing.Point(0, 0);
            this.btnConfig.Name = "btnConfig";
            this.btnConfig.Size = new System.Drawing.Size(267, 25);
            this.btnConfig.TabIndex = 6;
            this.btnConfig.Text = "配置连接信息";
            this.btnConfig.UseVisualStyleBackColor = true;
            this.btnConfig.Click += new System.EventHandler(this.btnConfig_Click);
            // 
            // btnOnlineUser
            // 
            this.btnOnlineUser.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnOnlineUser.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOnlineUser.Location = new System.Drawing.Point(0, 200);
            this.btnOnlineUser.Name = "btnOnlineUser";
            this.btnOnlineUser.Size = new System.Drawing.Size(267, 25);
            this.btnOnlineUser.TabIndex = 7;
            this.btnOnlineUser.Text = "在线用户";
            this.btnOnlineUser.UseVisualStyleBackColor = true;
            this.btnOnlineUser.Click += new System.EventHandler(this.btnOnlineUser_Click);
            // 
            // ServerForm
            // 
            this.AcceptButton = this.btnStart;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Teal;
            this.ClientSize = new System.Drawing.Size(267, 225);
            this.Controls.Add(this.btnConfig);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.richTextBoxUserRecords);
            this.Controls.Add(this.btnOnlineUser);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ServerForm";
            this.Text = "Server";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label labIPAddress;
        private System.Windows.Forms.TextBox txtIPAddress;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Label lblPort;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.RichTextBox richTextBoxUserRecords;
        private System.Windows.Forms.Button btnConfig;
        private System.Windows.Forms.Button btnOnlineUser;
    }
}

