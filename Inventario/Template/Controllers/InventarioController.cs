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