using Domain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Persistence
{
    public class DataContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Examination> Examinations { get; set; }
        public DbSet<ExaminationParameter> ExaminationParameters { get; set; }
        public DbSet<ExaminationParameterValue> ExaminationParameterValues { get; set; }
        public DbSet<ExaminationType> ExaminationTypes { get; set; }
        public DbSet<ImportantDate> ImportantDates { get; set; }
        public DbSet<Institution> Institutions { get; set; }
        public DbSet<Note> Notes { get; set; }
        public DbSet<Owner> Owners { get; set; }
        public DbSet<Pet> Pets { get; set; }
        public DbSet<Race> Races { get; set; }
        public DbSet<Request> Requests { get; set; }
        public DbSet<Species> Species { get; set; }
        public DbSet<Vet> Vets { get; set; }

        // association tables
        public DbSet<ImportantDatePet> ImportantDatePets { get; set; }
        public DbSet<InstitutionVet> InstitutionVets { get; set; }
        public DbSet<OwnerPet> OwnerPets { get; set; }
        public DbSet<VetPet> VetPets { get; set; }


        public DataContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<ApplicationUser>()
                .HasKey(x => x.Id);

            var converter = new EnumToStringConverter<GenderEnum>();

            // resolve enum with strings
            builder
                .Entity<Pet>()
                .Property(e => e.Gender)
                .HasConversion(converter);

            // association tables
            builder.Entity<ImportantDatePet>().HasKey(x => new {x.ImportantDateId, x.PetId});
            builder.Entity<InstitutionVet>().HasKey(x => new {x.InstitutionId, x.VetId});
            builder.Entity<OwnerPet>().HasKey(x => new {x.OwnerId, x.PetId});
            builder.Entity<VetPet>().HasKey(x => new {x.VetId, x.PetId});
        }
    }
}