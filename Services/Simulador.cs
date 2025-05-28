using System;
using System.Collections.Generic;
using EnergySimulator.Models;

namespace EnergySimulator.Services;

public class Simulador
{
    private readonly Random _random = new();

    // ✅ Simula geração das usinas com variação horária
    public void SimularGeracao(List<Usina> usinas, int horaDoDia)
    {
        double fatorPico = 1.0 + 0.3 * Math.Sin((horaDoDia / 24.0) * 2 * Math.PI);

        foreach (var usina in usinas)
        {
            double geracaoBase = usina.CapacidadeMinima + _random.NextDouble() * (usina.CapacidadeMaxima - usina.CapacidadeMinima);
            usina.EnergiaGerada = geracaoBase * fatorPico;
        }
    }

    // ✅ Simula falhas nas usinas
    public void SimularFalhasUsinas(List<Usina> usinas)
    {
        foreach (var usina in usinas)
        {
            double chanceFalha = 0.1; // 10% de chance de falha
            if (_random.NextDouble() < chanceFalha)
            {
                double reducao = 0.3 + _random.NextDouble() * 0.5; // Redução entre 30% e 80%
                usina.ReduzirGeracao(reducao);
                Console.WriteLine($"⚠️  Usina {usina.Nome} teve uma falha e reduziu sua geração em {(reducao * 100):F0}%.");
            }
            else
            {
                usina.RestaurarGeracao();
            }
        }
    }

    // ✅ Total de energia gerada
    public double ObterEnergiaTotal(List<Usina> usinas)
    {
        double total = 0;
        foreach (var u in usinas)
            total += u.EnergiaGerada;
        return total;
    }

    // ✅ Simula o consumo das cidades
    public void SimularConsumo(List<Cidade> cidades, int horaDoDia)
    {
        foreach (var cidade in cidades)
        {
            double fatorConsumo = 1.0 + 0.4 * Math.Sin((horaDoDia / 24.0) * 2 * Math.PI + Math.PI / 4);
            cidade.ConsumoEstimadoBase = cidade.ConsumoEstimadoBaseOriginal;
            cidade.ConsumoEstimado = cidade.ConsumoEstimadoBase * fatorConsumo;
        }
    }

    // ✅ Distribui energia, cuida de armazenamento e registra logs
    public double DistribuirEnergia(
        List<Cidade> cidades,
        double energiaTotalGerada,
        double energiaArmazenada,
        LogOperacoes log)
    {
        double energiaDisponivel = energiaTotalGerada;

        foreach (var cidade in cidades)
        {
            cidade.ResetarEnergiaRodada();

            // 1️⃣ Suprir com energia gerada
            if (energiaDisponivel >= cidade.ConsumoEstimado)
            {
                cidade.EnergiaRecebida = cidade.ConsumoEstimado;
                energiaDisponivel -= cidade.ConsumoEstimado;
            }
            else
            {
                cidade.EnergiaRecebida = energiaDisponivel;
                double deficit = cidade.ConsumoEstimado - energiaDisponivel;
                energiaDisponivel = 0;

                // 2️⃣ Suprir com energia armazenada
                if (energiaArmazenada >= deficit)
                {
                    cidade.EnergiaVindaDoArmazenamento = deficit;
                    energiaArmazenada -= deficit;
                    log.RegistrarSaida(cidade.Nome, deficit);
                }
                else
                {
                    cidade.EnergiaVindaDoArmazenamento = energiaArmazenada;
                    log.RegistrarSaida(cidade.Nome, energiaArmazenada);
                    energiaArmazenada = 0;
                }
            }

            // 3️⃣ Calcular saldo
            cidade.CalcularDisponivel();

            // 4️⃣ Armazenar excedente da cidade
            if (cidade.EnergiaDisponivel > 0)
            {
                cidade.EnergiaArmazenada = cidade.EnergiaDisponivel;
                energiaArmazenada += cidade.EnergiaDisponivel;
                log.RegistrarEntrada(cidade.Nome, cidade.EnergiaDisponivel);
            }
        }

        return energiaArmazenada;
    }
}
