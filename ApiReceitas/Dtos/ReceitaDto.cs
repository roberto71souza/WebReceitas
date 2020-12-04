using System;
using System.ComponentModel.DataAnnotations;

namespace ApiReceitas.Dtos
{
    public class ReceitaDto
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Conteudo { get; set; }
        public string Acessório { get; set; }
        
        [DisplayFormat(DataFormatString = "dd/MM/yyyy")]
        public DateTime Data_Publicacao { get; set; }

        public UsuarioDto Usuario { get; set; }
    }
}
