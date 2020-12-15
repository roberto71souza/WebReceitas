using System.ComponentModel.DataAnnotations;

namespace WebAppReceitas.Models
{
    public class EsqueciSenhaModel
    {
        [EmailAddress,Display(Name = "Email")]
        public string Email { get; set; }
    }
}