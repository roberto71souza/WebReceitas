using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dominio
{
    [Table("Receitas")]
    public class Receita
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Conteudo { get; set; }        
        public string Acessório { get; set; }

        [DisplayFormat(DataFormatString = "dd/MM/yyyy")]
        public DateTime Data_Publicacao { get; set; }

        public Usuario Usuario { get; set; }
    }
}
