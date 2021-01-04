using Dominio;
using System.Net.Mail;
using System.Threading.Tasks;

namespace WebAppReceitas.Models
{
    public class Email
    {

        public static async Task Enviar(Usuario usuario,string subj,string link)
        {
            string _email = usuario.Email;
            string _subject = subj;
            string _mensagem = $"{usuario.Nome} clique no link abaixo para validar sua requisicao: \nLink: {link}";
            using (MailMessage mm = new MailMessage())
            {
                mm.From = new MailAddress("contabilycontext@gmail.com");
                mm.To.Add(_email);
                mm.Subject = _subject;
                mm.Body = _mensagem;
                mm.IsBodyHtml = false;
                using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtp.Credentials = new System.Net.NetworkCredential("contabilycontext@gmail.com", "context1968");
                    smtp.EnableSsl = true;
                    await smtp.SendMailAsync(mm);
                }
            }
        }
    }
}
