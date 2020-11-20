using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ApiReceitas.Dtos
{
    public class UsuarioDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "{0} e requerido"),
            MinLength(3,ErrorMessage = "{0} minimo 3 caracteres")]
        public string Nome { get; set; }

        [EmailAddress(ErrorMessage = "{0} invalido ex: exemplo@exemplo.com")]
        public string Email { get; set; }

        [MinLength(3,ErrorMessage = "{0} minimo 3 caracteres")]
        public string Cidade { get; set; }

        public List<ReceitaDto> Receitas { get; set; }
    }
}
