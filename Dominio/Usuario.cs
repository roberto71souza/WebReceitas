using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Dominio
{
    [Table("Usuario")]
    public class Usuario : IdentityUser<int>
    {
        [Required(ErrorMessage = "{0} e requerido"),
                 MinLength(3, ErrorMessage = "{0} minimo 3 caracteres")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "{0} e requerido"),
        EmailAddress(ErrorMessage = "{0} invalido ex: exemplo@exemplo.com")]
        public override string Email { get => base.Email; set => base.Email = value; }

        [Required(ErrorMessage = "{0} e requerido"),
                MinLength(3, ErrorMessage = "{0} minimo 3 caracteres")]
        public string Cidade { get; set; }

        [MaxLength(20, ErrorMessage = "{0} Maximo 20 caracteres")]
        public string Estado { get; set; }
        public string Data_Nascimento { get; set; }
        public List<Receita> Receitas { get; set; }

    }
}
