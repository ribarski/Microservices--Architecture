using System.ComponentModel.DataAnnotations;

namespace MicroserviceInventario.Infra
{
    public class Produto
    {
        [Key]
        public int Id { get; set; }
        public string Nome { get; set; }
        public string? Descricao { get; set; }
        public int QuantidadeEstoque { get; set; }
        public string Status { get; set; } = "Ativo";
    }
}