using Microsoft.EntityFrameworkCore;
using MicroservicePrecos.Infra.Contexto;
using MicroserviceVendas.Infra;
using Template.Infra.Servicos;

var builder = WebApplication.CreateBuilder(args);

// Adiciona os servi�os ao container

builder.Services.AddControllers();

// Configura o Swagger
builder.Services.AddEndpointsApiExplorer(); // Explora os endpoints da API
builder.Services.AddSwaggerGen(); // Gera a documenta��o da API

// Configura o contexto do banco de dados SQLite
builder.Services.AddDbContext<PrecosContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// Adiciona o servi�o de vendas ao container de depend�ncias
builder.Services.AddScoped<PrecosService>();

// Configura��o do GeradorDeServicos, se necess�rio
GeradorDeServicos.ServiceProvider = builder.Services.BuildServiceProvider();

var app = builder.Build();

// Configura o pipeline de requisi��es HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); // Gera a documenta��o do Swagger
    app.UseSwaggerUI(); // Interface visual do Swagger
}

app.UseHttpsRedirection(); // Redireciona para HTTPS (caso necess�rio)

app.UseAuthorization(); // Habilita a autoriza��o para os endpoints (se necess�rio)

app.MapControllers(); // Mapeia os controladores da API

app.Run(); // Inicia a aplica��o
