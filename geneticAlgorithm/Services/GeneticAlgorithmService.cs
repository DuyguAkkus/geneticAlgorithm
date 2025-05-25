using geneticAlgorithm.Models;

namespace geneticAlgorithm.Services;



public class GeneticAlgorithmService
{
    private readonly Random _random = new();

    public double CalculateFitness(double x, double y)
    {
        return Math.Pow(x + 2 * y - 7, 2) + Math.Pow(2 * x + y - 5, 2);
    }

    public Chromosome GenerateRandomChromosome()
    {
        return new Chromosome
        {
            X = _random.NextDouble() * 20 - 10, // [-10, 10]
            Y = _random.NextDouble() * 20 - 10
        };
    }

    public List<Chromosome> InitializePopulation(int size)
    {
        var population = new List<Chromosome>();

        for (int i = 0; i < size; i++)
        {
            var individual = GenerateRandomChromosome();
            individual.Fitness = CalculateFitness(individual.X, individual.Y);
            population.Add(individual);
        }

        return population;
    }

    public GAResult RunGA(GAParameters parameters)
    {
        var fitnessHistory = new List<double>();
        var population = InitializePopulation(parameters.PopulationSize);
        Chromosome best = population.OrderBy(p => p.Fitness).First();

        fitnessHistory.Add(best.Fitness); // İlk nesil için

        for (int generation = 0; generation < parameters.MaxGenerations; generation++)
        {
            var newPopulation = new List<Chromosome>();

            while (newPopulation.Count < parameters.PopulationSize)
            {
                var parent1 = TournamentSelection(population);
                var parent2 = TournamentSelection(population);

                Chromosome child1, child2;
                if (_random.NextDouble() < parameters.CrossoverRate)
                    (child1, child2) = Crossover(parent1, parent2);
                else
                {
                    child1 = new Chromosome { X = parent1.X, Y = parent1.Y };
                    child2 = new Chromosome { X = parent2.X, Y = parent2.Y };
                }

                Mutate(child1, parameters.MutationRate);
                Mutate(child2, parameters.MutationRate);

                child1.Fitness = CalculateFitness(child1.X, child1.Y);
                child2.Fitness = CalculateFitness(child2.X, child2.Y);

                newPopulation.Add(child1);
                newPopulation.Add(child2);
            }

            population = newPopulation;
            var currentBest = population.OrderBy(p => p.Fitness).First();

            if (currentBest.Fitness < best.Fitness)
                best = currentBest;

            fitnessHistory.Add(best.Fitness); // Her jenerasyon sonunda en iyi fitness'ı ekliyoruz
        }

        return new GAResult
        {
            BestSolution = best,
            BestFitnessOverGenerations = fitnessHistory,
            LastGeneration = population // son nesildeki bireyler
        };

    }

    private Chromosome TournamentSelection(List<Chromosome> population)
    {
        var a = population[_random.Next(population.Count)];
        var b = population[_random.Next(population.Count)];
        return a.Fitness < b.Fitness ? a : b;
    }

    private (Chromosome, Chromosome) Crossover(Chromosome p1, Chromosome p2)
    {
        double alpha = _random.NextDouble();
        var child1 = new Chromosome
        {
            X = alpha * p1.X + (1 - alpha) * p2.X,
            Y = alpha * p1.Y + (1 - alpha) * p2.Y
        };
        var child2 = new Chromosome
        {
            X = alpha * p2.X + (1 - alpha) * p1.X,
            Y = alpha * p2.Y + (1 - alpha) * p1.Y
        };
        return (child1, child2);
    }

    private void Mutate(Chromosome c, double mutationRate)
    {
        if (_random.NextDouble() < mutationRate)
            c.X = _random.NextDouble() * 20 - 10;

        if (_random.NextDouble() < mutationRate)
            c.Y = _random.NextDouble() * 20 - 10;
    }
}