using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Seeds
{
    public class SeedVetPets
    {
        public static async Task Seed(DataContext context)
        {
            if (context.Pets.Any() && context.Vets.Any())
            {
                var vets = await context.Vets.ToListAsync();
                var pets = await context.Pets.ToListAsync();

                var vetPets = new List<VetPet>();

                var random = new Random();
                foreach (var pet in pets)
                {
                    var randomizedId = random.Next(0, vets.Count);
                    var vetPet = new VetPet()
                    {
                        Pet = pet,
                        Vet = vets[randomizedId],
                    };
                    vetPets.Add(vetPet);
                }

                context.AddRange(vetPets);
                await context.SaveChangesAsync();
            }
        }
    }
}
