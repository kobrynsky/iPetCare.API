using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Seeds
{
    public class SeedExaminationParameters
    {
        public static async Task Seed(DataContext context)
        {
            if (!context.ExaminationParameters.Any())
            {
                var examinationTypes = await context.ExaminationTypes.ToListAsync();
                var examinationParameters = new List<ExaminationParameter>();
                var random = new Random();

                var bloodTypes = examinationTypes.Where(x => x.Name == "Badanie krwi");
                foreach (var type in bloodTypes)
                {
                    var randomLimitChanger = random.Next(0, 10);
                    var parameter1 = new ExaminationParameter()
                    {
                        Name = "Płytki krwi",
                        LowerLimit = 100 - (randomLimitChanger * 5),
                        UpperLimit = 200 + (randomLimitChanger * 5),
                    };

                    var parameter2 = new ExaminationParameter()
                    {
                        Name = "Leukocyty",
                        LowerLimit = 20 - (randomLimitChanger * 5),
                        UpperLimit = 50 + (randomLimitChanger * 5),
                    };

                    var parameter3 = new ExaminationParameter()
                    {
                        Name = "Erytrocyty",
                        LowerLimit = 0 + randomLimitChanger,
                        UpperLimit = 11 + randomLimitChanger,
                    };

                    type.ExaminationParameters.Add(parameter1);
                    type.ExaminationParameters.Add(parameter2);
                    type.ExaminationParameters.Add(parameter3);
                }

                var hearTypes = examinationTypes.Where(x => x.Name == "Badanie słuchu");
                foreach (var type in hearTypes)
                {
                    var parameter1 = new ExaminationParameter()
                    {
                        Name = "Poprawność słuchu w lewym uchu",
                        LowerLimit = 0,
                        UpperLimit = 100,
                    };

                    var parameter2 = new ExaminationParameter()
                    {
                        Name = "Poprawność słuchu w prawym uchu",
                        LowerLimit = 0,
                        UpperLimit = 100,
                    };

                    type.ExaminationParameters.Add(parameter1);
                    type.ExaminationParameters.Add(parameter2);
                }

                var stomacheTypes = examinationTypes.Where(x => x.Name == "Badanie układu pokarmowego");
                foreach (var type in stomacheTypes)
                {
                    var randomLimitChanger = random.Next(0, 10);
                    var parameter1 = new ExaminationParameter()
                    {
                        Name = "Flora bakteryjna",
                        LowerLimit = 0,
                        UpperLimit = 100,
                    };

                    var parameter2 = new ExaminationParameter()
                    {
                        Name = "MCHC",
                        LowerLimit = 20 - (randomLimitChanger * 5),
                        UpperLimit = 50 + (randomLimitChanger * 5),
                    };

                    var parameter3 = new ExaminationParameter()
                    {
                        Name = "MCV",
                        LowerLimit = 0 + randomLimitChanger,
                        UpperLimit = 11 + randomLimitChanger,
                    };

                    type.ExaminationParameters.Add(parameter1);
                    type.ExaminationParameters.Add(parameter2);
                    type.ExaminationParameters.Add(parameter3);
                }
                await context.SaveChangesAsync();
            }
        }
    }
}
