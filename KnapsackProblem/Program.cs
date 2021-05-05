using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GAF;
using GAF.Operators;

namespace KnapsackProblem
{
    class Program
    {
        static List<Bag.Item> knapsackItems;

        static void Main(string[] args)
        {
            knapsackItems = new List<Bag.Item>();
            knapsackItems.Add(new Bag.Item() { Name = "a", Weight = 9, Value = 150 });
            knapsackItems.Add(new Bag.Item() { Name = "b", Weight = 153, Value = 200 });
            knapsackItems.Add(new Bag.Item() { Name = "c", Weight = 13, Value = 35 });
            knapsackItems.Add(new Bag.Item() { Name = "d", Weight = 50, Value = 160 });
            knapsackItems.Add(new Bag.Item() { Name = "e", Weight = 15, Value = 60 });
            knapsackItems.Add(new Bag.Item() { Name = "f", Weight = 68, Value = 45 });
            knapsackItems.Add(new Bag.Item() { Name = "g", Weight = 27, Value = 60 });
            knapsackItems.Add(new Bag.Item() { Name = "h", Weight = 39, Value = 40 });
            knapsackItems.Add(new Bag.Item() { Name = "i", Weight = 23, Value = 30 });
            knapsackItems.Add(new Bag.Item() { Name = "j", Weight = 52, Value = 10 });
            knapsackItems.Add(new Bag.Item() { Name = "k", Weight = 11, Value = 70 });
            knapsackItems.Add(new Bag.Item() { Name = "l", Weight = 32, Value = 30 });
            knapsackItems.Add(new Bag.Item() { Name = "m", Weight = 24, Value = 15 });
            knapsackItems.Add(new Bag.Item() { Name = "n", Weight = 48, Value = 10 });
            knapsackItems.Add(new Bag.Item() { Name = "o", Weight = 73, Value = 40 });
            knapsackItems.Add(new Bag.Item() { Name = "p", Weight = 42, Value = 70 });
            knapsackItems.Add(new Bag.Item() { Name = "q", Weight = 22, Value = 80 });
            knapsackItems.Add(new Bag.Item() { Name = "r", Weight = 7, Value = 20 });
            knapsackItems.Add(new Bag.Item() { Name = "s", Weight = 18, Value = 12 });
            knapsackItems.Add(new Bag.Item() { Name = "t", Weight = 4, Value = 50 });
            knapsackItems.Add(new Bag.Item() { Name = "u", Weight = 30, Value = 10 });
            knapsackItems.Add(new Bag.Item() { Name = "v", Weight = 43, Value = 75 });
            knapsackItems.Add(new Bag.Item() { Name = "v", Weight = 43, Value = 75 });
            knapsackItems.Add(new Bag.Item() { Name = "v", Weight = 43, Value = 75 });
            knapsackItems.Add(new Bag.Item() { Name = "v", Weight = 43, Value = 75 });

            const double crossoverProbability = 0.65;
            const double mutationProbability = 0.08;
            const int elitismPercentage = 5;

            // tao quan the
            var population = new Population(100, 22, false, false);

            // tao toan tu di truyen 
            var elite = new Elite(elitismPercentage);

            var crossover = new Crossover(crossoverProbability, true)
            {
                CrossoverType = CrossoverType.SinglePoint
            };
            var mutation = new BinaryMutate(mutationProbability, true);

            // tao GA
            var ga = new GeneticAlgorithm(population, EvaluateFitness);
         
            ga.OnGenerationComplete += ga_OnGenerationComplete;

            // them tung toan tu vao GA 
            ga.Operators.Add(elite);
            ga.Operators.Add(crossover);
            ga.Operators.Add(mutation);

            // chay GA 
            ga.Run(TerminateAlgorithm);
            
            // lay do vat tot nhat
            var Bestchromosome = ga.Population.GetTop(1)[0];

            // decode chromosome
            Console.WriteLine("Maximum value of knapsack contains these Items : "); 
            Bag BestBag = new Bag();
            for (int i = 0; i < Bestchromosome.Count; i++)
            {
                if (Bestchromosome.Genes[i].BinaryValue == 1)
                {
                    Console.WriteLine(knapsackItems[i].ToString());
                    BestBag.AddItem(knapsackItems[i]); 
                }
            }
            Console.WriteLine(" Best knapsack information:\n Total Weight : {0}   Total Value : {1} Fitness : {2}", BestBag.TotalWeight, BestBag.TotalValue, Bestchromosome.Fitness);
            Console.ReadKey(); 
        }

        public static double EvaluateFitness(Chromosome chromosome)
        {
            double fitnessValue = 0;
          
            //decode chromosome
            Bag bag = new Bag();
            for (int i = 0; i < chromosome.Count; i++)
            {
                if (chromosome.Genes[i].BinaryValue == 1)
                {
                    bag.AddItem(knapsackItems[i]);
                }
            }   
            if (bag.TotalWeight <= 400)
            {
                fitnessValue = (bag.TotalValue / Convert.ToDouble(10000));
            }

            return fitnessValue;
        }

        public static bool TerminateAlgorithm(Population population, int currentGeneration, long currentEvaluation)
        {
            return currentGeneration > 50;
        }

        private static void ga_OnGenerationComplete(object sender, GaEventArgs e)
        {
            // lay giai phap tot nhat
            var chromosome = e.Population.GetTop(1)[0];

            //decode chromosome
            Bag bag = new Bag();
            for (int i = 0; i < chromosome.Count; i++)
            {
                if (chromosome.Genes[i].BinaryValue == 1)
                {
                    bag.AddItem(knapsackItems[i]); 
                }
            }
            Console.WriteLine(" Generation info: Total Weight : {0}   Total Value : {1} Fitness : {2}", bag.TotalWeight, bag.TotalValue , e.Population.MaximumFitness);
        }
    }
}
