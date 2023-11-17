using FarAway2._0.Entities;
using System;
using System.Linq;
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
                MessageBox.Show($"Ошибка подключения к базе данных.\n{ex}");
            }
        }

        public static bool Authorization(string login, string password)
        {
            Users user = db.Users.Where(x => x.Email == login && x.Password == password && x.idRole != null).FirstOrDefault();
            if (user == null)
                return false;
            return true;
        }
    }
}
