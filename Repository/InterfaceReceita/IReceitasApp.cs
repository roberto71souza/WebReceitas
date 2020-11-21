using Dominio;
using System;
using System.Collections.Generic;

namespace Repository.InterfaceReceita
{
    public interface IReceitasApp
    {
        public IEnumerable<Receita> ListarReceitas();
        public Receita BuscaID(int id);
        public void AdicionarReceita(Receita modelo);
        public void Atualizar(Receita modelo);
        public void DeletarReceita(Receita modelo);
    }
}
