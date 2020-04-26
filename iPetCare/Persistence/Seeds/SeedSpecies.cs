using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Seeds
{
    public class SeedSpecies
    {
        public static async Task Seed(DataContext context)
        {
            if (!context.Species.Any())
            {
                var species = new List<Species>
                {
                    new Species
                    {
                        Name = "Pies",
                    },
                    new Species
                    {
                        Name = "Kot",
                    },
                    new Species
                    {
                        Name = "Królik",
                    }
                };

                context.Species.AddRange(species);
                await context.SaveChangesAsync();
            }
        }
    }
}
