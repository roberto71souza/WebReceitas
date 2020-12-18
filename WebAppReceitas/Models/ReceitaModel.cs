using Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppReceitas.Models
{
    public class ReceitaModel
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Conteudo { get; set; }

        public string Acessório { get; set; }
        public DateTime Data_Publicacao { get; set; }
        public Usuario Usuario { get; set; }

    }
}
