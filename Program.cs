using System;
using BankTransfer.Entidades;
using BankTransfer.Enums;
using BankTransfer.Repositorios;

namespace BankTransfer
{
    class Program
    {
        static void Main(string[] args)
        {
            Aplicacao app = new Aplicacao();
            app.Principal();
        }
    }
}
