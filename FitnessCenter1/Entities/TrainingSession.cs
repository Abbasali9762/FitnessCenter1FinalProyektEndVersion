namespace FitnessCenter1.Entities
{
    public class TrainingSession
    {
        public int TrainingSessionId { get; set; }
        public int UserId { get; set; }
        public int TrainerId { get; set; }
        public DateTime SessionTime { get; set; }
        public int DurationMinutes { get; set; }
        public decimal TotalPrice { get; set; }

        public virtual User? User { get; set; }
        public virtual Trainer? Trainer { get; set; }
    }
}