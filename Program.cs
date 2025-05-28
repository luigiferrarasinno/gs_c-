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

            var armazenamento = new Armazenamento();
            var logger = new Logger();
            var gerenciador = new GerenciadorEnergia(cidades, usinas, armazenamento, logger);

            while (true)
            {
                gerenciador.Rodar();

                Console.WriteLine();
                Console.WriteLine("Próxima rodada em 30 segundos. Pressione Ctrl+C para sair.");

                for (int i = 30; i > 0; i--)
                {
                    Console.Write($"\rPróxima rodada em {i} segundos... ");
                    Thread.Sleep(1000);
                }
            }
        }
    }
}
