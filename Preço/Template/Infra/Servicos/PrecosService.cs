using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MicroservicePrecos.DTO;
using MicroservicePrecos.Infra;
using MicroservicePrecos.Infra.Contexto;

namespace Template.Infra.Servicos
{
    public class PrecosService
    {
        private readonly PrecosContext _context;

        public PrecosService(PrecosContext context)
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