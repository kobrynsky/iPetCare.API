using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class Race
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        public int SpeciesId { get; set; }
        public virtual Species Species { get; set; }

    }
}
