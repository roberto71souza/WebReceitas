using System.ComponentModel.DataAnnotations;

namespace WebAppReceitas.Models
{
    public class EsqueciSenhaModel
    {
        [EmailAddress(ErrorMessage = "Formato Errado, ex: exemplo@exemplo.com"),Display(Name = "Email")]
        public string Email { get; set; }
    }
}