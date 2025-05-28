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

        public void LogMensagemCentralizada(string mensagem)
        {
            int width = 60;
            int margem = (width - mensagem.Length) / 2;
            if (margem < 0) margem = 0;
            Console.WriteLine(new string(' ', margem) + mensagem);
        }

        public void LogLista(string titulo, List<(string nome, double valor)> dados, string unidade = "")
        {
            Console.WriteLine($"\n {titulo}");
            Console.WriteLine(new string('-', 30));

            foreach (var item in dados)
            {
                Console.WriteLine($"{(titulo.Contains("Geração") ? "⚡" : "🏙️")} {item.nome.PadRight(18)} → {item.valor,8:F2} {unidade}");
            }

            if (titulo.Contains("Geração"))
            {
                double total = 0;
                foreach (var item in dados) total += item.valor;
                Console.WriteLine($"⚡ Total gerado: {total:F2} {unidade}");
                
            }

            Console.WriteLine();
        }

        public void LogFluxoEnergia(List<(string cidade, double meta, double recebida, bool metaAtingida)> dados)
        {
            Console.WriteLine(" Fluxo de Energia das Cidades");
            Console.WriteLine(new string('-', 40));

            foreach (var item in dados)
            {
                string status = item.metaAtingida ? "✅ Meta atingida" : "❌ Déficit";
                Console.WriteLine($"🏘️ {item.cidade.PadRight(15)} | Meta: {item.meta,6:F2} un | Recebida: {item.recebida,6:F2} un | {status}");
            }

            Console.WriteLine();
        }
    }
}
