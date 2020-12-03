using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dominio
{
    [Table("Usuario")]
    public class Usuario
    {
        public int Id { get; set; }

       // [Required(ErrorMessage = "{0} e requerido"),
       //     MinLength(3, ErrorMessage = "{0} minimo 3 caracteres")]
        public string Nome { get; set; }

       // [Required(ErrorMessage = "{0} e requerido"),
       //     EmailAddress(ErrorMessage = "{0} invalido ex: exemplo@exemplo.com")]
        public string Email { get; set; }

       // [Required(ErrorMessage = "{0} e requerido"),
       //     MinLength(3, ErrorMessage = "{0} minimo 3 caracteres")]
        public string Cidade { get; set; }

       // [MinLength(3, ErrorMessage = "{0} minimo 3 caracteres")]
        public string Estado { get; set; }
        public DateTime Data_Nascimento { get; set; }
        public List<Receita> Receitas { get; set; }

    }
}
