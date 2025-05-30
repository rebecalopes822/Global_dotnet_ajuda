using Ajuda.API;
using Ajuda.API.Repositories;
using Ajuda.API.Services;
using Ajuda.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// 🔌 Conexão com o banco Oracle
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseOracle(builder.Configuration.GetConnectionString("OracleConnection")));

// 🔄 Injeção de dependência dos Repositórios
builder.Services.AddScoped<UsuarioRepository>();
builder.Services.AddScoped<TipoAjudaRepository>();
builder.Services.AddScoped<PedidoAjudaRepository>();

// 🧠 Injeção de dependência dos Serviços (com interfaces)
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<ITipoAjudaService, TipoAjudaService>();
builder.Services.AddScoped<IPedidoAjudaService, PedidoAjudaService>();

// 📦 Suporte a ciclos no JSON (evita erro de referência circular)
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.WriteIndented = true;
    });

// 🧪 Swagger com comentários XML e descrição da API
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
        Description = "API para Cadastro e Solicitação de Ajuda Comunitária. Possui integração com IA (ML.NET) e segue boas práticas RESTful."
    });

    c.TagActionsBy(api => new[] { api.GroupName });
    c.DocInclusionPredicate((_, _) => true);
});

var app = builder.Build();

// 🚀 Middlewares
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
