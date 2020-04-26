using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Seeds
{
    public class SeedVets
    {
        public static async Task Seed(DataContext context)
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
    }
}
