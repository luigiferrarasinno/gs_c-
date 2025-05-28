namespace EnergySimulator.Models;

public class Cidade
{
    public string Nome { get; set; }
    public double ConsumoEstimadoBaseOriginal { get; set; } // Consumo base fixo original
    public double ConsumoEstimadoBase { get; set; }          // Atualizado a cada rodada pelo Simulador
    public double ConsumoEstimado { get; set; }              // Com variação horário (Simulador)

    public double EnergiaRecebida { get; set; }               // Recebida das usinas
    public double EnergiaDisponivel { get; private set; }     // Saldo final (positivo, zero ou negativo)

    public double EnergiaVindaDoArmazenamento { get; set; }   // 🔋 Quanto precisou buscar do armazenamento
    public double EnergiaArmazenada { get; set; }             // 📦 Quanto conseguiu armazenar (se sobrou)

    public Cidade(string nome, double consumoEstimado)
    {
        Nome = nome;
        ConsumoEstimadoBaseOriginal = consumoEstimado;
        ConsumoEstimadoBase = consumoEstimado;
        ConsumoEstimado = consumoEstimado;

        EnergiaRecebida = 0;
        EnergiaDisponivel = 0;
        EnergiaVindaDoArmazenamento = 0;
        EnergiaArmazenada = 0;
    }

    public void CalcularDisponivel()
    {
        EnergiaDisponivel = EnergiaRecebida + EnergiaVindaDoArmazenamento - ConsumoEstimado;
    }

    public void ResetarEnergiaRodada()
    {
        EnergiaRecebida = 0;
        EnergiaDisponivel = 0;
        EnergiaVindaDoArmazenamento = 0;
        EnergiaArmazenada = 0;
    }
}
