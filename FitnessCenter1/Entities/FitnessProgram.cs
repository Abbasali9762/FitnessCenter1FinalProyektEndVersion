using System.ComponentModel.DataAnnotations;
namespace FitnessCenter1.Entities
{
    public class FitnessProgram
    {
        public int FitnessProgramId { get; set; }

        [Required]
        [MaxLength(100)]
        public string? Name { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        [MaxLength(10)]
        public string? GenderTarget { get; set; }

        public bool IsSpecialOffer { get; set; }
        public decimal SpecialOfferPrice { get; set; }

        public virtual ICollection<ProgramUsage>? FitnessServiceUsages { get; set; }
    }
}