using System;
using System.ComponentModel.DataAnnotations;

namespace MicroserviceVendas.Infra
{
    public class Venda
    {
        [Key]
        public int Id { get; set; }
        public int ProdutoId { get; set; }
        public int Quantidade { get; set; }
        public decimal PrecoUnitario { get; set; }
        public decimal Total => Quantidade * PrecoUnitario;
        public DateTime DataVenda { get; set; } = DateTime.UtcNow;
        public string Status { get; set; } = "Pendente";
    }
}
