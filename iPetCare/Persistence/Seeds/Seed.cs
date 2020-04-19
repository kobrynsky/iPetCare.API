using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Seeds
{
    public class Seed
    {
        public static async Task SeedData(DataContext context, UserManager<ApplicationUser> userManager)
        {
            await SeedUsers.Seed(userManager, context);
            await SeedInstitutions.Seed(context);
            await SeedVets.Seed(context);
            await SeedOwners.Seed(context);
            await SeedSpecies.Seed(context);
            await SeedRaces.Seed(context);
            await SeedPets.Seed(context);
            await SeedPetOwners.Seed(context);
        }
    }
}