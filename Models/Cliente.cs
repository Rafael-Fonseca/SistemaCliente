using System;

namespace SistemaCliente.Models
{
    public class Cliente
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
        public string Gender { get; set; }
    }
}