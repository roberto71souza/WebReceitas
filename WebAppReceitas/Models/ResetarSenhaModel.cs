using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppReceitas.Models
{
    public class ResetarSenhaModel
    {
        public string Token { get; set; }
        public string Email { get; set; }

        [DataType(DataType.Password),Display(Name = "Nova Senha")]
        public string Password { get; set; }

        [DataType(DataType.Password), Compare("Password",ErrorMessage = "Senhas nao batem"),Display(Name = "Confirme a Senha")]
        public string ConfirmPassword { get; set; }
    }
}
