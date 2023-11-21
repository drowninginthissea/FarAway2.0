using FarAway2._0.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace FarAway2._0.Tools
{
    internal static class DbUtils
    {
        public static Db db;
        static DbUtils()
        {
            try
            {
                db = new Db();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка подключения к базе данных.\n{ex.Message}");
            }
        }
        public static bool Authorization(string login, string password)
        {
            if (!db.Database.CanConnect())
            {
                MessageBox.Show("something");
                return false;
            }
            HashService LogginPassword = new HashService(password);
            Users user = db.Users
                .FirstOrDefault(x => x.idRole != Entities.Enums.Roles.Client && x.Login == login);
            return user != null ? LogginPassword.VerifyWithThis(user.Password) : false;
        }
    }
}
