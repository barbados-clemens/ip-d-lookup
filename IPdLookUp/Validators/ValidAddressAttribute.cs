using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace IPdLookUp.Validators
{
    public class ValidAddressAttribute : ValidationAttribute
    {
        public static Regex IPv4 =
            new Regex(
                @"/\b(?:(?:2(?:[0-4][0-9]|5[0-5])|[0-1]?[0-9]?[0-9])\.){3}(?:(?:2([0-4][0-9]|5[0-5])|[0-1]?[0-9]?[0-9]))\b/ig");

        public static Regex DomainName = new Regex(@"^(ht|f)tp(s?)\:\/\/[0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*(:(0-9)*)*(\/?)([a-zA-Z0-9\-\.\?\,\'\/\\\+&%\$#_]*)?$");
        
        public static Regex Protocol = new Regex(@"^(ht|f)tp(s?)\:\/\/");

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var parsedVal = value.ToString();

            if (IPv4.IsMatch(parsedVal))
                return null;

            if (DomainName.IsMatch(parsedVal))
                return null;

            return new ValidationResult(
                $"Invalid address passed in. Expected {value} to be a valid IPv4 Address or Domain.");
        }
    }
}