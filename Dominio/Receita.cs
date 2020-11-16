using System;
using System.Collections.Generic;
using System.Text;

namespace Dominio
{
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
