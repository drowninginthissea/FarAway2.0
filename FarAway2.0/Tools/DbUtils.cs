using FarAway2._0.Entities;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace FarAway2._0.Tools
{
    //нужно переделать в синглтон
    internal static class DbUtils
    {
        public static Db db;
        static DbUtils()
        {
            try
            {
                db = new Db();
                CheckConnection();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка подключения к базе данных.\n{ex.Message}", "Ошибка");
            }
        }
        private static async void CheckConnection()
        {
            bool canConnect = db.Database.CanConnect();
            if (!canConnect)
            {
                _ = MessageBox.Show($"Ошибка подключения к базе данных.\n" +
                    $"Обратитесь к администратору компании для устранения проблемы.\n" +
                    $"Дальнейшая работа программного обеспечения не может быть выполнена.\n" +
                    $"После принятия данного соглашения последует остановка программы!", "Ошибка", MessageBoxButton.OK);
                await Application.Current.Dispatcher.InvokeAsync(Application.Current.Shutdown);
            }
        }
        public static bool Authorization(string login, string password)
        {
            HashService LogginPassword = new HashService(password);
            Users user = db.Users
                .FirstOrDefault(x => x.idRole != Entities.Enums.Roles.Client && x.Login == login);
            return user != null ? LogginPassword.VerifyWithThis(user.Password) : false;
        }
        public static async Task<bool> RegistrationAsync(Users user)
        {
            EntityEntry result = await db.Users.AddAsync(user);
            db.SaveChanges();
            return result.State == Microsoft.EntityFrameworkCore.EntityState.Added ? true : false;
        }
    }
}
