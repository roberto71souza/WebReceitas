using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Dominio
{
    [Table("Receitas")]
    public class Receita
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Conteudo { get; set; }
        public string Acessório { get; set; }
        public DateTime Data_Publicacao { get; set; }
        public Usuario Usuario { get; set; }
    }
}
