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
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Cidade { get; set; }

        [MaxLength(20, ErrorMessage = "{0} Maximo 20 caracteres")]
        public string Estado { get; set; }

        [DisplayFormat(DataFormatString = "dd/MM/yyyy")]
        public DateTime Data_Nascimento { get; set; }

        public List<Receita> Receitas { get; set; }

    }
}
