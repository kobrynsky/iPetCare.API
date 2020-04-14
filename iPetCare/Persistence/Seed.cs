using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace Persistence
{
    public class Seed
    {
        public static async Task SeedData(DataContext context, UserManager<ApplicationUser> userManager)
        {
            await SeedUsers(userManager, context);
        }

        private static async Task SeedUsers(UserManager<ApplicationUser> userManager, DataContext context)
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

            if (!context.Vets.Any())
            {
                var vets = new List<Vet>
                {
                    new Vet
                    {
                        Id = Guid.Parse("acad4a1d-3287-4c5a-bb05-6a62a9ae6eb8"),
                        Specialization = "Gady",
                        UserId = "acad4a1d-3287-4c5a-bb05-6a62a9ae6eb8",
                        InstitutionVets = new List<InstitutionVet>
                        {
                            new InstitutionVet
                            {
                                VetId = Guid.Parse("acad4a1d-3287-4c5a-bb05-6a62a9ae6eb8"),
                                InstitutionId = Guid.Parse("635c9ded-4261-4e42-86e2-09a7f72cae46")
                            },
                            new InstitutionVet
                            {
                                VetId = Guid.Parse("acad4a1d-3287-4c5a-bb05-6a62a9ae6eb8"),
                                InstitutionId = Guid.Parse("58951f15-854b-4bc4-a396-82c396765c42")
                            },
                            new InstitutionVet
                            {
                                VetId = Guid.Parse("acad4a1d-3287-4c5a-bb05-6a62a9ae6eb8"),
                                InstitutionId = Guid.Parse("fb988055-797d-46e9-b1c9-531dcce8c8dd")
                            }
                        }
                    },
                    new Vet
                    {
                        Id = Guid.Parse("eebb22c2-1184-4511-8b5e-6737c5a8ecaa"),
                        Specialization = "Chomiki, gady",
                        UserId = "eebb22c2-1184-4511-8b5e-6737c5a8ecaa",
                        InstitutionVets = new List<InstitutionVet>
                        {
                            new InstitutionVet
                            {
                                VetId = Guid.Parse("eebb22c2-1184-4511-8b5e-6737c5a8ecaa"),
                                InstitutionId = Guid.Parse("635c9ded-4261-4e42-86e2-09a7f72cae46")
                            },
                        }
                    },
                    new Vet
                    {
                        Id = Guid.Parse("f53d66f5-289e-425b-9c21-92da74122d38"),
                        Specialization = "Psy i koty",
                        UserId = "f53d66f5-289e-425b-9c21-92da74122d38",
                        InstitutionVets = new List<InstitutionVet>
                        {
                            new InstitutionVet
                            {
                                VetId = Guid.Parse("f53d66f5-289e-425b-9c21-92da74122d38"),
                                InstitutionId = Guid.Parse("fb988055-797d-46e9-b1c9-531dcce8c8dd")
                            }
                        }
                    },
                    new Vet
                    {
                        Id = Guid.Parse("5a9487ed-c7d9-4275-b0e3-d6708f1d4654"),
                        Specialization = "Psy i koty",
                        UserId = "5a9487ed-c7d9-4275-b0e3-d6708f1d4654",
                        InstitutionVets = new List<InstitutionVet>
                        {
                            new InstitutionVet
                            {
                                VetId = Guid.Parse("5a9487ed-c7d9-4275-b0e3-d6708f1d4654"),
                                InstitutionId = Guid.Parse("635c9ded-4261-4e42-86e2-09a7f72cae46")
                            },
                        }
                    },
                    new Vet
                    {
                        Id = Guid.Parse("bc93228a-9535-4417-ae3d-0ccb550380c2"),
                        Specialization = "Króliki",
                        UserId = "bc93228a-9535-4417-ae3d-0ccb550380c2",
                        InstitutionVets = new List<InstitutionVet>
                        {
                            new InstitutionVet
                            {
                                VetId = Guid.Parse("bc93228a-9535-4417-ae3d-0ccb550380c2"),
                                InstitutionId = Guid.Parse("635c9ded-4261-4e42-86e2-09a7f72cae46")
                            },
                            new InstitutionVet
                            {
                                VetId = Guid.Parse("bc93228a-9535-4417-ae3d-0ccb550380c2"),
                                InstitutionId = Guid.Parse("fb988055-797d-46e9-b1c9-531dcce8c8dd")
                            }
                        }
                    },
                    new Vet
                    {
                        Id = Guid.Parse("0b1d6b8b-8e78-4e7a-996f-bd4688e424d0"),
                        Specialization = "Króliki, chomiki",
                        UserId = "0b1d6b8b-8e78-4e7a-996f-bd4688e424d0",
                        InstitutionVets = new List<InstitutionVet>
                        {
                            new InstitutionVet
                            {
                                VetId = Guid.Parse("0b1d6b8b-8e78-4e7a-996f-bd4688e424d0"),
                                InstitutionId = Guid.Parse("635c9ded-4261-4e42-86e2-09a7f72cae46")
                            },
                            new InstitutionVet
                            {
                                VetId = Guid.Parse("0b1d6b8b-8e78-4e7a-996f-bd4688e424d0"),
                                InstitutionId = Guid.Parse("58951f15-854b-4bc4-a396-82c396765c42")
                            },
                        }
                    },
                };
                context.Vets.AddRange(vets);
                await context.SaveChangesAsync();
            }

            if (!context.Owners.Any())
            {
                var owners = new List<Owner>
                {
                    new Owner
                    {
                        Id = Guid.Parse("16f567aa-77ff-40c3-b317-716eba0c58b4"),
                        PlaceOfResidence = "Kalisz",
                        UserId = "16f567aa-77ff-40c3-b317-716eba0c58b4",
                    },
                    new Owner
                    {
                        Id = Guid.Parse("df70d5a3-7a67-407b-995c-52c0a4f711c1"),
                        PlaceOfResidence = "Wrocław",
                        UserId = "df70d5a3-7a67-407b-995c-52c0a4f711c1",
                    },
                    new Owner
                    {
                        Id = Guid.Parse("6ee7969c-195c-493c-b0a0-430af66e69cf"),
                        PlaceOfResidence = "Kraków",
                        UserId = "6ee7969c-195c-493c-b0a0-430af66e69cf",
                    },
                    new Owner
                    {
                        Id = Guid.Parse("27877f13-f540-4398-8db0-c67a0c5a646f"),
                        PlaceOfResidence = "Warszawa",
                        UserId = "27877f13-f540-4398-8db0-c67a0c5a646f",
                    },
                };
                context.Owners.AddRange(owners);
                await context.SaveChangesAsync();
            }
        }
    }
}
