using Dominio;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebAppReceitas.Models
{
    public class UsuarioModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "{0} e requerido"),
                MinLength(3, ErrorMessage = "{0} minimo 3 caracteres")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "{0} e requerido"),
                EmailAddress(ErrorMessage = "{0} invalido ex: exemplo@exemplo.com")]
        public string Email { get; set; }

        [Required(ErrorMessage = "{0} e requerido"),
                  MinLength(3, ErrorMessage = "{0} minimo 3 caracteres")]
        public string Cidade { get; set; }

        [MaxLength(20, ErrorMessage = "{0} Maximo 20 caracteres")]
        public string Estado { get; set; }
        public string Data_Nascimento { get; set; }
        public List<ReceitaModel> Receitas { get; set; }
    }
}
