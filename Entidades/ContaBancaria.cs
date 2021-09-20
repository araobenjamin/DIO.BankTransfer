using System;
using BankTransfer.Enums;

namespace BankTransfer.Entidades
{
    public class ContaBancaria : IEntidade
    {
        public String NomePessoa { get; }
        public TipoPessoa TipoPessoa { get; }
        public String Id { get; }
        public Decimal Saldo { get; private set; }
        public Decimal Credito { get; private set; }

        public ContaBancaria(String nome, String numeroConta, TipoPessoa tipoPessoa, Decimal saldo, Decimal credito)
        {
            this.NomePessoa = nome;
            this.TipoPessoa = tipoPessoa;
            this.Id = numeroConta;
            this.Saldo = saldo;
            this.Credito = credito;
        }

        public Boolean Sacar(Decimal valor)
        {
            if (Saldo + Credito < valor)
            {
                System.Console.WriteLine("Saldo insuficiente.");
                return false;
            }

            Saldo -= valor;
            return true;
        }

        public void Depositar(Decimal valor)
        {
            Saldo += valor;
        }

        public Boolean Transferir(ContaBancaria contaDestino, Decimal valor)
        {
            if (!this.Sacar(valor))
            {
                return false;
            }

            contaDestino.Depositar(valor);
            return true;
        }
    }
}