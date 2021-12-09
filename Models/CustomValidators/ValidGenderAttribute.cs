using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

public class ValidGenderAttribute : ValidationAttribute
{
    private enum _genders{ Masculino, Feminino, Outros };
    public string GetErrorMessage() =>
        "Gênero inválido, escolha entre Masculino, Feminino ou Outros.";

    protected override ValidationResult IsValid(object value,
        ValidationContext validationContext)
    {
        //Enum.GetNames(typeof(_genders)).Contains((string)value);

        foreach(string gender in Enum.GetNames(typeof(_genders)))
        {
            if ( gender == ((string)value))
            {
                return ValidationResult.Success;
            }
        }

        return new ValidationResult(GetErrorMessage());
    }
}