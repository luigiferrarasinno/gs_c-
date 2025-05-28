namespace EnergySimulator.Models
{
    public class Usina
    {
        public string Nome { get; set; }
        public double Geracao { get; set; }

        public Usina(string nome)
        {
            Nome = nome;
            Geracao = 0;
        }
    }
}
