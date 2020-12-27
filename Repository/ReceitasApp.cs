using Dominio;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class ReceitasApp : IReceitasRepository
    {
        public ReceitasContext _contexto { get; set; }

        public ReceitasApp(ReceitasContext contexto)
        {
            _contexto = contexto;
        }

        public async Task<IEnumerable<object>> Listar()
        {
            var lista = await _contexto.Receitas.Include(x => x.Usuario).AsNoTracking().OrderBy(d => d.Id).ToListAsync();
            return lista;
        }

        public async Task<object> ListarReceitasUsuario(int id)
        {
            var result = await _contexto.Receitas.Where(x => x.Usuario.Id == id).Include(d => d.Usuario).AsNoTracking().ToListAsync();
            return result;
        }

        public async Task<object> BuscaID(int id)
        {
            var result = await _contexto.Receitas.Where(x => x.Id == id).Include(d => d.Usuario).AsNoTracking().FirstOrDefaultAsync();
            return result;
        }

        public Task Adicionar(object modelo)
        {
            _contexto.Add(modelo);
            var modelReceita = (Receita)modelo;
            _contexto.Entry(modelReceita.Usuario).State = EntityState.Unchanged;

            _contexto.SaveChanges();
            return Task.CompletedTask;
        }

        public Task Atualizar(object modelo)
        {
            var modelReceita = (Receita)modelo;
            var entity = _contexto.Receitas.Find(modelReceita.Id);
            if (entity == null)
            {
                return Task.CompletedTask;
            }
            _contexto.Entry(entity).CurrentValues.SetValues(modelo);
            _contexto.SaveChanges();
            return Task.CompletedTask;
        }

        public Task Deletar<T>(T modelo)
        {
            _contexto.Remove(modelo);
            return Task.FromResult(_contexto.SaveChanges());
        }
    }
}
