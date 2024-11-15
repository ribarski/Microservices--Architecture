using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using MicroservicePrecos.DTO;
using Template.Infra.Servicos;

namespace MicroservicePrecos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrecosController : ControllerBase
    {
        private readonly PrecosService _precoService;

        public PrecosController(PrecosService precoService)
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