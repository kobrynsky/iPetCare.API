using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Seeds
{
    public class SeedInstitutions
    {
        public static async Task Seed(DataContext context)
        {
            if (!context.Institutions.Any())
            {
                var institutions = new List<Institution>
                {
                    new Institution
                    {
                        Id = Guid.Parse("635c9ded-4261-4e42-86e2-09a7f72cae46"),
                        Address = "Kalisz ul Nowowiejska 4C",
                        Name = "Przychodnia Kaliska"
                    },
                    new Institution
                    {
                        Id = Guid.Parse("58951f15-854b-4bc4-a396-82c396765c42"),
                        Address = "Warszawa ul Starowislna 55",
                        Name = "Przychodnia Stoleczna"
                    },
                    new Institution
                    {
                        Id = Guid.Parse("fb988055-797d-46e9-b1c9-531dcce8c8dd"),
                        Address = "Krakow ul Mickiewicza 12/3",
                        Name = "Krakuskie bidy"
                    },
                };
                context.Institutions.AddRange(institutions);
                await context.SaveChangesAsync();
            }
        }
    }
}
