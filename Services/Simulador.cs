using System;
using System.Collections.Generic;
using EnergySimulator.Models;

namespace EnergySimulator.Services;

public class Simulador
{
    private Random _random = new();

    // Simula a geração das usinas, com variação diária para simular pico e variação natural
    public void SimularGeracao(List<Usina> usinas, int horaDoDia)
    {
        // Calcula fator de pico diário (seno entre 0.7 e 1.3)
        double fatorPico = 1.0 + 0.3 * Math.Sin((horaDoDia / 24.0) * 2 * Math.PI);

        foreach (var usina in usinas)
        {
            // Simula variação natural da geração entre capacidade mínima e máxima
            double geracaoBase = usina.CapacidadeMinima + _random.NextDouble() * (usina.CapacidadeMaxima - usina.CapacidadeMinima);
            
            // Aplica fator de pico diário
            usina.EnergiaGerada = geracaoBase * fatorPico;
        }
    }

    // Calcula energia total gerada
    public double ObterEnergiaTotal(List<Usina> usinas)
    {
        double total = 0;
        foreach (var u in usinas)
            total += u.EnergiaGerada;
        return total;
    }

    // Simula o consumo estimado das cidades (com padrão diário também)
    public void SimularConsumo(List<Cidade> cidades, int horaDoDia)
    {
        foreach (var cidade in cidades)
        {
            double fatorConsumo = 1.0 + 0.4 * Math.Sin((horaDoDia / 24.0) * 2 * Math.PI + Math.PI / 4);
            cidade.ConsumoEstimadoBase = cidade.ConsumoEstimadoBaseOriginal;
            cidade.ConsumoEstimado = cidade.ConsumoEstimadoBase * fatorConsumo;
        }
    }
}
