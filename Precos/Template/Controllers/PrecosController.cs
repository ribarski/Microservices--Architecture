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

        [HttpPost("{produtoId}")]
        public async Task<IActionResult> AtualizarPreco(int produtoId, [FromBody] PrecoDTO precoDto)
        {
            if (produtoId != precoDto.ProdutoId)
                return BadRequest();

            var preco = await _precoService.AtualizarPreco(precoDto);
            return Ok(preco);
        }
    }
}