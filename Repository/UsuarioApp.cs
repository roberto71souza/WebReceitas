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

        public Task<object> BuscaID(int id)
        {
            throw new NotImplementedException();
        }

        public Task Adicionar(object modelo)
        {
            throw new NotImplementedException();
        }

        public Task Atualizar(object modelo)
        {
            throw new NotImplementedException();
        }

        public Task Deletar<T>(T modelo)
        {
            throw new NotImplementedException();
        }
    }
}
