using System;
using System.Collections.Generic;

namespace EnergySimulator.Services
{
    public class Logger
    {
        public void Log(string mensagem)
        {
            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] {mensagem}");
        }

        public void LogLinhaSeparadora()
        {
            Console.WriteLine(new string('=', 60));
        }

        public void LogTitulo(string titulo)
        {
            Console.WriteLine();
            Console.WriteLine($" {titulo} ");
            Console.WriteLine(new string('-', titulo.Length + 6));
        }

        public void LogMetasCidades(List<(string nome, double valor)> metas)
        {
            LogTitulo("Metas das Cidades");
            foreach (var item in metas)
            {
                Console.WriteLine($"🏙️ {item.nome.PadRight(15)}  →  {item.valor,7:F2} un");
            }
            Console.WriteLine();
        }

        public void LogGeracaoUsinas(List<(string nome, double valor)> geracoes)
        {
            LogTitulo("Geração das Usinas");
            double total = 0;
            foreach (var item in geracoes)
            {
                Console.WriteLine($"⚡ {item.nome.PadRight(20)}  →  {item.valor,7:F2} un");
                total += item.valor;
            }
            Console.WriteLine($"⚡ Total gerado: {total:F2} un");
            Console.WriteLine();
        }

        public void LogFluxoEnergia(List<(string cidade, double meta, double recebida, double deficit)> dados)
        {
            LogTitulo("Fluxo de Energia das Cidades");
            foreach (var item in dados)
            {
                string status = item.deficit > 0
                    ? $"⚠️  Déficit: {item.deficit:F2} un"
                    : "✅ Meta atingida";

                Console.WriteLine($"🏘️ {item.cidade.PadRight(15)} | Meta: {item.meta,7:F2} un | Recebida: {item.recebida,7:F2} un | {status}");
            }
            Console.WriteLine();
        }

        public void LogStatusGeral(double metaTotal, double geradoTotal, double enviadoTotal, double deficitTotal, double armazenamentoAntes, double armazenamentoDepois)
        {
            LogTitulo("Resumo Geral da Rodada");

            Console.WriteLine($"📊 Meta total das cidades:  {metaTotal:F2} un");
            Console.WriteLine($"⚡ Energia total gerada:     {geradoTotal:F2} un");
            Console.WriteLine($"📦 Energia total enviada:   {enviadoTotal:F2} un");

            if (deficitTotal > 0)
            {
                Console.WriteLine($"❌ Déficit total:            {deficitTotal:F2} un");

                double energiaUsadaArmazenamento = armazenamentoAntes - armazenamentoDepois;

                if (energiaUsadaArmazenamento > 0)
                {
                    Console.WriteLine($"🔋 Energia usada do armazenamento: {energiaUsadaArmazenamento:F2} un");
                }

                if (armazenamentoDepois <= 0 && deficitTotal > energiaUsadaArmazenamento)
                {
                    Console.WriteLine($"🚨 Déficit não pôde ser totalmente suprido. Energia insuficiente no armazenamento.");
                }
                else
                {
                    Console.WriteLine($"✅ Déficit foi totalmente suprido pelo armazenamento.");
                }
            }
            else
            {
                Console.WriteLine($"✅ Todas as metas foram atingidas sem déficit.");
            }

            Console.WriteLine();
        }

        public void LogEnergiaArmazenada(double energia)
        {
            LogLinhaSeparadora();
            Console.WriteLine($"🔋 Energia armazenada total: {energia:F2} un");
            LogLinhaSeparadora();
            Console.WriteLine();
        }
        
        public void LogMensagemCentralizada(string mensagem)
        {
            int width = 60;
            int margem = (width - mensagem.Length) / 2;
            if (margem < 0) margem = 0;
            Console.WriteLine(new string(' ', margem) + mensagem);
        }

    }
}
