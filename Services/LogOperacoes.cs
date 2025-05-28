using System;
using System.Collections.Generic;
using System.IO;
using EnergySimulator.Models;

namespace EnergySimulator.Services;

public class LogOperacoes
{
    private readonly Dictionary<string, double> _entradas = new();
    private readonly Dictionary<string, double> _saidas = new();
    private double _energiaAntesRodada = 0;
    private double _energiaGerada = 0;

    private readonly List<string> _historico = new();

    public void RegistrarEnergiaAntes(double energiaArmazenada)
    {
        _energiaAntesRodada = energiaArmazenada;
    }

    public void RegistrarEnergiaGerada(double energiaGerada)
    {
        _energiaGerada = energiaGerada;
    }

    public void RegistrarEntrada(string cidade, double quantidade)
    {
        if (_entradas.ContainsKey(cidade))
            _entradas[cidade] += quantidade;
        else
            _entradas[cidade] = quantidade;
    }

    public void RegistrarSaida(string cidade, double quantidade)
    {
        if (_saidas.ContainsKey(cidade))
            _saidas[cidade] += quantidade;
        else
            _saidas[cidade] = quantidade;
    }

    public void MostrarLog(List<Cidade> cidades, double energiaArmazenadaDepois, int rodada)
    {
        var log = new List<string>();

        log.Add($"\n══════════════════════════════════════════════════════════════");
        log.Add($"RODADA #{rodada} - SIMULAÇÃO DE ENERGIA");
        log.Add($"══════════════════════════════════════════════════════════════\n");

        log.Add($"🌟 ENERGIA TOTAL GERADA PELAS USINAS: {_energiaGerada:F2} MW");
        log.Add($"💾 ENERGIA ARMAZENADA ANTES DA RODADA: {_energiaAntesRodada:F2} MW\n");

        log.Add("🏙️  STATUS DAS CIDADES:");
        foreach (var cidade in cidades)
        {
            string detalhes = $"Recebeu: {cidade.EnergiaRecebida,7:F2} | Consumo: {cidade.ConsumoEstimado,7:F2}";

            if (cidade.EnergiaVindaDoArmazenamento > 0)
                detalhes += $" | 🔋 Complementou com {cidade.EnergiaVindaDoArmazenamento:F2} MW da energia armazenada";

            if (cidade.EnergiaDisponivel < 0)
                detalhes += $" | ⚠️ FALTOU -{Math.Abs(cidade.EnergiaDisponivel):F2} MW";
            else if (cidade.EnergiaDisponivel > 0)
                detalhes += $" | SOBROU +{cidade.EnergiaDisponivel:F2} MW";
            else
                detalhes += $" | Saldo: OK";

            log.Add($"- {cidade.Nome,-16} → {detalhes}");
        }

        log.Add("\n──────────────────────────────────────────────────────────────");

        if (_entradas.Count > 0)
        {
            log.Add("📦 OPERAÇÕES DE ARMAZENAMENTO:");
            foreach (var entrada in _entradas)
                log.Add($"✅ {entrada.Key} armazenou +{entrada.Value:F2} MW");
        }

        if (_saidas.Count > 0)
        {
            log.Add("\n⚡ USO DA ENERGIA ARMAZENADA:");
            foreach (var saida in _saidas)
                log.Add($"⚠️ {saida.Key} precisou de +{saida.Value:F2} MW da energia armazenada");
        }

        log.Add($"\n💡 NOVO SALDO DE ENERGIA ARMAZENADA: {energiaArmazenadaDepois:F2} MW");
        log.Add("══════════════════════════════════════════════════════════════\n");

        // Salva histórico para múltiplas rodadas
        _historico.AddRange(log);
        _historico.Add(new string('-', 60));

        // Imprime só as últimas 40 linhas do histórico
        int maxLinhasConsole = 40;
        int linhas = _historico.Count;
        int start = Math.Max(0, linhas - maxLinhasConsole);

        Console.Clear();
        for (int i = start; i < linhas; i++)
            Console.WriteLine(_historico[i]);

        // Apaga registros temporários para próxima rodada
        _entradas.Clear();
        _saidas.Clear();

        // Salva também em arquivo
        File.AppendAllLines("historico_simulacao.log", log);
    }
}
