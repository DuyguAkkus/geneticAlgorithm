namespace geneticAlgorithm.Models
{
    public class GAParameters
    {
        public int PopulationSize { get; set; }
        public int MaxGenerations { get; set; }
        public double CrossoverRate { get; set; }
        public double MutationRate { get; set; }
    }
}
