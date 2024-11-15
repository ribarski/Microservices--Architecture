using Microsoft.EntityFrameworkCore;
using MicroserviceInventario.Infra.Contexto;
using MicroserviceInventario.Services;
using MicroserviceInventario.Infra ;

var builder = WebApplication.CreateBuilder(args);

// Adiciona os serviços ao container

builder.Services.AddControllers();

// Configura o Swagger
builder.Services.AddEndpointsApiExplorer(); // Explora os endpoints da API
builder.Services.AddSwaggerGen(); // Gera a documentação da API

// Configura o contexto do banco de dados SQLite
builder.Services.AddDbContext<InventarioContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// Adiciona o serviço de vendas ao container de dependências
builder.Services.AddScoped<InventarioService>();

// Configuração do GeradorDeServicos, se necessário
GeradorDeServicos.ServiceProvider = builder.Services.BuildServiceProvider();

var app = builder.Build();

// Configura o pipeline de requisições HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); // Gera a documentação do Swagger
    app.UseSwaggerUI(); // Interface visual do Swagger
}

app.UseHttpsRedirection(); // Redireciona para HTTPS (caso necessário)

app.UseAuthorization(); // Habilita a autorização para os endpoints (se necessário)

app.MapControllers(); // Mapeia os controladores da API

app.Run(); // Inicia a aplicação
