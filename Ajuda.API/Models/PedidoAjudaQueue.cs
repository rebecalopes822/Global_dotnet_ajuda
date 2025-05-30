using System.Threading.Channels;
using Ajuda.API.Models;

namespace Ajuda.API.Mensageria
{
    public class PedidoAjudaQueue
    {
        private readonly Channel<PedidoAjuda> _channel;

        public PedidoAjudaQueue()
        {
            _channel = Channel.CreateUnbounded<PedidoAjuda>();
        }

        public async Task EnqueueAsync(PedidoAjuda pedido)
        {
            await _channel.Writer.WriteAsync(pedido);
        }

        public async Task<PedidoAjuda> DequeueAsync(CancellationToken cancellationToken)
        {
            return await _channel.Reader.ReadAsync(cancellationToken);
        }
    }
}
