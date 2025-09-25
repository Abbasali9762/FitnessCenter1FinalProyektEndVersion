using Microsoft.EntityFrameworkCore;
using FitnessCenter1.Context;
using FitnessCenter1.Entities;
using FitnessCenter1.Services.Abstract;

namespace FitnessCenter1.Services
{
    public class ParkingService : BaseService, IParkingService
    {
        public ParkingService(FitnessCenterDbContext context) : base(context)
        {
        }

        public async Task<ParkingReservation> ReserveParkingSpot(int userId, int parkingAreaId, string spotLocation)
        {
            var parkingArea = await _context.ParkingAreas.FindAsync(parkingAreaId);
            if (parkingArea == null)
                throw new Exception("Parking area not found");

            if (!parkingArea.IsEmpty)
                throw new Exception("Parking area is full");

            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                throw new Exception("User not found");

            if (!user.IsCar)
                throw new Exception("User doesn't have a car");

            var reservation = new ParkingReservation
            {
                UserId = userId,
                ParkingAreaId = parkingAreaId,
                ReservationTime = DateTime.Now,
                SpotLocation = spotLocation
            };

            parkingArea.AvailableSpots--;

            _context.ParkingReservations.Add(reservation);
            await _context.SaveChangesAsync();

            return reservation;
        }

        public async Task<bool> CancelParkingReservation(int reservationId)
        {
            var reservation = await _context.ParkingReservations
                .Include(pr => pr.ParkingArea)
                .FirstOrDefaultAsync(pr => pr.ParkingReservationId == reservationId);

            if (reservation == null) return false;

            reservation.ParkingArea.AvailableSpots++;
            _context.ParkingReservations.Remove(reservation);

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<List<ParkingArea>> GetAvailableParkingAreas()
        {
            return await _context.ParkingAreas
                .Where(pa => pa.IsEmpty)
                .ToListAsync();
        }

        public async Task<ParkingReservation> GetReservationById(int reservationId)
        {
            return await _context.ParkingReservations
                .Include(pr => pr.User)
                .Include(pr => pr.ParkingArea)
                .FirstOrDefaultAsync(pr => pr.ParkingReservationId == reservationId);
        }

        public async Task<List<ParkingReservation>> GetAllReservations()
        {
            return await _context.ParkingReservations
                .Include(pr => pr.User)
                .Include(pr => pr.ParkingArea)
                .ToListAsync();
        }
    }
}