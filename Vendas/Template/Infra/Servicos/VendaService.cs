using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using MicroserviceVendas.DTO;
using MicroserviceVendas.Infra;
using MicroserviceVendas.Infra.Contexto;
using Microsoft.Extensions.Configuration;

namespace MicroserviceVendas.Servicos
{
    public class VendaService
    {
        private readonly VendasContext _context;
        private readonly HttpClient _inventarioClient;
        private readonly HttpClient _precosClient;

        public VendaService(VendasContext context, IConfiguration configuration)
        {
            _context = context;

            // Configura os HttpClients com base nas URLs dos microserviços
            _inventarioClient = new HttpClient
            {
                BaseAddress = new Uri(configuration["Services:Inventario"]) // URL do Inventário
            };

            _precosClient = new HttpClient
            {
                BaseAddress = new Uri(configuration["Services:Precos"]) // URL do Preços
            };
        }

        // Método para verificar estoque
        private async Task<bool> VerificarEstoque(int produtoId, int quantidade)
        {
            var response = await _inventarioClient.GetAsync($"api/inventario/{produtoId}/disponibilidade?quantidade={quantidade}");
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<DisponibilidadeResponse>();
                return result?.Disponivel ?? false;
            }
            return false;
        }
        // Método para obter preço
        private async Task<decimal?> ObterPreco(int produtoId)
        {
            var response = await _precosClient.GetAsync($"api/precos/{produtoId}");
            if (response.IsSuccessStatusCode)
            {
                var preco = await response.Content.ReadFromJsonAsync<PrecoResponse>();
                return preco?.Valor;
            }
            return null;
        }
        // Método para criar venda com validação de estoque e preço
        public async Task<Venda> CriarVenda(VendaDTO vendaDto)
        {
            // Valida o estoque
            var estoqueDisponivel = await VerificarEstoque(vendaDto.ProdutoId, vendaDto.Quantidade);
            if (!estoqueDisponivel)
            {
                throw new InvalidOperationException("Estoque insuficiente para o produto.");
            }

            // Obtém o preço do produto
            var preco = await ObterPreco(vendaDto.ProdutoId);
            if (preco == null)
            {
                throw new InvalidOperationException("Preço do produto não encontrado.");
            }

            // Cria a venda
            var venda = new Venda
            {
                ProdutoId = vendaDto.ProdutoId,
                Quantidade = vendaDto.Quantidade,
                PrecoUnitario = preco.Value,
                Status = "Confirmado"
            };

            _context.Vendas.Add(venda);
            await _context.SaveChangesAsync();
            return venda;
        }

        public async Task<Venda> ObterVenda(int id) => await _context.Vendas.FindAsync(id);
    }

    // DTOs para respostas de outros serviços
    public class DisponibilidadeResponse
    {
        public bool Disponivel { get; set; }
    }

    public class PrecoResponse
    {
        public decimal Valor { get; set; }
    }
}