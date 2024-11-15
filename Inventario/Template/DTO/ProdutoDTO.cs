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