using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    public class Seed
    {
        public static async Task SeedData(DataContext context, UserManager<ApplicationUser> userManager)
        {
            await SeedUsers(userManager, context);
            await SeedInstitutions(context);
            await SeedVets(context);
            await SeedOwners(context);
            await SeedSpecies(context);
            await SeedRaces(context);
            await SeedPets(context);
            await SeedPetOwners(context);
        }

        private static async Task SeedRaces(DataContext context)
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

        private static async Task SeedPetOwners(DataContext context)
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
                        OwnerId = owners[randomizedId].Id
                    });
                }

                //
                // var list = new List<OwnerPet>
                // {
                //     new OwnerPet
                //     {
                //         OwnerId = context.Owners.OrderBy(r => Guid.NewGuid()).First().Id,
                //         PetId = context.Pets.OrderBy(r => Guid.NewGuid()).First().Id,
                //     },
                //     new OwnerPet
                //     {
                //         OwnerId = context.Owners.OrderBy(r => Guid.NewGuid()).First().Id,
                //         PetId = context.Pets.OrderBy(r => Guid.NewGuid()).First().Id,
                //     },
                //     new OwnerPet
                //     {
                //         OwnerId = context.Owners.OrderBy(r => Guid.NewGuid()).First().Id,
                //         PetId = context.Pets.OrderBy(r => Guid.NewGuid()).First().Id,
                //     },
                //     new OwnerPet
                //     {
                //         OwnerId = context.Owners.OrderBy(r => Guid.NewGuid()).First().Id,
                //         PetId = context.Pets.OrderBy(r => Guid.NewGuid()).First().Id,
                //     },
                //     new OwnerPet
                //     {
                //         OwnerId = context.Owners.OrderBy(r => Guid.NewGuid()).First().Id,
                //         PetId = context.Pets.OrderBy(r => Guid.NewGuid()).First().Id,
                //     },
                //     new OwnerPet
                //     {
                //         OwnerId = context.Owners.OrderBy(r => Guid.NewGuid()).First().Id,
                //         PetId = context.Pets.OrderBy(r => Guid.NewGuid()).First().Id,
                //     },
                //     new OwnerPet
                //     {
                //         OwnerId = context.Owners.OrderBy(r => Guid.NewGuid()).First().Id,
                //         PetId = context.Pets.OrderBy(r => Guid.NewGuid()).First().Id,
                //     },
                //     new OwnerPet
                //     {
                //         OwnerId = context.Owners.OrderBy(r => Guid.NewGuid()).First().Id,
                //         PetId = context.Pets.OrderBy(r => Guid.NewGuid()).First().Id,
                //     },
                //     new OwnerPet
                //     {
                //         OwnerId = context.Owners.OrderBy(r => Guid.NewGuid()).First().Id,
                //         PetId = context.Pets.OrderBy(r => Guid.NewGuid()).First().Id,
                //     },
                //     new OwnerPet
                //     {
                //         OwnerId = context.Owners.OrderBy(r => Guid.NewGuid()).First().Id,
                //         PetId = context.Pets.OrderBy(r => Guid.NewGuid()).First().Id,
                //     }
                // };
                context.OwnerPets.AddRange(ownerPets);
                await context.SaveChangesAsync();
            }
        }

        private static async Task SeedPets(DataContext context)
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
                        BirthDate = DateTime.Now.AddDays(-1 * (random.Next(1, 1000))),
                        Height = (float) (random.Next(2, 30) * random.NextDouble()),
                        Weight = (float) (random.Next(2, 30) * random.NextDouble()),
                        Gender = random.NextDouble() > 0.5 ? Gender.Female : Gender.Male,
                        Race = context.Races.OrderBy(r => Guid.NewGuid()).FirstOrDefault(),
                    },
                    new Pet
                    {
                        Id = Guid.Parse("9da3672d-314c-47a2-9c30-d681806922fa"),
                        Name = "Maniek",
                        BirthDate = DateTime.Now.AddDays(-1 * (random.Next(1, 1000))),
                        Height = (float) (random.Next(2, 30) * random.NextDouble()),
                        Weight = (float) (random.Next(2, 30) * random.NextDouble()),
                        Gender = random.NextDouble() > 0.5 ? Gender.Female : Gender.Male,
                        Race = context.Races.OrderBy(r => Guid.NewGuid()).FirstOrDefault(),
                    },
                    new Pet
                    {
                        Id = Guid.Parse("372783ea-9a78-4f6a-a1fe-69d09084876b"),
                        Name = "Benek",
                        BirthDate = DateTime.Now.AddDays(-1 * (random.Next(1, 1000))),
                        Height = (float) (random.Next(2, 30) * random.NextDouble()),
                        Weight = (float) (random.Next(2, 30) * random.NextDouble()),
                        Gender = random.NextDouble() > 0.5 ? Gender.Female : Gender.Male,
                        Race = context.Races.OrderBy(r => Guid.NewGuid()).FirstOrDefault(),
                    },
                    new Pet
                    {
                        Id = Guid.Parse("1966d430-0548-4c1d-ba5e-d99097f9aadb"),
                        Name = "Kapsel",
                        BirthDate = DateTime.Now.AddDays(-1 * (random.Next(1, 1000))),
                        Height = (float) (random.Next(2, 30) * random.NextDouble()),
                        Weight = (float) (random.Next(2, 30) * random.NextDouble()),
                        Gender = random.NextDouble() > 0.5 ? Gender.Female : Gender.Male,
                        Race = context.Races.OrderBy(r => Guid.NewGuid()).FirstOrDefault(),
                    },
                    new Pet
                    {
                        Id = Guid.Parse("2f65bad1-1c55-43c8-bcd8-7c0955f07f7e"),
                        Name = "Wafel",
                        BirthDate = DateTime.Now.AddDays(-1 * (random.Next(1, 1000))),
                        Height = (float) (random.Next(2, 30) * random.NextDouble()),
                        Weight = (float) (random.Next(2, 30) * random.NextDouble()),
                        Gender = random.NextDouble() > 0.5 ? Gender.Female : Gender.Male,
                        Race = context.Races.OrderBy(r => Guid.NewGuid()).FirstOrDefault(),
                    },
                    new Pet
                    {
                        Id = Guid.Parse("2c81dc7e-197f-4f53-b32f-7d8d329b1175"),
                        Name = "Całka",
                        BirthDate = DateTime.Now.AddDays(-1 * (random.Next(1, 1000))),
                        Height = (float) (random.Next(2, 30) * random.NextDouble()),
                        Weight = (float) (random.Next(2, 30) * random.NextDouble()),
                        Gender = random.NextDouble() > 0.5 ? Gender.Female : Gender.Male,
                        Race = context.Races.OrderBy(r => Guid.NewGuid()).FirstOrDefault(),
                    },
                    new Pet
                    {
                        Id = Guid.Parse("3ab414d6-efd9-4555-bfbe-6fb0b6ebad01"),
                        Name = "Gałgan",
                        BirthDate = DateTime.Now.AddDays(-1 * (random.Next(1, 1000))),
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

        private static async Task SeedSpecies(DataContext context)
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
        }

        private static async Task SeedOwners(DataContext context)
        {
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

        private static async Task SeedVets(DataContext context)
        {
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
        }

        private static async Task SeedInstitutions(DataContext context)
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