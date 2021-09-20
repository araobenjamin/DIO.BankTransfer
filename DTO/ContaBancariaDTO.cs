using System;
using BankTransfer.Enums;

namespace BankTransfer.DTO
{
    public record ContaBancariaDTO
    {
        public String NomePessoa { get; init; }
        public TipoPessoa TipoPessoa { get; init; }
        public String Id { get; init; }
        public Decimal Saldo { get; init; }
        public Decimal Credito { get; init; }
    }
}