using System.ComponentModel.DataAnnotations;

public class PedidoAjudaDto
{
    [Required]
    public int UsuarioId { get; set; }

    [Required]
    public int TipoAjudaId { get; set; }

    [Required]
    public string Endereco { get; set; } = string.Empty;

    [Required]
    public int QuantidadePessoas { get; set; }

    [Range(1, 5, ErrorMessage = "O nível de urgência deve ser entre 1 e 5.")]
    public int NivelUrgencia { get; set; }
}
