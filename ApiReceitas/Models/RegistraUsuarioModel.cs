﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ApiReceitas.Models
{
    public class RegistraUsuarioModel
    {
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

        [Required,DataType(DataType.Password)]
        public string Password { get; set; }

        [Required, DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Senhas nao batem")]
        public string ConfirmPassword { get; set; }

        [DisplayFormat(DataFormatString = "dd/MM/yyyy")]
        public string Data_Nascimento { get; set; }
    }
}
