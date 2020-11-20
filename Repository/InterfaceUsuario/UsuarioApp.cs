using Dominio;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Repository.InterfaceUsuario
{
    public class UsuarioApp : IUsuarioApp
    {
        public ReceitasContext _contexto { get; set; }

        public UsuarioApp(ReceitasContext contexto)
        {
            _contexto = contexto;
        }

        public void AdicionarUsuario(Usuario modelo)
        {
            _contexto.Usuarios.Add(modelo);

            _contexto.SaveChanges();
        }

        public IEnumerable<Usuario> ListarUsuario()
        {
            var lista = _contexto.Usuarios.Include(x => x.Receitas).AsNoTracking().OrderBy(d => d.Id);
            return lista.ToArray();
        }

        public Usuario BuscaId(int id)
        {
            var result = _contexto.Usuarios.Where(x => x.Id == id).Include(d => d.Receitas).AsNoTracking().OrderBy(d => d.Id);
            return result.FirstOrDefault();
        }
        public void DeleteUsuario(Usuario modelo)
        {
            _contexto.Usuarios.Remove(modelo);
            _contexto.SaveChanges();
        }
    }
}
