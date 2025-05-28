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

        var simulador = new Simulador();
        var log = new LogOperacoes();

        double energiaArmazenada = 0;
        int rodada = 0;

        Console.WriteLine("⚡ Simulação iniciada! Pressione CTRL+C para sair.\n");

        while (true)
        {
            rodada++;
            int horaDoDia = rodada % 24;

            Console.WriteLine($"\n🕑 Hora: {horaDoDia}h | 🔄 Rodada: {rodada}");

            // 1️⃣ Simular falhas nas usinas
            simulador.SimularFalhasUsinas(usinas);

            // 2️⃣ Simular geração das usinas no horário atual
            simulador.SimularGeracao(usinas, horaDoDia);

            // 3️⃣ Obter energia total gerada
            double energiaGerada = simulador.ObterEnergiaTotal(usinas);

            // 4️⃣ Simular consumo das cidades no horário atual
            simulador.SimularConsumo(cidades, horaDoDia);

            // 5️⃣ Log da energia antes da distribuição
            log.RegistrarEnergiaAntes(energiaArmazenada);
            log.RegistrarEnergiaGerada(energiaGerada);

            // 6️⃣ Distribuir energia e atualizar armazenamento
            energiaArmazenada = simulador.DistribuirEnergia(
                cidades,
                energiaGerada,
                energiaArmazenada,
                log
            );

            // 7️⃣ Mostrar logs e histórico
            log.MostrarLog(cidades, energiaArmazenada, rodada);

            // 8️⃣ Aguarda próxima rodada
            Thread.Sleep(30000); // 30 segundos por rodada
        }
    }
}
