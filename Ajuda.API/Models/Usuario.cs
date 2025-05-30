using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Ajuda.API.Models
{
    [Table("TB_USUARIO")]
    public class Usuario
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("NOME")]
        public string Nome { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        [EmailAddress]
        [Column("EMAIL")]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        [Column("TELEFONE")]
        public string Telefone { get; set; } = string.Empty;

        [Required]
        [Column("EH_VOLUNTARIO")]
        public int EhVoluntario { get; set; }

        /// <summary>
        /// Navegação reversa: um usuário pode ter vários pedidos de ajuda.
        /// </summary>
        [JsonIgnore]
        public ICollection<PedidoAjuda>? PedidosAjuda { get; set; }
    }
}
