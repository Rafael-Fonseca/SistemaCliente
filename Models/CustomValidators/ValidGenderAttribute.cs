using System;
using System.ComponentModel.DataAnnotations;

public class ValidGenderAttribute : ValidationAttribute
{
    private enum _genders{ Masculino, Feminino, Outros };
    public string GetErrorMessage() =>
        "Gênero inválido, escolha entre Masculino, Feminino ou Outros.";

    protected override ValidationResult IsValid(object value,
        ValidationContext validationContext)
    {
        foreach(string gender in Enum.GetNames(typeof(_genders)))
        {
            if ( gender == ((String)value))
            {
                return ValidationResult.Success;
            }
        }

        return new ValidationResult(GetErrorMessage());
    }
}