namespace EnergySimulator.Models
{
    public class Usina
    {
        public string Nome { get; set; }
        public double GeracaoAtual { get; private set; }
        private Random _rnd;

        public Usina(string nome)
        {
            Nome = nome;
            _rnd = new Random(nome.GetHashCode() + DateTime.Now.Millisecond);
            AtualizarGeracao();
        }

        // Geração variável por rodada, ex: entre 100 e 200
        public void AtualizarGeracao()
        {
            GeracaoAtual = _rnd.NextDouble() * 100 + 100; // Geração entre 100 e 200
        }

        // Envia energia para a cidade (limitado pela geração atual)
        public double EnviarEnergia(double quantidade)
        {
            double energiaEnviada = quantidade > GeracaoAtual ? GeracaoAtual : quantidade;
            GeracaoAtual -= energiaEnviada;
            return energiaEnviada;
        }
    }
}
