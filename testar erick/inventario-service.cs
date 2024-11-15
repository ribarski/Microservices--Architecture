// DTO/ProdutoDTO.cs
namespace MicroserviceInventario.DTO
{
    public class ProdutoDTO
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public int QuantidadeEstoque { get; set; }
    }

    public class AtualizarEstoqueDTO
    {
        public int ProdutoId { get; set; }
        public int Quantidade { get; set; }
    }
}

// Infra/Produto.cs
using System.ComponentModel.DataAnnotations;

namespace MicroserviceInventario.Infra
{
    public class Produto
    {
        [Key]
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public int QuantidadeEstoque { get; set; }
        public string Status { get; set; } = "Ativo";
    }
}

// Infra/Contexto/InventarioContext.cs
using Microsoft.EntityFrameworkCore;

namespace MicroserviceInventario.Infra.Contexto
{
    public class InventarioContext : DbContext
    {
        public InventarioContext(DbContextOptions<InventarioContext> options) : base(options) { }

        public DbSet<Produto> Produtos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Produto>().HasKey(e => e.Id);
        }
    }
}

// Services/InventarioService.cs
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using MicroserviceInventario.DTO;
using MicroserviceInventario.Infra;
using MicroserviceInventario.Infra.Contexto;

namespace MicroserviceInventario.Services
{
    public class InventarioService
    {
        private readonly InventarioContext _context;

        public InventarioService(InventarioContext context)
        {
            _context = context;
        }

        public async Task<List<Produto>> ObterTodosProdutos()
        {
            return await _context.Produtos.ToListAsync();
        }

        public async Task<Produto> ObterProdutoPorId(int id)
        {
            return await _context.Produtos.FindAsync(id);
        }

        public async Task<bool> VerificarDisponibilidade(int produtoId, int quantidade)
        {
            var produto = await _context.Produtos.FindAsync(produtoId);
            return produto != null && produto.QuantidadeEstoque >= quantidade;
        }

        public async Task<bool> AtualizarEstoque(AtualizarEstoqueDTO dto)
        {
            var produto = await _context.Produtos.FindAsync(dto.ProdutoId);
            
            if (produto == null || produto.QuantidadeEstoque < dto.Quantidade)
                return false;

            produto.QuantidadeEstoque -= dto.Quantidade;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Produto> AdicionarProduto(ProdutoDTO produtoDto)
        {
            var produto = new Produto
            {
                Nome = produtoDto.Nome,
                QuantidadeEstoque = produtoDto.QuantidadeEstoque
            };

            _context.Produtos.Add(produto);
            await _context.SaveChangesAsync();
            return produto;
        }
    }
}

// Controllers/InventarioController.cs
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using MicroserviceInventario.DTO;
using MicroserviceInventario.Services;

namespace MicroserviceInventario.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventarioController : ControllerBase
    {
        private readonly InventarioService _inventarioService;

        public InventarioController(InventarioService inventarioService)
        {
            _inventarioService = inventarioService;
        }

        [HttpGet]
        public async Task<IActionResult> ObterTodosProdutos()
        {
            var produtos = await _inventarioService.ObterTodosProdutos();
            return Ok(produtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObterProduto(int id)
        {
            var produto = await _inventarioService.ObterProdutoPorId(id);
            return produto == null ? NotFound() : Ok(produto);
        }

        [HttpGet("{id}/disponibilidade")]
        public async Task<IActionResult> VerificarDisponibilidade(int id, [FromQuery] int quantidade)
        {
            var disponivel = await _inventarioService.VerificarDisponibilidade(id, quantidade);
            return Ok(new { Disponivel = disponivel });
        }

        [HttpPut("{id}/estoque")]
        public async Task<IActionResult> AtualizarEstoque(int id, [FromBody] AtualizarEstoqueDTO dto)
        {
            if (id != dto.ProdutoId)
                return BadRequest();

            var resultado = await _inventarioService.AtualizarEstoque(dto);
            return resultado ? Ok() : NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> AdicionarProduto([FromBody] ProdutoDTO produtoDto)
        {
            var produto = await _inventarioService.AdicionarProduto(produtoDto);
            return CreatedAtAction(nameof(ObterProduto), new { id = produto.Id }, produto);
        }
    }
}
