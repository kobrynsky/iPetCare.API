using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Seeds
{
    public class SeedExaminations
    {
        public static async Task Seed(DataContext context)
        {
            if (!context.Examinations.Any())
            {
                var pets = await context.Pets.ToListAsync();
                var examinationTypes = await context.ExaminationTypes.ToListAsync();
                var species = pets.Select(x => x.Race.Species).Distinct();

                var examinations = new List<Examination>();
                var random = new Random();

                foreach (var speciesUnit in species)
                {
                    var speciesExaminationTypes = examinationTypes.Where(x => x.SpeciesId == speciesUnit.Id).ToList();
                    foreach (var pet in pets.Where(x => x.Race.SpeciesId == speciesUnit.Id))
                    {
                        var randomizedId = random.Next(0, speciesExaminationTypes.Count);
                        var examinationType = speciesExaminationTypes[randomizedId];
                        var examinationValues = new List<ExaminationParameterValue>();

                        foreach (var parameter in examinationType.ExaminationParameters)
                        {
                            examinationValues.Add(new ExaminationParameterValue()
                            {
                                ExaminationParameter = parameter,
                                Value = random.Next((int)parameter.LowerLimit, (int)parameter.UpperLimit),
                            });
                        }

                        examinations.Add(new Examination()
                        {
                            Pet = pet,
                            Date = DateTime.Now,
                            ExaminationParameterValues = examinationValues,
                            ExaminationType = examinationType,
                        });

                    }
                }

                context.AddRange(examinations);
                await context.SaveChangesAsync().ConfigureAwait(false);
            }
        }
    }
}
