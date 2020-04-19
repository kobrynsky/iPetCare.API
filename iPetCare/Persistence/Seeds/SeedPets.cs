using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Seeds
{
    public class SeedPets
    {
        public static async Task Seed(DataContext context)
        {
            if (!context.Pets.Any())
            {
                var random = new Random();
                var pets = new List<Pet>
                {
                    new Pet
                    {
                        Id = Guid.Parse("cdc22c8c-f61b-4a59-9cae-a6de43fb2445"),
                        Name = "Misiek",
                        BirthDate = DateTime.Now.AddDays(-1 * random.Next(1, 1000)),
                        Height = (float) (random.Next(2, 30) * random.NextDouble()),
                        Weight = (float) (random.Next(2, 30) * random.NextDouble()),
                        Gender = random.NextDouble() > 0.5 ? Gender.Female : Gender.Male,
                        Race = context.Races.OrderBy(r => Guid.NewGuid()).FirstOrDefault(),
                    },
                    new Pet
                    {
                        Id = Guid.Parse("9da3672d-314c-47a2-9c30-d681806922fa"),
                        Name = "Maniek",
                        BirthDate = DateTime.Now.AddDays(-1 * random.Next(1, 1000)),
                        Height = (float) (random.Next(2, 30) * random.NextDouble()),
                        Weight = (float) (random.Next(2, 30) * random.NextDouble()),
                        Gender = random.NextDouble() > 0.5 ? Gender.Female : Gender.Male,
                        Race = context.Races.OrderBy(r => Guid.NewGuid()).FirstOrDefault(),
                    },
                    new Pet
                    {
                        Id = Guid.Parse("372783ea-9a78-4f6a-a1fe-69d09084876b"),
                        Name = "Benek",
                        BirthDate = DateTime.Now.AddDays(-1 * random.Next(1, 1000)),
                        Height = (float) (random.Next(2, 30) * random.NextDouble()),
                        Weight = (float) (random.Next(2, 30) * random.NextDouble()),
                        Gender = random.NextDouble() > 0.5 ? Gender.Female : Gender.Male,
                        Race = context.Races.OrderBy(r => Guid.NewGuid()).FirstOrDefault(),
                    },
                    new Pet
                    {
                        Id = Guid.Parse("1966d430-0548-4c1d-ba5e-d99097f9aadb"),
                        Name = "Kapsel",
                        BirthDate = DateTime.Now.AddDays(-1 * random.Next(1, 1000)),
                        Height = (float) (random.Next(2, 30) * random.NextDouble()),
                        Weight = (float) (random.Next(2, 30) * random.NextDouble()),
                        Gender = random.NextDouble() > 0.5 ? Gender.Female : Gender.Male,
                        Race = context.Races.OrderBy(r => Guid.NewGuid()).FirstOrDefault(),
                    },
                    new Pet
                    {
                        Id = Guid.Parse("2f65bad1-1c55-43c8-bcd8-7c0955f07f7e"),
                        Name = "Wafel",
                        BirthDate = DateTime.Now.AddDays(-1 * random.Next(1, 1000)),
                        Height = (float) (random.Next(2, 30) * random.NextDouble()),
                        Weight = (float) (random.Next(2, 30) * random.NextDouble()),
                        Gender = random.NextDouble() > 0.5 ? Gender.Female : Gender.Male,
                        Race = context.Races.OrderBy(r => Guid.NewGuid()).FirstOrDefault(),
                    },
                    new Pet
                    {
                        Id = Guid.Parse("2c81dc7e-197f-4f53-b32f-7d8d329b1175"),
                        Name = "Całka",
                        BirthDate = DateTime.Now.AddDays(-1 * random.Next(1, 1000)),
                        Height = (float) (random.Next(2, 30) * random.NextDouble()),
                        Weight = (float) (random.Next(2, 30) * random.NextDouble()),
                        Gender = random.NextDouble() > 0.5 ? Gender.Female : Gender.Male,
                        Race = context.Races.OrderBy(r => Guid.NewGuid()).FirstOrDefault(),
                    },
                    new Pet
                    {
                        Id = Guid.Parse("3ab414d6-efd9-4555-bfbe-6fb0b6ebad01"),
                        Name = "Gałgan",
                        BirthDate = DateTime.Now.AddDays(-1 * random.Next(1, 1000)),
                        Height = (float) (random.Next(2, 30) * random.NextDouble()),
                        Weight = (float) (random.Next(2, 30) * random.NextDouble()),
                        Gender = random.NextDouble() > 0.5 ? Gender.Female : Gender.Male,
                        Race = context.Races.OrderBy(r => Guid.NewGuid()).FirstOrDefault(),
                    }
                };

                context.Pets.AddRange(pets);
                await context.SaveChangesAsync();
            }
        }
    }
}
