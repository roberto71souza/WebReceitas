using Dominio;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public interface IReceitasRepository
    {
        public Task<IEnumerable<Receita>> Listar();
        public Task<IEnumerable<Receita>> ListarReceitasUsuario(int id);
        public Task<Receita> BuscaID(int id);
        public Task Adicionar(Receita modelo);
        public Task Atualizar(Receita modelo);
        public Task Deletar(Receita modelo);
    }
}
