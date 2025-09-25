namespace FitnessCenter1.Entities
{
    public class ParkingReservation
    {
        public int ParkingReservationId { get; set; }
        public int UserId { get; set; }
        public int ParkingAreaId { get; set; }
        public DateTime ReservationTime { get; set; }
        public string? SpotLocation { get; set; }

        public virtual User? User { get; set; }
        public virtual ParkingArea? ParkingArea { get; set; }
    }
}