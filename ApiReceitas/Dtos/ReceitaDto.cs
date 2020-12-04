using System;
using System.ComponentModel.DataAnnotations;

namespace ApiReceitas.Dtos
{
    public class ReceitaDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "{0} e requerido"),
                MinLength(5, ErrorMessage = "{0} devera ter no minimo 5 caracteres")]
        public string Titulo { get; set; }

        [Required(ErrorMessage = "{0} e requerido"),
                MinLength(5, ErrorMessage = "{0} devera ter no minimo 5 caracteres")]
        public string Conteudo { get; set; }

        [MinLength(3, ErrorMessage = "{0} devera conter no minimo 3 palavras")]
        public string Acessório { get; set; }

        [DisplayFormat(DataFormatString = "dd/MM/yyyy")]
        public DateTime Data_Publicacao { get; set; }

        public UsuarioDto Usuario { get; set; }
    }
}
