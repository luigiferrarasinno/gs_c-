using System;
using System.Collections.Generic;
using System.Linq;
using EnergySimulator.Models;

namespace EnergySimulator.Services
{
    public class GerenciadorEnergia
    {
        private readonly List<Cidade> _cidades;
        private readonly List<Usina> _usinas;
        private readonly Logger _logger;
        private double _energiaArmazenada;
        private const double CapacidadeMaximaArmazenamento = 500;
        private int _rodada = 1;
        private readonly Random _random = new Random();

        public GerenciadorEnergia(List<Cidade> cidades, List<Usina> usinas, Logger logger)
        {
            _cidades = cidades;
            _usinas = usinas;
            _logger = logger;
            _energiaArmazenada = 200;
        }

        public void ExecutarRodada()
        {
            double energiaArmazenadaInicio = _energiaArmazenada;

            _logger.LogLinhaSeparadora();
            _logger.LogMensagemCentralizada($"RODADA {_rodada}");
            _logger.LogLinhaSeparadora();

            // Verificar crise
            bool temCrise = _rodada % 2 == 0;
            bool criseNaGeracao = false;
            bool criseNaDemanda = false;

            if (temCrise)
            {
                criseNaGeracao = _random.Next(0, 2) == 1;
                criseNaDemanda = _random.Next(0, 2) == 1;

                if (!criseNaGeracao && !criseNaDemanda)
                    criseNaGeracao = true;
            }

            // Gerar metas das cidades
            foreach (var cidade in _cidades)
            {
                double metaBase = _random.NextDouble() * (120 - 80) + 80;
                cidade.MetaEnergia = criseNaDemanda
                    ? metaBase * (_random.NextDouble() * (1.5 - 1.2) + 1.2)
                    : metaBase;
            }

            double totalMetaCidades = _cidades.Sum(c => c.MetaEnergia);
            _logger.LogLista("Metas das Cidades", _cidades.Select(c => (c.Nome, c.MetaEnergia)).ToList(), "un");
            _logger.Log($"🔢 Total de energia demandada pelas cidades: {totalMetaCidades:F2} un");

            // Gerar energia das usinas
            foreach (var usina in _usinas)
            {
                double geracaoBase = _random.NextDouble() * (200 - 150) + 150;
                usina.Geracao = criseNaGeracao
                    ? geracaoBase * (_random.NextDouble() * (0.7 - 0.5) + 0.5)
                    : geracaoBase;
            }

            double totalGerado = _usinas.Sum(u => u.Geracao);

            _logger.LogLista("Geração das Usinas", _usinas.Select(u => (u.Nome, u.Geracao)).ToList(), "un");

            // Verifica se há crise energética
            if (totalGerado < totalMetaCidades)
            {
                _logger.Log("🚨 CRISE ENERGÉTICA DETECTADA! A geração não é suficiente para suprir a demanda.");
            }


            // ===========================
            // Distribuição de Energia
            // ===========================
            double energiaGeradaDisponivel = totalGerado;
            double energiaArmazenadaDisponivel = _energiaArmazenada;

            var fluxo = new List<(string cidade, double meta, double recebida, bool metaAtingida)>();

            foreach (var cidade in _cidades)
            {
                double energiaNecessaria = cidade.MetaEnergia;
                double energiaEntregue = 0;
                double veioDaGeracao = 0;
                double veioDoArmazenamento = 0;

                // Usa geração primeiro
                if (energiaGeradaDisponivel >= energiaNecessaria)
                {
                    veioDaGeracao = energiaNecessaria;
                    energiaEntregue = energiaNecessaria;
                    energiaGeradaDisponivel -= energiaNecessaria;
                }
                else
                {
                    veioDaGeracao = energiaGeradaDisponivel;
                    energiaEntregue = energiaGeradaDisponivel;
                    energiaNecessaria -= energiaGeradaDisponivel;
                    energiaGeradaDisponivel = 0;

                    // Complementa com armazenamento
                    if (energiaArmazenadaDisponivel >= energiaNecessaria)
                    {
                        veioDoArmazenamento = energiaNecessaria;
                        energiaEntregue += energiaNecessaria;
                        energiaArmazenadaDisponivel -= energiaNecessaria;
                        energiaNecessaria = 0;
                    }
                    else
                    {
                        veioDoArmazenamento = energiaArmazenadaDisponivel;
                        energiaEntregue += energiaArmazenadaDisponivel;
                        energiaNecessaria -= energiaArmazenadaDisponivel;
                        energiaArmazenadaDisponivel = 0;
                    }
                }

                bool metaAtingida = energiaEntregue >= cidade.MetaEnergia;
                fluxo.Add((cidade.Nome, cidade.MetaEnergia, energiaEntregue, metaAtingida));

               if (veioDoArmazenamento > 0)
                {
                    _logger.Log($"📦 {cidade.Nome} recebeu {veioDoArmazenamento:F2} un do armazenamento.");
                    
                    if (metaAtingida)
                    {
                        _logger.Log("   ✅ O armazenamento foi suficiente para complementar.");
                    }
                }

                if (!metaAtingida)
                {
                    _logger.Log($"⚠️ Faltou energia para {cidade.Nome}!");
                    _logger.Log($"   🔸 Recebeu {veioDaGeracao:F2} un da geração.");
                    _logger.Log($"   🔸 Recebeu {veioDoArmazenamento:F2} un do armazenamento.");
                    _logger.Log($"   ❌ Mesmo com armazenamento, faltaram {energiaNecessaria:F2} un.");
                }

            }

            _logger.LogFluxoEnergia(fluxo);

            // ===========================
            // Atualização do Armazenamento
            // ===========================
            double sobra = energiaGeradaDisponivel;

            if (sobra > 0)
            {
                double espacoDisponivel = CapacidadeMaximaArmazenamento - energiaArmazenadaDisponivel;
                double energiaParaArmazenar = Math.Min(sobra, espacoDisponivel);
                energiaArmazenadaDisponivel += energiaParaArmazenar;
            }

            _energiaArmazenada = energiaArmazenadaDisponivel;

            _logger.LogLinhaSeparadora();
            _logger.Log($"🔋 Energia armazenada no início da rodada: {energiaArmazenadaInicio:F2} un");
            _logger.Log($"🔋 Energia armazenada no final da rodada: {_energiaArmazenada:F2} un");
            _logger.LogLinhaSeparadora();

            _rodada++;
        }
    }
}
