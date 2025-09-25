namespace FitnessCenter1.Entities
{
    public class ParkingArea
    {
        public int ParkingAreaId { get; set; }
        public string? Name { get; set; }
        public int TotalSpots { get; set; }
        public int AvailableSpots { get; set; }
        public bool IsEmpty => AvailableSpots > 0;

        public virtual ICollection<ParkingReservation>? ParkingReservations { get; set; }
    }
}