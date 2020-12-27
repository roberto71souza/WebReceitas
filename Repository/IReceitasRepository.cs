using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public interface IReceitasRepository
    {
        public Task<IEnumerable<object>> Listar();
        public Task<object> ListarReceitasUsuario(int id);
        public Task<object> BuscaID(int id);
        public Task Adicionar(object modelo);
        public Task Atualizar(object modelo);
        public Task Deletar<T>(T modelo);
    }
}
