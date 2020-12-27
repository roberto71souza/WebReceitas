using System;
using System.ComponentModel.DataAnnotations;

namespace ApiReceitas.Models
{
    public class ReceitaModel
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
        public string Data_Publicacao { get; set; }
        public UsuarioModel Usuario { get; set; }
    }
}
