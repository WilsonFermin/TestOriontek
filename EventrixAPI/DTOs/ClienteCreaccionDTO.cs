using EventrixAPI.Utilidades;
using System.ComponentModel.DataAnnotations;

namespace EventrixAPI.DTOs
{
    public class ClienteCreaccionDTO
    {
        [Required]
        [StringLength(100, ErrorMessage = "Nombre no puede ser muy extenso")]
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        [Required]
        [FormatoCedulaCorrecto]
        public string Cedula { get; set; }
        [Phone]
        public string Telefono { get; set; }
        [EmailAddress]
        public string Email { get; set; }
    }
}
