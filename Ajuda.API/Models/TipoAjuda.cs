using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ajuda.API.Models
{
    [Table("TB_TIPO_AJUDA")]
    public class TipoAjuda
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }

        [Column("NOME")]
        public string Nome { get; set; } = string.Empty;

        [Column("DESCRICAO")]
        public string Descricao { get; set; } = string.Empty;
    }
}
