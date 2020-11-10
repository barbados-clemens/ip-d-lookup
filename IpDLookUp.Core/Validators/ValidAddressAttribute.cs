using System.ComponentModel.DataAnnotations;

namespace IPdLookUp.Core.Validators
{
    /// <summary>
    /// Custom validation attribute for validating an address to a domain or IPv4 address.
    /// </summary>
    public class ValidAddressAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object value, ValidationContext validationContext)
        {
            var parsedVal = value.ToString() ?? string.Empty;

            // make sure protocol isn't in the address
            if (IpDLookUp.Services.Models.Validators.Protocol.IsMatch(parsedVal))
                return new ValidationResult($"Value does should not contain the protocol");


            if (IpDLookUp.Services.Models.Validators.IPv4.IsMatch(parsedVal))
                return null;

            if (IpDLookUp.Services.Models.Validators.DomainName.IsMatch(parsedVal))
                return null;

            return new ValidationResult(
                $"Invalid address passed in. Expected {value} to be a valid IPv4 Address or Domain Name (without protocol).");
        }
    }
}