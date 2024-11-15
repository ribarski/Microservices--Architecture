<<<<<<< HEAD
using System.ComponentModel.DataAnnotations;
=======
﻿using System.ComponentModel.DataAnnotations;
>>>>>>> 75c7645fc84b4fec54658bb263f887ae6bd4ba70

namespace MicroserviceInventario.Infra
{
    public class Produto
    {
        [Key]
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public int QuantidadeEstoque { get; set; }
        public string Status { get; set; } = "Ativo";
    }
}
