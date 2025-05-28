using EnergySimulator.Models;

namespace EnergySimulator.Database;

public static class BancoDeDados
{
    public static List<Cidade> Cidades = new();
    public static List<Usina> Usinas = new();
    public static List<Estado> Estados = new();

    public static void PopularDados()
    {
        // Usinas com capacidade mínima e máxima
        Usinas.Add(new Usina("Usina Hidrelétrica", 1000, 2000));
        Usinas.Add(new Usina("Usina Solar", 500, 1500));
        Usinas.Add(new Usina("Usina Eólica", 300, 1000));

        // Estado e Cidades
        var estado = new Estado("São Paulo");
        estado.AdicionarCidade(new Cidade("São Paulo", 1200));
        estado.AdicionarCidade(new Cidade("Campinas", 800));
        estado.AdicionarCidade(new Cidade("Santos", 600));
        estado.AdicionarCidade(new Cidade("Ribeirão Preto", 500));

        Estados.Add(estado);
        Cidades.AddRange(estado.Cidades);
    }
}
