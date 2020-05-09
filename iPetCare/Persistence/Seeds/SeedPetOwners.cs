using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Seeds
{
    public class SeedPetOwners
    {
        public static async Task Seed(DataContext context)
        {
            if (!context.OwnerPets.Any())
            {
                var owners = await context.Owners.ToListAsync();
                var pets = await context.Pets.OrderBy(p => p.Id).ToListAsync();

                var ownerPets = new List<OwnerPet>();

                var random = new Random();

                foreach (var pet in pets)
                {
                    var randomizedId = random.Next(0, owners.Count);
                    ownerPets.Add(new OwnerPet
                    {
                        PetId = pet.Id,
                        OwnerId = owners[randomizedId].Id,
                        MainOwner = true
                    });
                }

                context.OwnerPets.AddRange(ownerPets);
                await context.SaveChangesAsync();
            }
        }

    }
}
