namespace EnergySimulator.Models;

public class Usina
{
    public string Nome { get; set; }
    public double CapacidadeMinima { get; set; }
    public double CapacidadeMaxima { get; set; }
    public double EnergiaGerada { get; set; }
    public double ReducaoPercentual { get; private set; } = 0;

    public Usina(string nome, double capacidadeMinima, double capacidadeMaxima)
    {
        Nome = nome;
        CapacidadeMinima = capacidadeMinima;
        CapacidadeMaxima = capacidadeMaxima;
        EnergiaGerada = capacidadeMaxima;  // Inicializa gerando no máximo
    }

    // Simula geração de energia entre capacidade mínima e máxima
    public void SimularGeracaoRandomica(Random random)
    {
        EnergiaGerada = CapacidadeMinima + random.NextDouble() * (CapacidadeMaxima - CapacidadeMinima);
        EnergiaGerada *= (1 - ReducaoPercentual);
    }

    public void ReduzirGeracao(double percentual)
    {
        ReducaoPercentual = percentual;
        EnergiaGerada = EnergiaGerada * (1 - percentual);
    }

    public void RestaurarGeracao()
    {
        ReducaoPercentual = 0;
        EnergiaGerada = CapacidadeMaxima;
    }
}
