using System.ComponentModel.DataAnnotations;

namespace FitnessCenter1.Entities
{
    public class Membership
    {
        public int MembershipId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        [MaxLength(50)]
        public string? Type { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        [Required]
        public decimal Price { get; set; }

        public bool IsActive => EndDate >= DateTime.Now;

        public virtual User? User { get; set; }
    }
}