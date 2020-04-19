using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Seeds
{
    public class SeedRequests
    {
        public static async Task Seed(DataContext context)
        {
            if (!context.Requests.Any() && context.Vets.Any() && context.Pets.Any() && context.Owners.Any())
            {
                var vets = await context.Vets.ToListAsync();
                var owners = await context.Owners.ToListAsync();
                var pets = await context.Pets.ToListAsync();

                var requests = new List<Request>();

                //Misiek - od Ownera, niezaakceptowane
                requests.Add(new Request()
                {
                    DidUserRequest = true,
                    User = owners.Where(x => x.OwnerPets.All(y => y.PetId == pets[0].Id)).FirstOrDefault().User,
                    Pet = pets[0],
                    IsAccepted = false,
                });

                //Maniek - od Veta, niezaakceptowane
                requests.Add(new Request()
                {
                    DidUserRequest = true,
                    User = vets.Where(x => x.VetPets.All(y => y.PetId == pets[1].Id)).FirstOrDefault().User,
                    Pet = pets[1],
                    IsAccepted = false,
                });

                //Benek - od zwierzaka do veta, niezaakceptowane
                requests.Add(new Request()
                {
                    DidUserRequest = false,
                    User = vets.Where(x => x.VetPets.All(y => y.PetId == pets[2].Id)).FirstOrDefault().User,
                    Pet = pets[2],
                    IsAccepted = false,
                });

                //Kapsel - od zwierzaka do ownera, niezaakceptowane
                requests.Add(new Request()
                {
                    DidUserRequest = false,
                    User = owners.Where(x => x.OwnerPets.All(y => y.PetId == pets[2].Id)).FirstOrDefault().User,
                    Pet = pets[2],
                    IsAccepted = false,
                });

                context.Add(requests);
                await context.SaveChangesAsync();
            }
        }
    }
}
