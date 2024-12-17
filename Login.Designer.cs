namespace WinFormsApp1
{
    partial class Login
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            AuthorizationBtn = new Button();
            pictureBox1 = new PictureBox();
            LoginTxt = new TextBox();
            label1 = new Label();
            label2 = new Label();
            Pb_hide = new PictureBox();
            Pb_eye = new PictureBox();
            PassTxt = new TextBox();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)Pb_hide).BeginInit();
            ((System.ComponentModel.ISupportInitialize)Pb_eye).BeginInit();
            SuspendLayout();
            // 
            // AuthorizationBtn
            // 
            AuthorizationBtn.BackColor = Color.FromArgb(255, 192, 128);
            AuthorizationBtn.Cursor = Cursors.Hand;
            AuthorizationBtn.Font = new Font("Times New Roman", 21.75F, FontStyle.Bold, GraphicsUnit.Point);
            AuthorizationBtn.Location = new Point(91, 398);
            AuthorizationBtn.Name = "AuthorizationBtn";
            AuthorizationBtn.Size = new Size(183, 59);
            AuthorizationBtn.TabIndex = 0;
            AuthorizationBtn.Text = "Вход";
            AuthorizationBtn.UseVisualStyleBackColor = false;
            AuthorizationBtn.Click += AuthorizationBtn_Click;
            // 
            // pictureBox1
            // 
            pictureBox1.BackColor = Color.Black;
            pictureBox1.Image = Properties.Resources.img_1;
            pictureBox1.Location = new Point(102, 12);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(160, 135);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 1;
            pictureBox1.TabStop = false;
            // 
            // LoginTxt
            // 
            LoginTxt.Font = new Font("Times New Roman", 24F, FontStyle.Bold, GraphicsUnit.Point);
            LoginTxt.Location = new Point(59, 217);
            LoginTxt.Multiline = true;
            LoginTxt.Name = "LoginTxt";
            LoginTxt.Size = new Size(249, 50);
            LoginTxt.TabIndex = 2;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(149, 179);
            label1.Name = "label1";
            label1.Size = new Size(70, 24);
            label1.TabIndex = 4;
            label1.Text = "Логин";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(149, 285);
            label2.Name = "label2";
            label2.Size = new Size(82, 24);
            label2.TabIndex = 5;
            label2.Text = "Пароль";
            // 
            // Pb_hide
            // 
            Pb_hide.Image = Properties.Resources.hide;
            Pb_hide.Location = new Point(317, 321);
            Pb_hide.Name = "Pb_hide";
            Pb_hide.Size = new Size(45, 44);
            Pb_hide.SizeMode = PictureBoxSizeMode.StretchImage;
            Pb_hide.TabIndex = 6;
            Pb_hide.TabStop = false;
            Pb_hide.Click += Pb_hide_Click;
            // 
            // Pb_eye
            // 
            Pb_eye.Image = Properties.Resources.eye;
            Pb_eye.Location = new Point(317, 321);
            Pb_eye.Name = "Pb_eye";
            Pb_eye.Size = new Size(45, 44);
            Pb_eye.SizeMode = PictureBoxSizeMode.StretchImage;
            Pb_eye.TabIndex = 7;
            Pb_eye.TabStop = false;
            Pb_eye.Click += Pb_eye_Click;
            // 
            // PassTxt
            // 
            PassTxt.Font = new Font("Times New Roman", 24F, FontStyle.Bold, GraphicsUnit.Point);
            PassTxt.Location = new Point(59, 321);
            PassTxt.Multiline = true;
            PassTxt.Name = "PassTxt";
            PassTxt.Size = new Size(249, 50);
            PassTxt.TabIndex = 8;
            // 
            // Login
            // 
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.Silver;
            ClientSize = new Size(374, 469);
            Controls.Add(PassTxt);
            Controls.Add(Pb_hide);
            Controls.Add(Pb_eye);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(LoginTxt);
            Controls.Add(pictureBox1);
            Controls.Add(AuthorizationBtn);
            DoubleBuffered = true;
            Font = new Font("Times New Roman", 15.75F, FontStyle.Bold, GraphicsUnit.Point);
            MaximizeBox = false;
            Name = "Login";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Окно входа";
            Load += Login_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)Pb_hide).EndInit();
            ((System.ComponentModel.ISupportInitialize)Pb_eye).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button AuthorizationBtn;
        private PictureBox pictureBox1;
        private TextBox LoginTxt;
        private Label label1;
        private Label label2;
        private PictureBox Pb_hide;
        private PictureBox Pb_eye;
        private TextBox PassTxt;
    }
}
