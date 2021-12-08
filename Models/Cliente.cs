using System;
using System.ComponentModel.DataAnnotations;


namespace SistemaCliente.Models
{
    public class Cliente
    {
        public long Id { get; set; }


        [Required]
        [RegularExpression(@"^([A-Z][a-zç]+\s)+([A-Z][a-zç]+)$",
         ErrorMessage = "Nome deve possuir ao menos DUAS palavras, separadas por UM espaço. As palavras devem iniciar com maiúsculas. E não podem conter outras maiúsculas, símbolos, números e acentos.")]
        public string Name { get; set; }
        
        [Required]
        [ValidDate]
        public DateTime BirthDate { get; set; }
        
        [Required]
        [ValidGender]
        public string Gender { get; set; }
    }
}