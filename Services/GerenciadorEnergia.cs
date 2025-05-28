using System;
using System.Collections.Generic;
using EnergySimulator.Models;

namespace EnergySimulator.Services
{
    public class GerenciadorEnergia
    {
        public List<Cidade> Cidades { get; private set; }
        public List<Usina> Usinas { get; private set; }
        public Armazenamento Armazenamento { get; private set; }
        private Logger _logger;
        private int rodada;

        public GerenciadorEnergia(List<Cidade> cidades, List<Usina> usinas, Armazenamento armazenamento, Logger logger)
        {
            Cidades = cidades;
            Usinas = usinas;
            Armazenamento = armazenamento;
            _logger = logger;
            rodada = 0;
        }

        public void Rodar()
        {
            rodada++;

            Console.Clear();
            _logger.LogLinhaSeparadora();
            _logger.LogMensagemCentralizada($"RODADA {rodada}");
            _logger.LogLinhaSeparadora();

            // Atualizar metas e gerações
            foreach (var c in Cidades)
                c.AtualizarMeta();
            foreach (var u in Usinas)
                u.AtualizarGeracao();

            var metas = new List<(string, double)>();
            foreach (var c in Cidades)
                metas.Add((c.Nome, c.MetaEnergia));
            _logger.LogMetasCidades(metas);

            var geracoes = new List<(string, double)>();
            foreach (var u in Usinas)
                geracoes.Add((u.Nome, u.GeracaoAtual));
            _logger.LogGeracaoUsinas(geracoes);

            // Estatísticas gerais
            double metaTotal = Cidades.Sum(c => c.MetaEnergia);
            double geradoTotal = Usinas.Sum(u => u.GeracaoAtual);
            double enviadoTotal = 0;
            double deficitTotal = 0;
            double armazenamentoAntes = Armazenamento.EnergiaArmazenada;

            foreach (var c in Cidades)
                c.ResetEnergiaRecebida();

            var fluxo = new List<(string cidade, double meta, double recebida, double deficit)>();

            foreach (var cidade in Cidades)
            {
                double energiaNecessaria = cidade.MetaEnergia;
                double energiaFornecida = 0;

                foreach (var usina in Usinas)
                {
                    if (energiaNecessaria <= 0) break;

                    double enviada = usina.EnviarEnergia(energiaNecessaria);
                    energiaNecessaria -= enviada;
                    energiaFornecida += enviada;
                }

                if (energiaNecessaria > 0)
                {
                    double usada = Armazenamento.UsarEnergia(energiaNecessaria);
                    energiaFornecida += usada;
                    energiaNecessaria -= usada;
                }

                cidade.ReceberEnergia(energiaFornecida);
                enviadoTotal += energiaFornecida;

                double deficitCidade = energiaNecessaria > 0 ? energiaNecessaria : 0;
                deficitTotal += deficitCidade;

                fluxo.Add((cidade.Nome, cidade.MetaEnergia, cidade.EnergiaRecebida, deficitCidade));
            }

            // Energia sobrando das usinas para armazenamento
            double sobraTotal = Usinas.Sum(u => u.GeracaoAtual);
            if (sobraTotal > 0)
            {
                Armazenamento.Armazenar(sobraTotal);
            }

            _logger.LogFluxoEnergia(fluxo);

            _logger.LogStatusGeral(
                metaTotal,
                geradoTotal,
                enviadoTotal,
                deficitTotal,
                armazenamentoAntes,
                Armazenamento.EnergiaArmazenada
            );

            _logger.LogEnergiaArmazenada(Armazenamento.EnergiaArmazenada);
        }


       
    }
}
