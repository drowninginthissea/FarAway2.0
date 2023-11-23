using System.Text.RegularExpressions;

namespace FarAway2._0.Tools
{
    /// <summary>
    /// Phone number format from frontend:+7(123)456-78-90
    /// Necessary phone number format to database:71234567890
    /// </summary>
    internal class PhoneNumberParser
    {
        private string _originalPhoneNumber;
        public string OriginalPhoneNumber
        {
            private set
            {
                _originalPhoneNumber = value;
                ParsedPhoneNumber = ParseNumber();
            }
            get
            {
                return _originalPhoneNumber;
            }
        }
        public string ParsedPhoneNumber { get; private set; }
        public PhoneNumberParser(string PhoneNumber) => OriginalPhoneNumber = PhoneNumber;
        private string ParseNumber() => Regex.Replace(_originalPhoneNumber, @"[^\d]", "");
    }
}