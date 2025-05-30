using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ajuda.API.Models
{
    [Table("TB_PEDIDO_AJUDA")]
    public class PedidoAjuda
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }

        [Required]
        [Column("USUARIO_ID")]
        public int UsuarioId { get; set; }

        [ForeignKey("UsuarioId")]
        public Usuario? Usuario { get; set; }

        [Required]
        [Column("TIPO_AJUDA_ID")]
        public int TipoAjudaId { get; set; }

        [ForeignKey("TipoAjudaId")]
        public TipoAjuda? TipoAjuda { get; set; }

        [Required]
        [Column("ENDERECO")]
        public string Endereco { get; set; } = string.Empty;

        [Required]
        [Column("QUANTIDADE_PESSOAS")]
        public int QuantidadePessoas { get; set; }

        [Column("NIVEL_URGENCIA")]
        public int NivelUrgencia { get; set; }

        [Column("DATA_HORA_PEDIDO")]
        public DateTime DataHoraPedido { get; set; } = DateTime.Now;
    }
}
