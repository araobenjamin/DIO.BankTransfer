using System;
using System.Collections.Generic;
using System.Linq;
using BankTransfer.Entidades;
using BankTransfer.DTO;

namespace BankTransfer.Repositorios
{
    public class ContaBancariaRepo
    {
        private List<ContaBancaria> lista = new ();

        public ContaBancariaRepo()
        {
            // TODO - Buscar contas de repositório persistente
        }

        public Boolean Adicionar(ContaBancariaDTO contaDTO)
        {
            if (lista.Exists(c => c.Id == contaDTO.Id))
            {
                System.Console.WriteLine("Numero de conta já existe.");
                return false;
            }

            ContaBancaria conta = new ContaBancaria(contaDTO.NomePessoa, contaDTO.Id, contaDTO.TipoPessoa, contaDTO.Saldo, contaDTO.Credito);
            lista.Add(conta);
            return true;
        }

        public List<ContaBancaria> Listar() =>
            new List<ContaBancaria>(lista);

        public ContaBancaria Buscar(String id) =>
            lista.FirstOrDefault(c => c.Id == id);

        public Boolean ExisteId(String id)
        {
            return lista.Exists(c => c.Id == id);
        }
    }
}