namespace WinFormsApp1
{
    public partial class Login : Form
    {
        static public string Loginactive;
        static public string whoIS;
        public Login()
        {
            InitializeComponent();
        }

        private void Login_Load(object sender, EventArgs e)
        {
            DB.ConnectionBd();
            PassTxt.PasswordChar = '*';
            Pb_eye.Visible = false;
            LoginTxt.MaxLength = 50;
            PassTxt.MaxLength = 50;


        }

        private void AuthorizationBtn_Click(object sender, EventArgs e)
        {
            if (LoginTxt.Text != "" && PassTxt.Text != "")
            {
                Authorization.Authorizations(LoginTxt.Text, PassTxt.Text);

                switch (Authorization.Role)
                {
                    case null:
                        {
                            MessageBox.Show("Такого аккаунта не существует!", "Проверьте данные и попробуйте снова!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            break;
                        }
                    case "Admin":
                        {
                            Loginactive = LoginTxt.Text;
                            whoIS = "Администратор";
                            Authorization.User = LoginTxt.Text;
                            string user = Authorization.AuthorizationsName(LoginTxt.Text);
                            Authorization.User = user;
                            MessageBox.Show(user + ", Добро пожаловать в меню Администратора!", "Успешно!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            this.Hide();
                            MainFormsAdmin mainFormsAdmin = new MainFormsAdmin();
                            mainFormsAdmin.Show();
                            break;
                        }
                    case "Users":
                        {
                            Loginactive = LoginTxt.Text;
                            whoIS = "Пользователь";
                            Authorization.User = LoginTxt.Text;
                            string user = Authorization.AuthorizationsName(LoginTxt.Text);
                            Authorization.User = user;
                            MessageBox.Show(user + ", Добро пожаловать в меню Пользователя!", "Успешно!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            this.Hide();
                            MainFormsUser mainFormsUser = new MainFormsUser();
                            mainFormsUser.Show();
                            break;
                        }
                }
            }
            else
            {
                MessageBox.Show("Поля пустые..", "Заполните поля", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void Pb_hide_Click(object sender, EventArgs e)
        {
            PassTxt.UseSystemPasswordChar = true;
            Pb_eye.Visible = true;
            Pb_hide.Visible = false;
           
        }

        private void Pb_eye_Click(object sender, EventArgs e)
        {
            PassTxt.UseSystemPasswordChar = false;
            Pb_eye.Visible = false;
            Pb_hide.Visible = true;
            
        }
    }
}
