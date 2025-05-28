namespace EnergySimulator.Models
{
    public class Cidade
    {
        public string Nome { get; set; }
        public double MetaEnergia { get; set; }

        public Cidade(string nome)
        {
            Nome = nome;
            MetaEnergia = 0;
        }
    }
}
