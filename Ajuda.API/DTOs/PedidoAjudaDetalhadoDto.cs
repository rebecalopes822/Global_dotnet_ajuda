namespace Ajuda.API.DTOs
{
    public class PedidoAjudaDetalhadoDto
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public string? NomeUsuario { get; set; }
        public int TipoAjudaId { get; set; }
        public string? NomeTipoAjuda { get; set; }
        public string Endereco { get; set; } = string.Empty;
        public int QuantidadePessoas { get; set; }
        public int NivelUrgencia { get; set; }
        public DateTime DataHoraPedido { get; set; }
    }
}
