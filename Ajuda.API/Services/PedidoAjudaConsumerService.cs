using Ajuda.API.Mensageria;
using Ajuda.API.Services.Interfaces;

namespace Ajuda.API.Services
{
    public class PedidoAjudaConsumerService : BackgroundService
    {
        private readonly PedidoAjudaQueue _queue;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<PedidoAjudaConsumerService> _logger;

        public PedidoAjudaConsumerService(
            PedidoAjudaQueue queue,
            IServiceScopeFactory scopeFactory,
            ILogger<PedidoAjudaConsumerService> logger)
        {
            _queue = queue;
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("✅ PedidoAjudaConsumerService iniciado. Aguardando pedidos...");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var pedido = await _queue.DequeueAsync(stoppingToken);

                    using var scope = _scopeFactory.CreateScope();
                    var service = scope.ServiceProvider.GetRequiredService<IPedidoAjudaService>();

                    var salvo = await service.CriarDiretoAsync(pedido);

                    _logger.LogInformation("[SUCESSO] Pedido salvo com sucesso! ID = {Id}, Endereço = {Endereco}, Data = {DataHora}",
                        salvo.Id, salvo.Endereco, salvo.DataHoraPedido);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "[ERRO NO CONSUMER] Falha ao salvar pedido.");
                }
            }
        }
    }
}
