namespace Client
{
    partial class ChatForm
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
            this.richTextBoxChat = new System.Windows.Forms.RichTextBox();
            this.txtMessage = new System.Windows.Forms.TextBox();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnSend = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lblPeerPort = new System.Windows.Forms.Label();
            this.lblPeerIP = new System.Windows.Forms.Label();
            this.lblPeerName = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblReceive = new System.Windows.Forms.Label();
            this.lblSend = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // richTextBoxChat
            // 
            this.richTextBoxChat.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBoxChat.Font = new System.Drawing.Font("SimHei", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.richTextBoxChat.Location = new System.Drawing.Point(12, 30);
            this.richTextBoxChat.Name = "richTextBoxChat";
            this.richTextBoxChat.ReadOnly = true;
            this.richTextBoxChat.Size = new System.Drawing.Size(218, 208);
            this.richTextBoxChat.TabIndex = 3;
            this.richTextBoxChat.Text = "";
            // 
            // txtMessage
            // 
            this.txtMessage.AllowDrop = true;
            this.txtMessage.Location = new System.Drawing.Point(13, 247);
            this.txtMessage.Multiline = true;
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.Size = new System.Drawing.Size(217, 54);
            this.txtMessage.TabIndex = 0;
            this.txtMessage.DragDrop += new System.Windows.Forms.DragEventHandler(this.txtMessage_DragDrop);
            this.txtMessage.DragEnter += new System.Windows.Forms.DragEventHandler(this.txtMessage_DragEnter);
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(322, 272);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(75, 25);
            this.btnClear.TabIndex = 1;
            this.btnClear.Text = "清空";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(236, 272);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(75, 25);
            this.btnSend.TabIndex = 2;
            this.btnSend.Text = "发送";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "聊天纪录：";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.lblPeerPort);
            this.groupBox1.Controls.Add(this.lblPeerIP);
            this.groupBox1.Controls.Add(this.lblPeerName);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(247, 21);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(154, 95);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "对方的资料";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(11, 68);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(29, 13);
            this.label7.TabIndex = 2;
            this.label7.Text = "Port:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(13, 46);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(20, 13);
            this.label5.TabIndex = 2;
            this.label5.Text = "IP:";
            // 
            // lblPeerPort
            // 
            this.lblPeerPort.AutoSize = true;
            this.lblPeerPort.Location = new System.Drawing.Point(53, 68);
            this.lblPeerPort.Name = "lblPeerPort";
            this.lblPeerPort.Size = new System.Drawing.Size(35, 13);
            this.lblPeerPort.TabIndex = 1;
            this.lblPeerPort.Text = "label4";
            // 
            // lblPeerIP
            // 
            this.lblPeerIP.AutoSize = true;
            this.lblPeerIP.Location = new System.Drawing.Point(53, 46);
            this.lblPeerIP.Name = "lblPeerIP";
            this.lblPeerIP.Size = new System.Drawing.Size(35, 13);
            this.lblPeerIP.TabIndex = 1;
            this.lblPeerIP.Text = "label4";
            // 
            // lblPeerName
            // 
            this.lblPeerName.AutoSize = true;
            this.lblPeerName.Location = new System.Drawing.Point(53, 22);
            this.lblPeerName.Name = "lblPeerName";
            this.lblPeerName.Size = new System.Drawing.Size(35, 13);
            this.lblPeerName.TabIndex = 1;
            this.lblPeerName.Text = "label4";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Name:";
            // 
            // lblReceive
            // 
            this.lblReceive.AutoSize = true;
            this.lblReceive.Location = new System.Drawing.Point(261, 145);
            this.lblReceive.Name = "lblReceive";
            this.lblReceive.Size = new System.Drawing.Size(0, 13);
            this.lblReceive.TabIndex = 5;
            // 
            // lblSend
            // 
            this.lblSend.AutoSize = true;
            this.lblSend.Location = new System.Drawing.Point(261, 181);
            this.lblSend.Name = "lblSend";
            this.lblSend.Size = new System.Drawing.Size(0, 13);
            this.lblSend.TabIndex = 6;
            // 
            // ChatForm
            // 
            this.AcceptButton = this.btnSend;
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Teal;
            this.ClientSize = new System.Drawing.Size(403, 310);
            this.Controls.Add(this.lblSend);
            this.Controls.Add(this.lblReceive);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.txtMessage);
            this.Controls.Add(this.richTextBoxChat);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "ChatForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ChatForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ChatForm_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBoxChat;
        private System.Windows.Forms.TextBox txtMessage;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblPeerPort;
        private System.Windows.Forms.Label lblPeerIP;
        private System.Windows.Forms.Label lblPeerName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblReceive;
        private System.Windows.Forms.Label lblSend;
    }
}