using Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository.InterfaceUsuario
{
    public interface IUsuarioApp
    {
        public IEnumerable<Usuario> ListarUsuario();
        public void AdicionarUsuario(Usuario modelo);
        public Usuario BuscaId(int id);
        public void DeleteUsuario(Usuario modelo);
    }
}
