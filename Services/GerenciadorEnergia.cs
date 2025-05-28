using System;
using System.Collections.Generic;
using EnergySimulator.Models;
using EnergySimulator.Services;

namespace EnergySimulator.Services;

public class GerenciadorEnergia
{
    private const double CapacidadeMaximaArmazenamento = 5000; // Limite máximo de energia armazenada (MW)
    private const double EficiênciaArmazenamento = 0.95;       // Perda de 5% ao armazenar ou recuperar

    public double EnergiaArmazenada { get; private set; } = 0;

    private Random _random = new();

    // Distribui energia gerada entre cidades, tentando evitar déficit
    public void DistribuirEnergia(List<Cidade> cidades, double energiaGerada)
    {
        double energiaPorCidade = energiaGerada / cidades.Count;

        foreach (var cidade in cidades)
        {
            // Distribui energia com variação aleatória +/- 20%
            double variacao = 0.8 + _random.NextDouble() * 0.4; // 0.8 a 1.2
            cidade.EnergiaRecebida = energiaPorCidade * variacao;

            // Ajusta para que normalmente energia recebida seja >= consumo estimado 80% do tempo
            if (_random.NextDouble() < 0.8 && cidade.EnergiaRecebida < cidade.ConsumoEstimado)
            {
                cidade.EnergiaRecebida = cidade.ConsumoEstimado * (1 + _random.NextDouble() * 0.2);
            }

            cidade.CalcularDisponivel();
        }
    }

    // Tenta cobrir déficits das cidades com energia armazenada
    public void RedistribuirEnergia(List<Cidade> cidades, LogOperacoes log)
    {
        foreach (var cidade in cidades)
        {
            if (cidade.EnergiaDisponivel < 0 && EnergiaArmazenada > 0)
            {
                double deficit = -cidade.EnergiaDisponivel;
                double podeUsar = Math.Min(deficit / EficiênciaArmazenamento, EnergiaArmazenada);
                if (podeUsar > 0)
                {
                    double energiaFornecida = podeUsar * EficiênciaArmazenamento;
                    cidade.EnergiaRecebida += energiaFornecida;
                    cidade.CalcularDisponivel();

                    EnergiaArmazenada -= podeUsar;
                    log.RegistrarSaida(cidade.Nome, energiaFornecida);
                }
            }
        }
    }

    // Armazena energia excedente das cidades no sistema, respeitando limite e eficiência
    public void ArmazenarExcedentes(List<Cidade> cidades, LogOperacoes log)
    {
        foreach (var cidade in cidades)
        {
            if (cidade.EnergiaDisponivel > 0)
            {
                double energiaARmazenar = cidade.EnergiaDisponivel / EficiênciaArmazenamento;

                // Verifica se cabe no armazenamento
                double espaçoDisponivel = CapacidadeMaximaArmazenamento - EnergiaArmazenada;
                if (energiaARmazenar > espaçoDisponivel)
                {
                    energiaARmazenar = espaçoDisponivel;
                }

                if (energiaARmazenar > 0)
                {
                    EnergiaArmazenada += energiaARmazenar;
                    cidade.EnergiaRecebida -= cidade.EnergiaDisponivel; // retira o que vai armazenar
                    cidade.CalcularDisponivel();
                    log.RegistrarEntrada(cidade.Nome, energiaARmazenar * EficiênciaArmazenamento);
                }
            }
        }
    }

    // Simula falhas em usinas (redução de geração)
    public void SimularFalhasUsinas(List<Usina> usinas)
    {
        foreach (var usina in usinas)
        {
            double chanceFalha = 0.1; // 10% chance de falha
            if (_random.NextDouble() < chanceFalha)
            {
                double reducao = 0.3 + _random.NextDouble() * 0.5; // 30% a 80% redução
                usina.ReduzirGeracao(reducao);
            }
            else
            {
                usina.RestaurarGeracao();
            }
        }
    }
}
