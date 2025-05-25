namespace geneticAlgorithm.Models;

public class GAResult
{
    public Chromosome BestSolution { get; set; } // En iyi birey
    public List<double> BestFitnessOverGenerations { get; set; } = new(); // Her nesildeki en iyi fitness
    public List<Chromosome> LastGeneration { get; set; } = new();

}