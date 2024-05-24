using System.Security.Cryptography;
using System.Text;

namespace FarAwayClient.Services
{
    public class HashService
    {
        private string _sourcePassword;
        public string SourcePassword
        {
            get { return _sourcePassword; }
            set
            {
                _sourcePassword = value;
                EncrypredPassword = Encrypt(SourcePassword);
            }
        }
        public string EncrypredPassword { get; private set; }
        public void SetPassword(string InputtedPassword)
        {
            SourcePassword = InputtedPassword;
        }
        private byte[] SaltPlusPassword(string SourcePassword, byte[] Salt)
        {
            byte[] PasswordBytes = Encoding.UTF8.GetBytes(SourcePassword);
            byte[] CombineBytes = new byte[PasswordBytes.Length + Salt.Length];
            Array.Copy(Salt, 0, CombineBytes, 0, Salt.Length);
            Array.Copy(PasswordBytes, 0, CombineBytes, Salt.Length, PasswordBytes.Length);
            return CombineBytes;
        }
        private string Encrypt(string UnencryptedPassword)
        {
            byte[] Salt = new byte[16];
            new RNGCryptoServiceProvider().GetBytes(Salt);

            byte[] CombinedBytes = SaltPlusPassword(UnencryptedPassword, Salt);

            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] HashedBytes = sha256.ComputeHash(CombinedBytes);

                return $"{Convert.ToBase64String(Salt)}:{Convert.ToBase64String(HashedBytes)}";
            }
        }
        private bool VerifyPassword(string InputPassword, string StoredHashPassword)
        {
            string[] Parts = StoredHashPassword.Split(":");
            if (Parts.Length != 2)
                return false;

            byte[] StoredSalt = Convert.FromBase64String(Parts[0]); // извлекаем соль
            string StoredHash = Parts[1];

            byte[] InputtedPlusSalt = SaltPlusPassword(InputPassword, StoredSalt);
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] HashedInputtedPassword = sha256.ComputeHash(InputtedPlusSalt);

                string HashedString = Convert.ToBase64String(HashedInputtedPassword);
                return HashedString == StoredHash;
            }
        }
        public bool VerifyWithThis(string PassForComparison) =>
            VerifyPassword(SourcePassword, PassForComparison);
    }
}
