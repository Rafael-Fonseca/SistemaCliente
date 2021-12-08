using System;
using System.ComponentModel.DataAnnotations;

public class ValidDateAttribute : ValidationAttribute
{
    public string GetErrorMessage(string erro) {
        if (erro == "future"){
            return "A data de nascimento não pode estar no futuro.";
        }
        return "Data de nascimento não pode ser superior a 130 anos.";
    }

    protected override ValidationResult IsValid(object value,
        ValidationContext validationContext)
    {
        var now = DateTime.Now.Date;
        string erro = "";

        if (((DateTime)value).Date > now)
        {
            erro = "future";
            return new ValidationResult(GetErrorMessage(erro));

        }
        else if(DateTime.Now.Year - ((DateTime)value).Year > 130)
        {
            erro = "max age";
            return new ValidationResult(GetErrorMessage(erro));

        }

        return ValidationResult.Success;
    }
}