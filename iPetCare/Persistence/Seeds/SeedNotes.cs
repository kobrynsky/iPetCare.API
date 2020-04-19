using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Seeds
{
    public class SeedNotes
    {
        public static async Task Seed(DataContext context)
        {
            if (!context.Notes.Any())
            {
                var examinations = await context.Examinations.ToListAsync();
                var pets = await context.Pets.ToListAsync();

                var notes = new List<Note>();

                foreach (var examination in examinations.Take(3))
                {
                    notes.Add(new Note()
                    {
                        Pet = examination.Pet,
                        CreatedAt = DateTime.Now,
                        Payload = $"Mój ukochany zwierzaczek {examination.Pet.Name} był na badaniu!",
                        User = examination.Pet.OwnerPets.ElementAt(0).Owner.User,
                    });
                }

                notes.AddRange(new List<Note>()
                {
                    new Note()
                    {
                        Pet = pets[0],
                        CreatedAt = DateTime.Now,
                        Payload = $"Notateczka bez badania u {pets[0].Name}",
                        User = pets[0].OwnerPets.ElementAt(0).Owner.User,
                    },
                    new Note()
                    {
                        Pet = pets[1],
                        CreatedAt = DateTime.Now,
                        Payload = $"Notateczka bez badania u {pets[1].Name}",
                        User = pets[1].OwnerPets.ElementAt(0).Owner.User,
                    }
                });

                context.AddRange(notes);
                await context.SaveChangesAsync().ConfigureAwait(false);
            }
        }
    }
}
