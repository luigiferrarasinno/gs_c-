﻿using System;
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

            while (true)
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
        }
    }
}
