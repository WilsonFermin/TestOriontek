using System.ComponentModel.DataAnnotations;

namespace EventrixAPI.DTOs
{
    public class EditarAdminDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
