namespace EnergySimulator.Models
{
    public class Armazenamento
    {
        public double EnergiaArmazenada { get; private set; }

        public Armazenamento()
        {
            EnergiaArmazenada = 0;
        }

        // Armazena energia excedente
        public void Armazenar(double quantidade)
        {
            EnergiaArmazenada += quantidade;
        }

        // Usa energia armazenada para suprir falta, limitado pelo que tem armazenado
        public double UsarEnergia(double quantidade)
        {
            double energiaFornecida = quantidade > EnergiaArmazenada ? EnergiaArmazenada : quantidade;
            EnergiaArmazenada -= energiaFornecida;
            return energiaFornecida;
        }
    }
}
