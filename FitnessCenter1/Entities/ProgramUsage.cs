using System.ComponentModel.DataAnnotations;
namespace FitnessCenter1.Entities
{
    public class ProgramUsage
    {
        public int ProgramUsageId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int FitnessProgramId { get; set; }

        public DateTime UsageTime { get; set; }
        public decimal PricePaid { get; set; }

        public virtual User? User { get; set; }
        public virtual FitnessProgram? FitnessProgram { get; set; }
    }
}