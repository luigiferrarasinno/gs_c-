namespace EnergySimulator.Models
{
    public class Cidade
    {
        public string Nome { get; set; }
        public double MetaEnergia { get; private set; }
        public double EnergiaRecebida { get; private set; }

        private Random _rnd;

        public Cidade(string nome)
        {
            Nome = nome;
            _rnd = new Random(nome.GetHashCode()); // Semente baseada no nome para variar menos repetido
            AtualizarMeta();
        }

        // Meta variável por rodada, por exemplo 80 a 120
        public void AtualizarMeta()
        {
            MetaEnergia = _rnd.NextDouble() * 40 + 80; // Meta entre 80 e 120
            EnergiaRecebida = 0;
        }

    
        // Muda método ReceberEnergia pra acumular energia recebida da rodada
        public void ReceberEnergia(double quantidade)
        {
            EnergiaRecebida += quantidade;
        }

        // Para resetar energia recebida antes da rodada:
        public void ResetEnergiaRecebida()
        {
            EnergiaRecebida = 0;
        }


        public bool EstaSatisfeita()
        {
            return EnergiaRecebida >= MetaEnergia;
        }

        public double EnergiaFaltante()
        {
            return MetaEnergia - EnergiaRecebida;
        }
    }
}
