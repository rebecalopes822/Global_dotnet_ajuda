using Ajuda.API.Models;

namespace Ajuda.API.DTOs
{
    public class UsuarioComLinksDto : Usuario
    {
        public List<LinkDto> Links { get; set; } = new();

        public UsuarioComLinksDto(Usuario usuario)
        {
            Id = usuario.Id;
            Nome = usuario.Nome;
            Email = usuario.Email;
            Telefone = usuario.Telefone;
            EhVoluntario = usuario.EhVoluntario;
        }
    }
}
