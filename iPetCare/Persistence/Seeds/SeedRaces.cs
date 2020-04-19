using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Seeds
{
    public class SeedRaces
    {
        public static async Task Seed(DataContext context)
        {
            if (!context.Races.Any())
            {
                var species = await context.Species.ToListAsync();

                var dogId = species.Single(s => s.Name.Equals("Pies")).Id;
                var catId = species.Single(s => s.Name.Equals("Kot")).Id;
                var rabbitId = species.Single(s => s.Name.Equals("Królik")).Id;

                var races = new List<Race>
                {
                    new Race
                    {
                        SpeciesId = dogId,
                        Name = "Owczarek niemiecki"
                    },
                    new Race
                    {
                        SpeciesId = dogId,
                        Name = "Owczarek szkocki"
                    },
                    new Race
                    {
                        SpeciesId = dogId,
                        Name = "Beagle"
                    },
                    new Race
                    {
                        SpeciesId = dogId,
                        Name = "Jamnik"
                    },
                    new Race
                    {
                        SpeciesId = catId,
                        Name = "Maine Coon"
                    },
                    new Race
                    {
                        SpeciesId = catId,
                        Name = "Perski"
                    },
                    new Race
                    {
                        SpeciesId = catId,
                        Name = "Syjamski"
                    },
                    new Race
                    {
                        SpeciesId = catId,
                        Name = "Brytyjski"
                    },
                    new Race
                    {
                        SpeciesId = rabbitId,
                        Name = "Mini Rex"
                    },
                    new Race
                    {
                        SpeciesId = rabbitId,
                        Name = "Baran angielski"
                    },
                    new Race
                    {
                        SpeciesId = rabbitId,
                        Name = "Jersey Wooly"
                    },
                    new Race
                    {
                        SpeciesId = rabbitId,
                        Name = "English Spot"
                    },
                };

                context.Races.AddRange(races);
                await context.SaveChangesAsync();
            }
        }

    }
}
