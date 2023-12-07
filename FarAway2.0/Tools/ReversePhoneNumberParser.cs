using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarAway2._0.Tools
{
    /// <summary>
    /// Incoming phone number format:71234567890
    /// Necessary phone number format to out:+7(123)456-78-90
    /// </summary>
    internal class ReversePhoneNumberParser
    {
        private string _originalPhoneNumber;
        public string OriginalPhoneNumber
        {
            get
            {
                return _originalPhoneNumber;
            }
            private set
            {
                _originalPhoneNumber = value;
                ParsedPhoneNumber = ParseNumber();
            }
        }
        public string ParsedPhoneNumber { get; private set; }
        public ReversePhoneNumberParser(string PhoneNumber) { OriginalPhoneNumber = PhoneNumber; }
        private string ParseNumber()
        {
            return $"+{OriginalPhoneNumber.Substring(0, 1)}({OriginalPhoneNumber.Substring(1, 3)}){OriginalPhoneNumber.Substring(4, 3)}-{OriginalPhoneNumber.Substring(7, 2)}-{OriginalPhoneNumber.Substring(9)}";
        }
    }
}
