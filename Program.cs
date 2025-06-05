using System;
using System.Collections.Generic;
using System.Threading;
using EnergySimulator.Models;
using EnergySimulator.Services;

namespace EnergySimulator
{
    class Program
    {
        static void Main(string[] args)
        {
            var logger = new Logger();

            var cidades = new List<Cidade>
            {
                new Cidade("São Paulo"),
                new Cidade("Rio de Janeiro"),
                new Cidade("Belo Horizonte")
            };

            var usinas = new List<Usina>
            {
                new Usina("Usina Hidrelétrica 1"),
                new Usina("Usina Hidrelétrica 2")
            };

            var gerenciador = new GerenciadorEnergia(cidades, usinas, logger);

            // Menu e Login obrigatório
            while (true)
            {
                try
                {
                    Console.Clear();
                    Console.WriteLine("=======================================");
                    Console.WriteLine("           Bem-vindo, usuário          ");
                    Console.WriteLine("=======================================");
                    Console.WriteLine("Digite 1 para logar:");

                    string entrada = Console.ReadLine()?.Trim();

                    if (entrada == "1")
                    {
                        if (FazerLogin())
                        {
                            Console.WriteLine("\nLogin realizado com sucesso!");
                            Thread.Sleep(1000);
                            break;
                        }
                        else
                        {
                            Console.WriteLine("\nUsuário ou senha incorretos. Pressione qualquer tecla para tentar novamente.");
                            Console.ReadKey();
                        }
                    }
                    else
                    {
                        Console.WriteLine("\nOpção inválida. Você precisa digitar 1 para logar. Pressione qualquer tecla para tentar novamente.");
                        Console.ReadKey();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"\nErro inesperado: {ex.Message}. Pressione qualquer tecla para tentar novamente.");
                    Console.ReadKey();
                }
            }

            // Menu principal pós-login
            while (true)
            {
                try
                {
                    Console.Clear();
                    Console.WriteLine("=======================================");
                    Console.WriteLine("             Menu Principal            ");
                    Console.WriteLine("=======================================");
                    Console.WriteLine("Digite:");
                    Console.WriteLine("1 - Simulação de Geração e Distribuição de Energia");
                    Console.WriteLine("0 - Sair");

                    string opcao = Console.ReadLine()?.Trim();

                    switch (opcao)
                    {
                        case "1":
                            IniciarSimulacao(gerenciador);
                            break;
                        case "0":
                            Console.WriteLine("Saindo do programa...");
                            Thread.Sleep(1000);
                            return;
                        default:
                            Console.WriteLine("Opção inválida. Pressione qualquer tecla para tentar novamente.");
                            Console.ReadKey();
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"\nErro inesperado: {ex.Message}. Pressione qualquer tecla para tentar novamente.");
                    Console.ReadKey();
                }
            }
        }

        static void IniciarSimulacao(GerenciadorEnergia gerenciador)
        {
            Console.Clear();
            Console.WriteLine("Iniciando simulação... Pressione Ctrl+C para sair.\n");

            while (true)
            {
                try
                {
                    gerenciador.ExecutarRodada();

                    Console.WriteLine("\nPróxima rodada em 30 segundos. Pressione Ctrl+C para sair.\n");
                    for (int i = 30; i >= 1; i--)
                    {
                        Console.Write($"\r⏳ {i} segundos... ");
                        Thread.Sleep(1000);
                    }

                    Console.WriteLine();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"\nErro durante simulação: {ex.Message}. Pressione qualquer tecla para continuar.");
                    Console.ReadKey();
                }
            }
        }

        static bool FazerLogin()
        {
            try
            {
                Console.Write("Usuário: ");
                string usuario = Console.ReadLine()?.Trim();

                Console.Write("Senha: ");
                string senha = LerSenha();

                return usuario == "admin" && senha == "admin";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nErro ao tentar fazer login: {ex.Message}");
                return false;
            }
        }

        static string LerSenha()
        {
            string senha = "";
            ConsoleKeyInfo key;

            do
            {
                key = Console.ReadKey(true);

                if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
                {
                    senha += key.KeyChar;
                    Console.Write("*");
                }
                else if (key.Key == ConsoleKey.Backspace && senha.Length > 0)
                {
                    senha = senha.Substring(0, senha.Length - 1);
                    Console.Write("\b \b");
                }
            } while (key.Key != ConsoleKey.Enter);

            Console.WriteLine();
            return senha;
        }
    }
}
