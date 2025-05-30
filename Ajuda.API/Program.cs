using Ajuda.API;
using Ajuda.API.Mensageria;
using Ajuda.API.Repositories;
using Ajuda.API.Services;
using Ajuda.API.Services.Interfaces;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Text.Json.Serialization;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

// 🔌 Banco Oracle
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseOracle(builder.Configuration.GetConnectionString("OracleConnection")));

// 🧠 Repositórios (Interfaces)
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<ITipoAjudaRepository, TipoAjudaRepository>();
builder.Services.AddScoped<IPedidoAjudaRepository, PedidoAjudaRepository>();

// 🧠 Serviços (Interfaces)
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<ITipoAjudaService, TipoAjudaService>();
builder.Services.AddScoped<IPedidoAjudaService, PedidoAjudaService>();

// ✅ Fila com Channel<T>
builder.Services.AddSingleton<PedidoAjudaQueue>();
builder.Services.AddHostedService<PedidoAjudaConsumerService>();

// ✅ Rate Limiting
builder.Services.AddRateLimiter(_ =>
{
    _.AddPolicy("fixed", context =>
        RateLimitPartition.GetFixedWindowLimiter(
            context.Connection.RemoteIpAddress?.ToString() ?? "anon",
            _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 5,
                Window = TimeSpan.FromSeconds(60),
                QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                QueueLimit = 0
            }));
});

// 📦 Controllers e JSON
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.WriteIndented = true;
    });

// 🧪 Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);

    c.SwaggerDoc("v1", new()
    {
        Title = "Ajuda.API",
        Version = "v1",
        Description = "API para Cadastro e Solicitação de Ajuda Comunitária. Possui integração com IA (ML.NET), fila assíncrona via Channel<T> e boas práticas RESTful."
    });

    c.TagActionsBy(api => new[] { api.GroupName });
    c.DocInclusionPredicate((_, _) => true);
});

var app = builder.Build();

// 🌐 Middlewares
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.UseRateLimiter(); // ✅ Ativa Rate Limiting

// ✅ Mensagem personalizada para 429
app.Use(async (context, next) =>
{
    await next();

    if (context.Response.StatusCode == 429)
    {
        context.Response.ContentType = "application/json";
        var response = new { mensagem = "Limite de requisições excedido. Tente novamente em alguns instantes." };
        var json = System.Text.Json.JsonSerializer.Serialize(response);
        await context.Response.WriteAsync(json);
    }
});

app.MapControllers();
app.Run();
