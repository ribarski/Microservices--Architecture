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
                Descricao = produtoDto.Descricao,
                QuantidadeEstoque = produtoDto.QuantidadeEstoque
            };

            _context.Produtos.Add(produto);
            await _context.SaveChangesAsync();
            return produto;
        }
    }
}