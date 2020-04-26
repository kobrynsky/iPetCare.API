using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Seeds
{
    public class SeedOwners
    {
        public static async Task Seed(DataContext context)
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

    }
}
