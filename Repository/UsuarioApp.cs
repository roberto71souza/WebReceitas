using Dominio;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class UsuarioApp : IReceitasRepository
    {
        public ReceitasContext _contexto { get; set; }

        public UsuarioApp(ReceitasContext contexto)
        {
            _contexto = contexto;
        }

        public async Task<IEnumerable<object>> Listar()
        {
            var lista = await _contexto.Usuarios.Include(x => x.Receitas).AsNoTracking().OrderBy(d => d.Id).ToListAsync();
            return lista;
        }

        public async Task<object> BuscaID(int id)
        {
            var result = await _contexto.Usuarios.Where(x => x.Id == id).Include(d => d.Receitas).AsNoTracking().OrderBy(d => d.Id).FirstOrDefaultAsync();
            return result;
        }

        public Task Adicionar(object modelo)
        {
            _contexto.Add(modelo);

           return Task.FromResult(_contexto.SaveChanges());
        }

        public Task Deletar<T>(T modelo)
        {
            _contexto.Remove(modelo);
           return Task.FromResult(_contexto.SaveChanges());
        }

        public Task Atualizar(object modelo)
        {
            throw new NotImplementedException();
        }
    }
}
