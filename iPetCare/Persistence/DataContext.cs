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
        public DbSet<Institution> Institutions { get; set; }
        public DbSet<Note> Notes { get; set; }
        public DbSet<Owner> Owners { get; set; }
        public DbSet<Pet> Pets { get; set; }
        public DbSet<Race> Races { get; set; }
        public DbSet<Request> Requests { get; set; }
        public DbSet<Species> Species { get; set; }
        public DbSet<Vet> Vets { get; set; }

        // association tables
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

            var converter = new EnumToStringConverter<Gender>();

            // resolve enum with strings
            builder
                .Entity<Pet>()
                .Property(e => e.Gender)
                .HasConversion(converter);

            builder.Entity<Vet>()
                .HasOne(x => x.User)
                .WithOne(x => x.Vet)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Owner>()
                .HasOne(x => x.User)
                .WithOne(x => x.Owner)
                .OnDelete(DeleteBehavior.Cascade);

            // association tables
            builder.Entity<InstitutionVet>().HasKey(x => new {x.InstitutionId, x.VetId});
            builder.Entity<OwnerPet>().HasKey(x => new {x.OwnerId, x.PetId});
            builder.Entity<VetPet>().HasKey(x => new {x.VetId, x.PetId});
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseLazyLoadingProxies();
        }
    }
}