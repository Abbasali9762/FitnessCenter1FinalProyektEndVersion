namespace FitnessCenter1.Entities
{
    public class Trainer
    {
        public int TrainerId { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Specialization { get; set; }
        public decimal HourlyRate { get; set; }
        public bool IsAvailable { get; set; }

        public virtual ICollection<TrainingSession>? TrainingSessions { get; set; }
    }
}