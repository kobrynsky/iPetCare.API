using Domain.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Seeds
{
    class SeedUsers
    {
        public static async Task Seed(UserManager<ApplicationUser> userManager, DataContext context)
        {
            if (!userManager.Users.Any())
            {
                var users = new List<ApplicationUser>
                {
                    new ApplicationUser
                    {
                        Id = "f71308b6-e185-44ff-997d-86bc23f849e9",
                        FirstName = "Admin",
                        LastName = "Admin",
                        UserName = "Admin",
                        Email = "admin@admin.com",
                        Role = "Administrator"
                    },

                    // vets

                    new ApplicationUser
                    {
                        Id = "acad4a1d-3287-4c5a-bb05-6a62a9ae6eb8",
                        FirstName = "Jan",
                        LastName = "Niezbedny",
                        UserName = "jnie",
                        Email = "vet1@admin.com",
                        Role = "Vet"
                    },
                    new ApplicationUser
                    {
                        Id = "eebb22c2-1184-4511-8b5e-6737c5a8ecaa",
                        FirstName = "Andrzej",
                        LastName = "Zolnierowski",
                        UserName = "azol",
                        Email = "vet2@admin.com",
                        Role = "Vet"
                    },
                    new ApplicationUser
                    {
                        Id = "f53d66f5-289e-425b-9c21-92da74122d38",
                        FirstName = "Roman",
                        LastName = "Birdman",
                        UserName = "rbir",
                        Email = "vet3@admin.com",
                        Role = "Vet"
                    },
                    new ApplicationUser
                    {
                        Id = "5a9487ed-c7d9-4275-b0e3-d6708f1d4654",
                        FirstName = "Zbigniew",
                        LastName = "Prezes",
                        UserName = "zpre",
                        Email = "vet4@admin.com",
                        Role = "Vet"
                    },
                    new ApplicationUser
                    {
                        Id = "bc93228a-9535-4417-ae3d-0ccb550380c2",
                        FirstName = "Janusz",
                        LastName = "Bierny",
                        UserName = "jbie",
                        Email = "vet5@admin.com",
                        Role = "Vet"
                    },
                    new ApplicationUser
                    {
                        Id = "0b1d6b8b-8e78-4e7a-996f-bd4688e424d0",
                        FirstName = "Piotr",
                        LastName = "Patronus",
                        UserName = "ppat",
                        Email = "vet6@admin.com",
                        Role = "Vet"
                    },

                    // owners
                    new ApplicationUser
                    {
                        Id = "16f567aa-77ff-40c3-b317-716eba0c58b4",
                        FirstName = "Jolanta",
                        LastName = "Pepiczek",
                        UserName = "jpep",
                        Email = "owner1@admin.com",
                        Role = "Owner"
                    },
                    new ApplicationUser
                    {
                        Id = "df70d5a3-7a67-407b-995c-52c0a4f711c1",
                        FirstName = "Anna",
                        LastName = "Niezawodna",
                        UserName = "anie",
                        Email = "owner2@admin.com",
                        Role = "Owner"
                    },
                    new ApplicationUser
                    {
                        Id = "6ee7969c-195c-493c-b0a0-430af66e69cf",
                        FirstName = "Maria",
                        LastName = "Wesołowska",
                        UserName = "mwes",
                        Email = "owner3@admin.com",
                        Role = "Owner"
                    },
                    new ApplicationUser
                    {
                        Id = "27877f13-f540-4398-8db0-c67a0c5a646f",
                        FirstName = "Joanna",
                        LastName = "Grzebowska",
                        UserName = "jgrz",
                        Email = "owner4@admin.com",
                        Role = "Owner"
                    },
                };

                foreach (var user in users)
                {
                    await userManager.CreateAsync(user, "Haslo123!");
                }
            }
        }
    }
}
