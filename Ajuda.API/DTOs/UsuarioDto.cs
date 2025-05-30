using System.ComponentModel.DataAnnotations;

namespace Ajuda.API.DTOs
{
    public class UsuarioDto
    {
        public int Id { get; set; }

        [Required]
        public string Nome { get; set; } = string.Empty;

        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required, MaxLength(20)]
        public string Telefone { get; set; } = string.Empty;

        [Range(0, 1)]
        public int EhVoluntario { get; set; }
    }
}