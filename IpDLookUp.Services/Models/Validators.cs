using System.Text.RegularExpressions;

namespace IpDLookUp.Services.Models
{
    /// <summary>
    /// Regex patterns for validation of addresses
    /// </summary>
    public static class Validators
    {
        // ReSharper disable once InconsistentNaming
        public static readonly Regex IPv4 =
            new Regex(
                @"\b(?:(?:2(?:[0-4][0-9]|5[0-5])|[0-1]?[0-9]?[0-9])\.){3}(?:(?:2([0-4][0-9]|5[0-5])|[0-1]?[0-9]?[0-9]))\b");

        public static readonly Regex DomainName =
            new Regex(@"^[0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*(:(0-9)*)*(\/?)([a-zA-Z0-9\-\.\?\,\'\/\\\+&%\$#_]*)?$");

        public static readonly Regex Protocol = new Regex(@"^(ht|f)tp(s?)\:\/\/");

    }
}