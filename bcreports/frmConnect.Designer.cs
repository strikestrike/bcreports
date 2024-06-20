namespace bcreports
{
    partial class frmConnect
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmConnect));
            this.CheckBoxEncrypt = new System.Windows.Forms.CheckBox();
            this.btnConnect = new System.Windows.Forms.Button();
            this.TextBoxPassword = new System.Windows.Forms.TextBox();
            this.Label5 = new System.Windows.Forms.Label();
            this.TextBoxUserName = new System.Windows.Forms.TextBox();
            this.Label4 = new System.Windows.Forms.Label();
            this.TextBoxDBName = new System.Windows.Forms.TextBox();
            this.Label3 = new System.Windows.Forms.Label();
            this.TextBoxAddress = new System.Windows.Forms.TextBox();
            this.Label2 = new System.Windows.Forms.Label();
            this.ComboBoxAuthentication = new System.Windows.Forms.ComboBox();
            this.Label1 = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // CheckBoxEncrypt
            // 
            this.CheckBoxEncrypt.AutoSize = true;
            this.CheckBoxEncrypt.Location = new System.Drawing.Point(42, 229);
            this.CheckBoxEncrypt.Name = "CheckBoxEncrypt";
            this.CheckBoxEncrypt.Size = new System.Drawing.Size(119, 17);
            this.CheckBoxEncrypt.TabIndex = 22;
            this.CheckBoxEncrypt.Text = "Encrypt Connection";
            this.CheckBoxEncrypt.UseVisualStyleBackColor = true;
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(388, 270);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(75, 23);
            this.btnConnect.TabIndex = 23;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // TextBoxPassword
            // 
            this.TextBoxPassword.Location = new System.Drawing.Point(163, 182);
            this.TextBoxPassword.Name = "TextBoxPassword";
            this.TextBoxPassword.Size = new System.Drawing.Size(399, 20);
            this.TextBoxPassword.TabIndex = 21;
            this.TextBoxPassword.UseSystemPasswordChar = true;
            // 
            // Label5
            // 
            this.Label5.AutoSize = true;
            this.Label5.Location = new System.Drawing.Point(39, 185);
            this.Label5.Name = "Label5";
            this.Label5.Size = new System.Drawing.Size(114, 13);
            this.Label5.TabIndex = 20;
            this.Label5.Text = "SQL Server Password:";
            // 
            // TextBoxUserName
            // 
            this.TextBoxUserName.Location = new System.Drawing.Point(163, 144);
            this.TextBoxUserName.Name = "TextBoxUserName";
            this.TextBoxUserName.Size = new System.Drawing.Size(399, 20);
            this.TextBoxUserName.TabIndex = 19;
            // 
            // Label4
            // 
            this.Label4.AutoSize = true;
            this.Label4.Location = new System.Drawing.Point(39, 147);
            this.Label4.Name = "Label4";
            this.Label4.Size = new System.Drawing.Size(118, 13);
            this.Label4.TabIndex = 18;
            this.Label4.Text = "SQL Server UserName:";
            // 
            // TextBoxDBName
            // 
            this.TextBoxDBName.Location = new System.Drawing.Point(163, 106);
            this.TextBoxDBName.Name = "TextBoxDBName";
            this.TextBoxDBName.Size = new System.Drawing.Size(399, 20);
            this.TextBoxDBName.TabIndex = 17;
            // 
            // Label3
            // 
            this.Label3.AutoSize = true;
            this.Label3.Location = new System.Drawing.Point(39, 109);
            this.Label3.Name = "Label3";
            this.Label3.Size = new System.Drawing.Size(114, 13);
            this.Label3.TabIndex = 16;
            this.Label3.Text = "SQL Server DB Name:";
            // 
            // TextBoxAddress
            // 
            this.TextBoxAddress.Location = new System.Drawing.Point(163, 66);
            this.TextBoxAddress.Name = "TextBoxAddress";
            this.TextBoxAddress.Size = new System.Drawing.Size(399, 20);
            this.TextBoxAddress.TabIndex = 15;
            // 
            // Label2
            // 
            this.Label2.AutoSize = true;
            this.Label2.Location = new System.Drawing.Point(39, 69);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(106, 13);
            this.Label2.TabIndex = 14;
            this.Label2.Text = "SQL Server Address:";
            // 
            // ComboBoxAuthentication
            // 
            this.ComboBoxAuthentication.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ComboBoxAuthentication.FormattingEnabled = true;
            this.ComboBoxAuthentication.Location = new System.Drawing.Point(163, 22);
            this.ComboBoxAuthentication.Name = "ComboBoxAuthentication";
            this.ComboBoxAuthentication.Size = new System.Drawing.Size(399, 21);
            this.ComboBoxAuthentication.TabIndex = 13;
            // 
            // Label1
            // 
            this.Label1.AutoSize = true;
            this.Label1.Location = new System.Drawing.Point(39, 25);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(78, 13);
            this.Label1.TabIndex = 12;
            this.Label1.Text = "Authentication:";
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(487, 270);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 24;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // frmConnect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(591, 325);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.CheckBoxEncrypt);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.TextBoxPassword);
            this.Controls.Add(this.Label5);
            this.Controls.Add(this.TextBoxUserName);
            this.Controls.Add(this.Label4);
            this.Controls.Add(this.TextBoxDBName);
            this.Controls.Add(this.Label3);
            this.Controls.Add(this.TextBoxAddress);
            this.Controls.Add(this.Label2);
            this.Controls.Add(this.ComboBoxAuthentication);
            this.Controls.Add(this.Label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "frmConnect";
            this.Text = "Database Connect";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.CheckBox CheckBoxEncrypt;
        internal System.Windows.Forms.Button btnConnect;
        internal System.Windows.Forms.TextBox TextBoxPassword;
        internal System.Windows.Forms.Label Label5;
        internal System.Windows.Forms.TextBox TextBoxUserName;
        internal System.Windows.Forms.Label Label4;
        internal System.Windows.Forms.TextBox TextBoxDBName;
        internal System.Windows.Forms.Label Label3;
        internal System.Windows.Forms.TextBox TextBoxAddress;
        internal System.Windows.Forms.Label Label2;
        internal System.Windows.Forms.ComboBox ComboBoxAuthentication;
        internal System.Windows.Forms.Label Label1;
        private System.Windows.Forms.Button btnClose;
    }
}

