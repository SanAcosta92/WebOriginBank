using Azure;
using System;
using System.ComponentModel.DataAnnotations;

namespace WebOriginBank.Models
{
    public class Tarjeta
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(16)]
        public string Nro { get; set; }

        [Required]
        [StringLength(4)]
        public string Pin { get; set; }

        public decimal Balance { get; set; }

        public DateTime Vencimiento { get; set; }

        public bool Bloqueada { get; set; }

        public int Intentos { get; set; }

        public ICollection<Operacion> Operacion { get; set; }
    }
}
