using System.ComponentModel.DataAnnotations;

namespace EventrixAPI.Entidades
{
    public class Direccion
    {
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Ciudad { get; set; }
        [Required]
        [StringLength(100)]
        public string Sector { get; set; }
        [StringLength(100)]
        public string Calle { get; set; }
        public int NumeroCasa { get; set; }
        [StringLength(250, ErrorMessage = "Referencia muy larga")]
        public string Referencia { get; set; }
        public int ClienteId { get; set; }
        public Cliente Cliente { get; set; }

    }
}
