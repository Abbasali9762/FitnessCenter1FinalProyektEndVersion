using FitnessCenter1.Entities;

namespace FitnessCenter1.Services.Abstract
{
    public interface ITrainerService
    {
        Task<TrainingSession> BookTrainingSession(int userId, int trainerId, DateTime sessionTime, int durationMinutes);
        Task<bool> CancelTrainingSession(int sessionId);
        Task<List<Trainer>> GetAvailableTrainers();
        Task<TrainingSession> GetSessionById(int sessionId);
        Task<List<TrainingSession>> GetAllSessions();
    }
}