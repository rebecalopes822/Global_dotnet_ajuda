namespace Ajuda.API.IA
{
    public class UrgenciaInputDTO
    {
        public int TipoAjudaId { get; set; }
        public int CriancasNoLocal { get; set; } // 0 = Não, 1 = Sim
        public int PessoasNoLocal { get; set; }
        public int DiasSemAjuda { get; set; }
        public int VoluntariosProximos { get; set; }
    }
}
