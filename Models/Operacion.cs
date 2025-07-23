using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebOriginBank.Models
{
    public class Operacion
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int TarjetaId { get; set; }

        [ForeignKey("TarjetaId")]
        public Tarjeta Tarjeta { get; set; }

        public DateTime FechaHora { get; set; }

        public string TipoOperacion { get; set; } 

        public decimal? Monto { get; set; }
    }
}