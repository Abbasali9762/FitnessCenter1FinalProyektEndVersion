using FitnessCenter1.Entities;

namespace FitnessCenter1.Services.Abstract
{
    public interface IParkingService
    {
        Task<ParkingReservation> ReserveParkingSpot(int userId, int parkingAreaId, string spotLocation);
        Task<bool> CancelParkingReservation(int reservationId);
        Task<List<ParkingArea>> GetAvailableParkingAreas();
        Task<ParkingReservation> GetReservationById(int reservationId);
        Task<List<ParkingReservation>> GetAllReservations();
    }
}