using System.Globalization;

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
            Users? user = db.Users.FirstOrDefault(x => x.Login == login);
            if (user == null)
                return (false, null);
            if (!(user.idRole == Entities.Enums.Roles.Admin ||
                user.idRole == Entities.Enums.Roles.Controller ||
                user.idRole == Entities.Enums.Roles.Manager ||
                user.idRole == Entities.Enums.Roles.Director))
            {
                Application.Current.Dispatcher.Invoke(() => MessageBox.Show("У вас недостаточно прав для входа в систему! Обратитесь к администратору или директору!", "Ошибка входа"));
                return (false, user);
            }
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
        public static void UpdateUser(Users user,
            string Name,
            string Surname,
            string Patronymic,
            string Email,
            string SourcePhoneNumber,
            string SourcePassword,
            byte[] Photo)
        {
            user.Name = Name;
            user.Surname = Surname;
            user.Patronymic = Patronymic;
            user.Email = Email;
            user.PhoneNumber = new PhoneNumberParser(SourcePhoneNumber).ParsedPhoneNumber;
            user.Password = new HashService(SourcePassword).EncrypredPassword;
            user.Photo = Photo;
            db.SaveChanges();
        }
        public static Users GetLastUserRental(ICollection<ParkingSpaceRental> Rentals)
        {
            if (Rentals.Count == 0)
                return null;
            else
                if (Rentals.Any(r => r.RentEndDate == null))
                    return Rentals.Single(r => r.RentEndDate == null).idUserNavigation;
                else
                    return Rentals.OrderByDescending(r => r.RentEndDate).First().idUserNavigation;
        }
        public static Users GetLastUserRental(ParkingSpots spot) =>
            GetLastUserRental(spot.ParkingSpaceRental);
        private static string GetStringOrEmpty(string Source) => Source != null ? Source : "";
        private static string GetFirstCharWithPoint(string Source) => Source == "" ? "" : $"{Source.First()}.";
        public static string GetUserInitials(Users user) => $"{user.Surname} " +
                $"{GetFirstCharWithPoint(user.Name)}" +
                $"{GetFirstCharWithPoint(GetStringOrEmpty(user.Patronymic))}";
        public static string GetInitialsOfDefaultForParkingSpots(Users user) =>
            user == null ? "Аренд ещё не было" : GetUserInitials(user);
        public static bool ValidateDecimal(decimal Value, int Precision, int Scale)
        {
            var valueStr = Value.ToString(CultureInfo.InvariantCulture);
            var decimalIndex = valueStr.IndexOf('.');
            var totalDigits = valueStr.Length - (decimalIndex == -1 ? 0 : 1);
            var digitsAfterDecimal = decimalIndex == -1 ? 0 : valueStr.Length - decimalIndex - 1;

            return totalDigits <= Precision && digitsAfterDecimal == Scale;
        }
    }
}
