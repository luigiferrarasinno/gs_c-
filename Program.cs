using System;
using System.Collections.Generic;
using System.Threading;
using EnergySimulator.Models;
using EnergySimulator.Services;

class Program
{
    static void Main()
    {
        var cidades = new List<Cidade>
        {
            new Cidade("São Paulo", 1200),
            new Cidade("Campinas", 800),
            new Cidade("Santos", 600),
            new Cidade("Ribeirão Preto", 500)
        };

        var usinas = new List<Usina>
        {
            new Usina("Itaipu", 1400, 1600),
            new Usina("Jirau", 900, 1100),
            new Usina("Belo Monte", 1100, 1300)
        };

        var gerenciador = new GerenciadorEnergia();
        var simulador = new Simulador();
        var log = new LogOperacoes();

        int horaDoDia = 0;
        int rodada = 0;

        Console.WriteLine("Simulação iniciada! Pressione CTRL+C para sair.\n");

        while (true)
        {
            rodada++;
            horaDoDia = rodada % 24;

            // 1. Simular falhas nas usinas
            gerenciador.SimularFalhasUsinas(usinas);

            // 2. Simular geração das usinas no horário atual
            simulador.SimularGeracao(usinas, horaDoDia);

            // 3. Obter energia total gerada
            double energiaGerada = simulador.ObterEnergiaTotal(usinas);

            // 4. Simular consumo das cidades no horário
            simulador.SimularConsumo(cidades, horaDoDia);

            // 5. Log energia armazenada antes da rodada
            log.RegistrarEnergiaAntes(gerenciador.EnergiaArmazenada);
            log.RegistrarEnergiaGerada(energiaGerada);

            // 6. Distribuir energia gerada entre cidades
            gerenciador.DistribuirEnergia(cidades, energiaGerada);

            // 7. Redistribuir energia armazenada para cidades com déficit
            gerenciador.RedistribuirEnergia(cidades, log);

            // 8. Armazenar excedentes das cidades
            gerenciador.ArmazenarExcedentes(cidades, log);

            // 9. Mostrar logs com histórico
            log.MostrarLog(cidades, gerenciador.EnergiaArmazenada, rodada);

            Thread.Sleep(3000); // 3 segundos por rodada
        }
    }
}
