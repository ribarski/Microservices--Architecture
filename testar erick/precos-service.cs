// DTO/PrecoDTO.cs
namespace MicroservicePrecos.DTO
{
    public class PrecoDTO
    {
        public int ProdutoId { get; set; }
        public decimal Valor { get; set; }
    }
}

// Infra/Preco.cs
using System;
using System.ComponentModel.DataAnnotations;

namespace MicroservicePrecos.Infra
{
    public class Preco
    {
        [Key]
        public int Id { get; set; }
        public int ProdutoId { get; set; }
        public decimal Valor { get; set; }
        public DateTime DataAtualizacao { get; set; } = DateTime.UtcNow;
        public bool Ativo { get; set; } = true;
    }
}

// Infra/Contexto/PrecosContext.cs
using Microsoft.EntityFrameworkCore;

namespace MicroservicePrecos.Infra.Contexto
{
    public class PrecosContext : DbContext
    {
        public PrecosContext(DbContextOptions<PrecosContext> options) : base(options) { }

        public DbSet<Preco> Precos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Preco>().HasKey(e => e.Id);
        }
    }
}

// Services/PrecoService.cs
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MicroservicePrecos.DTO;
using MicroservicePrecos.Infra;
using MicroservicePrecos.Infra.Contexto;

namespace MicroservicePrecos.Services
{
    public class PrecoService
    {
        private readonly PrecosContext _context;

        public PrecoService(PrecosContext context)
        {
            _context = context;
        }

        public async Task<Preco> ObterPrecoProduto(int produtoId)
        {
            return await _context.Precos
                .Where(p => p.ProdutoId == produtoId && p.Ativo)
                .OrderByDescending(p => p.DataAtualizacao)
                .FirstOrDefaultAsync();
        }

        public async Task<Preco> AtualizarPreco(PrecoDTO precoDto)
        {
            // Desativa o preço atual
            var precoAtual = await _context.Precos
                .Where(p => p.ProdutoId == precoDto.ProdutoId && p.Ativo)
                .FirstOrDefaultAsync();

            if (precoAtual != null)
            {
                precoAtual.Ativo = false;
                _context.Precos.Update(precoAtual);
            }

            // Cria novo preço
            var novoPreco = new Preco
            {
                ProdutoId = precoDto.ProdutoId,
                Valor = precoDto.Valor,
                DataAtualizacao = DateTime.UtcNow,
                Ativo = true
            };

            _context.Precos.Add(novoPreco);
            await _context.SaveChangesAsync();
            return novoPreco;
        }
    }
}

// Controllers/PrecosController.cs
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using MicroservicePrecos.DTO;
using MicroservicePrecos.Services;

namespace MicroservicePrecos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrecosController : ControllerBase
    {
        private readonly PrecoService _precoService;

        public PrecosController(PrecoService precoService)
        {
            _precoService = precoService;
        }

        [HttpGet("{produtoId}")]
        public async Task<IActionResult> ObterPreco(int produtoId)
        {
            var preco = await _precoService.ObterPrecoProduto(produtoId);
            return preco == null ? NotFound() : Ok(preco);
        }

        [HttpPut("{produtoId}")]
        public async Task<IActionResult> AtualizarPreco(int produtoId, [FromBody] PrecoDTO precoDto)
        {
            if (produtoId != precoDto.ProdutoId)
                return BadRequest();

            var preco = await _precoService.AtualizarPreco(precoDto);
            return Ok(preco);
        }
    }
}
