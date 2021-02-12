using Dominio;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<IEnumerable<Receita>> Listar()
        {
            var lista = await _contexto.Receitas.Include(x => x.Usuario).AsNoTracking().OrderBy(d => d.Id).ToListAsync();
            return lista;
        }

        public async Task<IEnumerable<Receita>> ListarReceitasUsuario(int id)
        {
            var result = await _contexto.Receitas.Where(x => x.Usuario.Id == id).Include(d => d.Usuario).AsNoTracking().ToListAsync();
            return result;
        }

        public async Task<Receita> BuscaID(int id)
        {
            var result = await _contexto.Receitas.Where(x => x.Id == id).Include(d => d.Usuario).AsNoTracking().FirstOrDefaultAsync();
            return result;
        }

        public async Task Adicionar(Receita modelo)
        {
            _contexto.Add(modelo);
           await _contexto.SaveChangesAsync();
        }

        public async Task Atualizar(Receita modelo)
        {
            _contexto.Update(modelo);
            await _contexto.SaveChangesAsync();
        }

        public async Task Deletar(Receita modelo)
        {
            _contexto.Remove(modelo);
           await _contexto.SaveChangesAsync();
        }
    }
}
