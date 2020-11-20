using Dominio;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Repository.InterfaceReceita
{
    public class ReceitasApp : IReceitasApp
    {
        public ReceitasContext _contexto { get; set; }

        public ReceitasApp(ReceitasContext contexto)
        {
            _contexto = contexto;
        }

        public IEnumerable<Receita> ListarReceitas()
        {
            IQueryable<Receita> lista = _contexto.Receitas.Include(x => x.Usuario).AsNoTracking().OrderBy(d => d.Id);
            return lista.ToArray();
        }

        public void AdicionarReceita(Receita modelo)
        {
            _contexto.Receitas.Add(modelo);
            _contexto.Entry(modelo.Usuario).State = EntityState.Unchanged;
            _contexto.SaveChanges();
        }

        public Receita BuscaID(int id)
        {
            var result = _contexto.Receitas.Where(x => x.Id == id).Include(d => d.Usuario).AsNoTracking().FirstOrDefault();
            return result;
        }

        public void Atualizar(Receita modelo)
        {
            var entity = _contexto.Receitas.Find(modelo.Id);
            if (entity == null)
            {
                return;
            }
            _contexto.Entry(entity).CurrentValues.SetValues(modelo);

            _contexto.SaveChanges();
        }

        public void DeletarReceita(Receita modelo)
        { 
            _contexto.Receitas.Remove(modelo);
            _contexto.SaveChanges();
        }

    }
}
