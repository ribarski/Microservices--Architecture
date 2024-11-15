<<<<<<< HEAD
using System;
=======
﻿using System;
>>>>>>> 75c7645fc84b4fec54658bb263f887ae6bd4ba70
using System.ComponentModel.DataAnnotations;

namespace MicroservicePrecos.Infra
{
    public class Preco
    {
        [Key]
        public int Id { get; set; }
        public int ProdutoId { get; set; }
        public decimal Valor { get; set; }
        public DateTime DataAtualizacao { get; set; } = DateTime.UtcNow;
        public bool Ativo { get; set; } = true;
    }
}