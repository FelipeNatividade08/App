using App.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.Interfaces.Application
{
    public interface ICidadeService
    {
        Cidade BuscaPorCep(string cep);
        void Remover(Guid id);
        void Salvar(Cidade obj);
        List<Cidade> ListaCidades(string cep, string nome);


    }
}
