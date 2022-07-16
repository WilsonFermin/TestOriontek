using System.ComponentModel.DataAnnotations;

namespace EventrixAPI.Entidades
{
    public class Cliente
    {
        public int Id { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "Nombre no puede ser muy extenso")]
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        [Required]
        public string Cedula { get; set; }
        [Phone]
        public string Telefono { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public List<Direccion> Direcciones { get; set; }
    }
}
