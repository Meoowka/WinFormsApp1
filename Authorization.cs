using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp1
{
    public class Authorization
    {
        static public string Role, User;

        static public void Authorizations(string login, string passwd)
        {
            try
            {
               

                DB.sqlCommand.CommandText = @"SELECT name_role 
                              FROM sp_role 
                              INNER JOIN Users ON Users.id_role = sp_role.id_role 
                              WHERE login_user = @login AND passwd_user = @passwd";

                DB.sqlCommand.Parameters.Clear(); // Очищаем параметры перед добавлением новых.
                DB.sqlCommand.Parameters.AddWithValue("@login", login);
                DB.sqlCommand.Parameters.AddWithValue("@passwd", passwd);

                object result = DB.sqlCommand.ExecuteScalar();

                if (result != null)
                {
                    Role = result.ToString();
                    User = login;
                }
                else
                {
                    Role = null;
                }
            }
            catch
            {
                Role = User = null;
                MessageBox.Show("Ошибка при авторизации!");
            }
        }

        static public string AuthorizationsName(string login)
        {
            try
            {
                DB.sqlCommand.CommandText = @"SELECT login_user FROM Users WHERE login_user = '" + login + "'";
                Object result = DB.sqlCommand.ExecuteScalar();
                login = result.ToString();
                return login;
            }
            catch
            {
                return null;
            }
        }
    }
}
