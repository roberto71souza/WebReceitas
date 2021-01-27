using Dominio;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebAppReceitas.Models
{
    public class UsuarioModel
    {
        public int Id { get; set; }

        public string Nome { get; set; }

        [EmailAddress(ErrorMessage = "{0} invalido ex: exemplo@exemplo.com")]
        public string Email { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public string Data_Nascimento { get; set; }
        public List<ReceitaModel> Receitas { get; set; }
    }
}
