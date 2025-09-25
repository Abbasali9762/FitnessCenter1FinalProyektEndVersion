using FitnessCenter1.Entities;

namespace FitnessCenter1.Services.Abstract
{
    public interface IMembershipService
    {
        Task<Membership> CreateMembership(int userId, string type, DateTime startDate, DateTime endDate, decimal price);
        Task<bool> CancelMembership(int membershipId);
        Task<Membership> GetMembershipById(int membershipId);
        Task<List<Membership>> GetUserMemberships(int userId);
        Task<List<Membership>> GetAllMemberships();
        Task<bool> CheckMembershipStatus(int userId);
        Task<bool> ExtendMembership(int membershipId, int additionalDays);
    }
}