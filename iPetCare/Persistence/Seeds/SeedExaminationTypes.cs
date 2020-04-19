using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Seeds
{
    public class SeedExaminationTypes
    {
        public static async Task Seed(DataContext context)
        {
            if (!context.ExaminationTypes.Any())
            {
                var species = await context.Species.ToListAsync();
                var examinationTypes = new List<ExaminationType>();

                foreach (var speciesUnit in species)
                {
                    var examinationType1 = new ExaminationType()
                    {
                        Name = "Badanie krwi",
                        Species = speciesUnit,
                    };
                    examinationTypes.Add(examinationType1);

                    var examinationType2 = new ExaminationType()
                    {
                        Name = "Badanie słuchu",
                        Species = speciesUnit,
                    };
                    examinationTypes.Add(examinationType2);

                    var examinationType3 = new ExaminationType()
                    {
                        Name = "Badanie układu pokarmowego",
                        Species = speciesUnit,
                    };
                    examinationTypes.Add(examinationType3);
                }

                context.AddRange(examinationTypes);
                await context.SaveChangesAsync().ConfigureAwait(false);
            }
        }
    }
}
