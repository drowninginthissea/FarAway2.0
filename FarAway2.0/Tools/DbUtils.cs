using FarAway2._0.Entities;
using FarAway2._0.Exceptions;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Linq;
using System.Windows;

namespace FarAway2._0.Tools
{
    public class DbUtils
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
        public static (bool, Users?) Authorization(string login, string password)
        {
            HashService LogginPassword = new HashService(password);
            Users? user = db.Users
                .FirstOrDefault(x => x.idRole != Entities.Enums.Roles.Client && x.Login == login);
            if (user == null)
                return (false, null);
            bool IsRightPassword = LogginPassword.VerifyWithThis(user.Password);
            if (!IsRightPassword)
                return (false, user);
            return (true, user);
        }
        /// <summary>
        /// Can throw an exception "UserAlreadyExistsException"
        /// </summary>
        public static bool Registration(Users user)
        {
            if (db.Users.Any(u => u.Login == user.Login))
                throw new UserAlreadyExistsException("Пользователь с таким логином уже существует!");
            try
            {
                db.Users.Add(user);
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        private static string GetStringOrEmpty(string Source) => Source != null ? Source : "";
        private static string GetFirstCharWithPoint(string Source) => Source == "" ? "" : $"{Source.First()}.";
        public static string GetUserInitials(Users user) => $"{user.Surname} " +
                $"{GetFirstCharWithPoint(user.Name)}" +
                $"{GetFirstCharWithPoint(GetStringOrEmpty(user.Patronymic))}";

    }
}
