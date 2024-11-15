using Microsoft.AspNetCore.Mvc;
using MicroserviceVendas.DTO;
using MicroserviceVendas.Infra;
using MicroserviceVendas.Servicos;
using System.Threading.Tasks;

namespace MicroserviceVendas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VendasController : ControllerBase
    {
        private readonly VendaService _vendaService;

        public VendasController(VendaService vendaService)
        {
            _vendaService = vendaService;
        }

        [HttpPost]
        public async Task<IActionResult> CriarVenda([FromBody] VendaDTO vendaDto)
        {
            var venda = await _vendaService.CriarVenda(vendaDto);
            return CreatedAtAction(nameof(ObterVenda), new { id = venda.Id }, venda);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObterVenda(int id)
        {
            var venda = await _vendaService.ObterVenda(id);
            return venda == null ? NotFound() : Ok(venda);
        }
    }
}
