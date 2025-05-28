namespace EnergySimulator.Models;

public class Estado
{
    public string Nome { get; set; }
    public List<Cidade> Cidades { get; set; }

    public Estado(string nome)
    {
        Nome = nome;
        Cidades = new List<Cidade>();
    }

    public void AdicionarCidade(Cidade cidade)
    {
        Cidades.Add(cidade);
    }
}
