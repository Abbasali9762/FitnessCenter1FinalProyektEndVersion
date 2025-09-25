using Microsoft.EntityFrameworkCore;
using FitnessCenter1.Context;
using FitnessCenter1.Entities;
using FitnessCenter1.Services.Abstract;

namespace FitnessCenter1.Services
{
    public class TrainerService : BaseService, ITrainerService
    {
        public TrainerService(FitnessCenterDbContext context) : base(context)
        {
        }

        public async Task<TrainingSession> BookTrainingSession(int userId, int trainerId, DateTime sessionTime, int durationMinutes)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) throw new Exception("User not found");

            var trainer = await _context.Trainers.FindAsync(trainerId);
            if (trainer == null) throw new Exception("Trainer not found");
            if (!trainer.IsAvailable) throw new Exception("Trainer is not available");

            decimal totalPrice = trainer.HourlyRate * (durationMinutes / 60m);

            if (user.Money < totalPrice)
                throw new Exception("Insufficient funds");

            var session = new TrainingSession
            {
                UserId = userId,
                TrainerId = trainerId,
                SessionTime = sessionTime,
                DurationMinutes = durationMinutes,
                TotalPrice = totalPrice
            };

            user.Money -= totalPrice;

            _context.TrainingSessions.Add(session);
            await _context.SaveChangesAsync();

            return session;
        }

        public async Task<bool> CancelTrainingSession(int sessionId)
        {
            var session = await _context.TrainingSessions
                .Include(ts => ts.User)
                .FirstOrDefaultAsync(ts => ts.TrainingSessionId == sessionId);

            if (session == null) return false;

            session.User.Money += session.TotalPrice;
            _context.TrainingSessions.Remove(session);

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<List<Trainer>> GetAvailableTrainers()
        {
            return await _context.Trainers
                .Where(t => t.IsAvailable)
                .ToListAsync();
        }

        public async Task<TrainingSession> GetSessionById(int sessionId)
        {
            return await _context.TrainingSessions
                .Include(ts => ts.User)
                .Include(ts => ts.Trainer)
                .FirstOrDefaultAsync(ts => ts.TrainingSessionId == sessionId);
        }

        public async Task<List<TrainingSession>> GetAllSessions()
        {
            return await _context.TrainingSessions
                .Include(ts => ts.User)
                .Include(ts => ts.Trainer)
                .ToListAsync();
        }
    }
}