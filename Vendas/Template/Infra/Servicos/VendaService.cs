using System.Threading.Tasks;
using MicroserviceVendas.DTO;
using MicroserviceVendas.Infra;
using MicroserviceVendas.Infra.Contexto;

namespace MicroserviceVendas.Servicos
{
    public class VendaService
    {
        private readonly VendasContext _context;

        public VendaService(VendasContext context)
        {
            _context = context;
        }

        public async Task<Venda> CriarVenda(VendaDTO vendaDto)
        {
            var venda = new Venda
            {
                ProdutoId = vendaDto.ProdutoId,
                Quantidade = vendaDto.Quantidade,
                PrecoUnitario = vendaDto.PrecoUnitario,
                Status = "Confirmado"
            };

            _context.Vendas.Add(venda);
            await _context.SaveChangesAsync();
            return venda;
        }

        public async Task<Venda> ObterVenda(int id) => await _context.Vendas.FindAsync(id);
    }
}
