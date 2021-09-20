using System;
using System.Collections.Generic;
using System.Linq;
using BankTransfer.DTO;
using BankTransfer.Entidades;
using BankTransfer.Enums;
using BankTransfer.Repositorios;

namespace BankTransfer
{
    public class Aplicacao
    {
        ContaBancariaRepo contasRepo = new ContaBancariaRepo();

        public void Principal()
        {
            String opcao = "";

            while (opcao.ToUpper() != "X")
            {
                opcao = ColetarOpcao();

                switch (opcao.ToUpper())
                {
                    case "1":
                        ListarContas();
                        break;
                    case "2":
                        CadastrarConta();
                        break;
                    case "3":
                        Sacar();
                        break;
                    case "4":
                        Depositar();
                        break;
                    case "5":
                        Transferir();
                        break;
                    case "C":
                        LimparTela();
                        break;
                }
            }
        }

        private void ListarContas()
        {
            System.Console.WriteLine("\nContas Bancárias");
            System.Console.WriteLine("----------------");

            foreach (var conta in contasRepo.Listar())
            {
                System.Console.WriteLine($"Conta # {conta.Id} \t| Nome: {conta.NomePessoa} \t| {conta.TipoPessoa.ToString()} \t| Saldo: ${conta.Saldo} \t| Credito: ${conta.Credito}");
            }
        }

        private void CadastrarConta()
        {
            System.Console.Write("\nDigite o nome da pessoa: ");
            String nome = Console.ReadLine();
            if (nome.Length < 3)
            {
                System.Console.WriteLine("\nERRO: Nome deve conter ao menos 3 dígitos.");
                System.Console.WriteLine("Pressione ENTER para continuar...");
                Console.ReadLine();
                return;
            }

            Console.Write($"Digite {(Int32)TipoPessoa.Fisica} para Pessoa Física e {(Int32)TipoPessoa.Juridica} para Pessoa Juridica: ");
            String tipoPessoa = Console.ReadLine();
            if (!Int32.TryParse(tipoPessoa, out Int32 tipo))
            {
                System.Console.WriteLine("\nERRO: Apenas números são permitidos.");
                System.Console.WriteLine("Pressione ENTER para continuar...");
                Console.ReadLine();
                return;
            }

            if (tipo != (Int32)TipoPessoa.Fisica && tipo != (Int32)TipoPessoa.Juridica)
            {
                System.Console.WriteLine("\nERRO: Tipo inexistente.");
                System.Console.WriteLine("Pressione ENTER para continuar...");
                Console.ReadLine();
                return;
            }

            Console.Write("Digite o número de conta desejado: ");
            String numeroConta = Console.ReadLine();
            if (numeroConta.Length < 1)
            {
                System.Console.WriteLine("\nERRO: Número da conta deve conter ao menos 1 dígito.");
                System.Console.WriteLine("Pressione ENTER para continuar...");
                Console.ReadLine();
                return;
            }

            if (contasRepo.ExisteId(numeroConta))
            {
                System.Console.WriteLine("\nERRO: Número de conta já existente.");
                System.Console.WriteLine("Pressione ENTER para continuar...");
                Console.ReadLine();
                return;
            }

            Console.Write("Digite o saldo inicial da conta: ");
            String saldoString = Console.ReadLine();
            if(!Decimal.TryParse(saldoString, out Decimal saldo))
            {
                System.Console.WriteLine("\nERRO: Apenas números são permitidos.");
                System.Console.WriteLine("Pressione ENTER para continuar...");
                Console.ReadLine();
                return;
            }

            Console.Write("Digite o crédito da conta: ");
            String creditoString = Console.ReadLine();
            if(!Decimal.TryParse(creditoString, out Decimal credito))
            {
                System.Console.WriteLine("\nERRO: Apenas números são permitidos.");
                System.Console.WriteLine("Pressione ENTER para continuar...");
                Console.ReadLine();
                return;
            }

            ContaBancariaDTO contaDto = new ContaBancariaDTO()
            {
                NomePessoa = nome,
                TipoPessoa = (TipoPessoa)tipo,
                Id = numeroConta,
                Saldo = saldo,
                Credito = credito
            };

            if(!contasRepo.Adicionar(contaDto))
            {
                System.Console.WriteLine("\nConta não criada.");
                System.Console.WriteLine("Pressione ENTER para continuar...");
                Console.ReadLine();
            }

            System.Console.WriteLine("\nConta criada.");
            System.Console.WriteLine("Pressione ENTER para continuar...");
            Console.ReadLine();
        }

        private void Sacar()
        {
            System.Console.Write("\nDigite a conta da qual sacar (ou X para sair): ");
            String contaId = Console.ReadLine();
            if (contaId.ToUpper() == "X")
            {
                return;
            }

            ContaBancaria conta = contasRepo.Buscar(contaId);
            if (conta is null)
            {
                System.Console.WriteLine("ERRO: Conta bancaria não existe.");
                System.Console.WriteLine("Pressione ENTER para continuar...");
                Console.ReadLine();
                return;
            }
            
            System.Console.WriteLine($"Conta em nome de: {conta.NomePessoa}");
            System.Console.WriteLine($"\nSaldo da conta: {conta.Saldo} | Crédito: {conta.Credito}");

            System.Console.Write("\nDigite o valor a sacar: ");
            if (!Decimal.TryParse(Console.ReadLine(), out Decimal valor))
            {
                System.Console.WriteLine("ERRO: Valor deve conter apenas digitos numéricos e, no máximo, 1 vírgula.");
                System.Console.WriteLine("Pressione ENTER para continuar...");
                Console.ReadLine();
                return;
            }

            if (!conta.Sacar(valor))
            {
                System.Console.WriteLine("Saque negado.");
                System.Console.WriteLine("Pressione ENTER para continuar...");
                Console.ReadLine();
            }

            System.Console.WriteLine($"\nSaldo final da conta: {conta.Saldo} | Crédito: {conta.Credito}");
            System.Console.WriteLine("Pressione ENTER para continuar...");
            Console.ReadLine();
        }

        private void Depositar()
        {
            System.Console.Write("\nDigite a conta para qual depositar (ou X para sair): ");
            String contaId = Console.ReadLine();
            if (contaId.ToUpper() == "X")
            {
                return;
            }

            ContaBancaria conta = contasRepo.Buscar(contaId);
            if (conta is null)
            {
                System.Console.WriteLine("ERRO: Conta bancaria não existe.");
                System.Console.WriteLine("Pressione ENTER para continuar...");
                Console.ReadLine();
                return;
            }
            
            System.Console.WriteLine($"Conta em nome de: {conta.NomePessoa}");
            System.Console.WriteLine($"\nSaldo da conta: {conta.Saldo} | Crédito: {conta.Credito}");

            System.Console.Write("\nDigite o valor a depositar: ");
            if (!Decimal.TryParse(Console.ReadLine(), out Decimal valor))
            {
                System.Console.WriteLine("ERRO: Valor deve conter apenas digitos numéricos e, no máximo, 1 vírgula.");
                System.Console.WriteLine("Pressione ENTER para continuar...");
                Console.ReadLine();
                return;
            }

            conta.Depositar(valor);

            System.Console.WriteLine($"\nSaldo final da conta: {conta.Saldo} | Crédito: {conta.Credito}");
            System.Console.WriteLine("Pressione ENTER para continuar...");
            Console.ReadLine();
        }

        private void Transferir()
        {
            System.Console.Write("\nDigite a conta de origem da transferencia (ou X para sair): ");
            String contaIdOrigem = Console.ReadLine();
            if (contaIdOrigem.ToUpper() == "X")
            {
                return;
            }

            ContaBancaria contaOrigem = contasRepo.Buscar(contaIdOrigem);
            if (contaOrigem is null)
            {
                System.Console.WriteLine("ERRO: Conta bancaria não existe.");
                System.Console.WriteLine("Pressione ENTER para continuar...");
                Console.ReadLine();
                return;
            }
            
            System.Console.WriteLine($"\nConta origem em nome de: {contaOrigem.NomePessoa}");
            System.Console.WriteLine($"Saldo da conta: {contaOrigem.Saldo} | Crédito: {contaOrigem.Credito}");

            System.Console.Write("\nDigite a conta de destino da transferencia (ou X para sair): ");
            String contaIdDestino = Console.ReadLine();
            if (contaIdDestino.ToUpper() == "X")
            {
                return;
            }

            ContaBancaria contaDestino = contasRepo.Buscar(contaIdDestino);
            if (contaDestino is null)
            {
                System.Console.WriteLine("ERRO: Conta bancaria não existe.");
                System.Console.WriteLine("Pressione ENTER para continuar...");
                Console.ReadLine();
                return;
            }
            
            System.Console.WriteLine($"\nConta destino em nome de: {contaDestino.NomePessoa}");
            System.Console.WriteLine($"Saldo da conta: {contaDestino.Saldo} | Crédito: {contaDestino.Credito}");

            System.Console.Write("\nDigite o valor a transferir: ");
            if (!Decimal.TryParse(Console.ReadLine(), out Decimal valor))
            {
                System.Console.WriteLine("ERRO: Valor deve conter apenas digitos numéricos e, no máximo, 1 vírgula.");
                System.Console.WriteLine("Pressione ENTER para continuar...");
                Console.ReadLine();
                return;
            }

            if (!contaOrigem.Transferir(contaDestino, valor))
            {
                System.Console.WriteLine("Transferência negada.");
                System.Console.WriteLine("Pressione ENTER para continuar...");
                Console.ReadLine();
                return;
            }

            System.Console.WriteLine($"\nSaldo final da conta origem : {contaOrigem.Saldo} \t| Crédito: {contaOrigem.Credito}");
            System.Console.WriteLine($"Saldo final da conta destino: {contaDestino.Saldo} \t| Crédito: {contaDestino.Credito}");
            System.Console.WriteLine("Pressione ENTER para continuar...");
            Console.ReadLine();
        }

        private void LimparTela()
        {
            Console.Clear();
        }

        private String ColetarOpcao()
        {
            MostrarOpcoes();

            System.Console.Write("\nDigite a opcao desejada: ");
            return Console.ReadLine();
        }

        private void MostrarOpcoes()
        {
            System.Console.WriteLine();
            System.Console.WriteLine("\n------------------------");
            System.Console.WriteLine("---      Opções      ---");
            System.Console.WriteLine("------------------------");
            System.Console.WriteLine("1 - Listar Contas");
            System.Console.WriteLine("2 - Cadastrar nova Conta");
            System.Console.WriteLine("3 - Sacar");
            System.Console.WriteLine("4 - Depositar");
            System.Console.WriteLine("5 - Transferir");
            System.Console.WriteLine("C - Limpar Tela");
            System.Console.WriteLine("X - Terminar programa");
        }
    }
}