namespace Ajuda.API.Helpers
{
    public static class RateLimitStore
    {
        private static readonly Dictionary<string, DateTime> UltimasRequisicoes = new();
        private static readonly object LockObject = new();

        public static bool PodeExecutar(string chave, TimeSpan intervalo)
        {
            lock (LockObject)
            {
                if (UltimasRequisicoes.TryGetValue(chave, out var ultima))
                {
                    if ((DateTime.Now - ultima) < intervalo)
                        return false;
                }

                UltimasRequisicoes[chave] = DateTime.Now;
                return true;
            }
        }
    }
}
