using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace EventrixAPI.Utilidades
{
    public class FormatoCedulaCorrectoAttribute: ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if(value == null || string.IsNullOrEmpty(value.ToString()))
            {
                return ValidationResult.Success;
            }

            var formatoGuion = Regex.IsMatch(value.ToString(), "^[0-9][0-9][0-9]-[0-9][0-9][0-9][0-9][0-9][0-9][0-9]-[0-9]$");
            var formatoNormal = Regex.IsMatch(value.ToString(), "^[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]$");

            if (!(formatoGuion || formatoNormal))
            {
                return new ValidationResult("Formato de cedula incorrecto");
            }

            return ValidationResult.Success;
        }
    }
}
