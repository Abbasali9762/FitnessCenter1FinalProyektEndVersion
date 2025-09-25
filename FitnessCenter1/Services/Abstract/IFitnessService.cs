using FitnessCenter1.Entities;
namespace FitnessCenter1.Services.Abstract
{
    public interface IFitnessService
    {
        Task<ProgramUsage> UseFitnessService(int userId, int programId);
        Task<List<FitnessProgram>> GetProgramsByGender(string gender);
        Task<List<FitnessProgram>> GetSpecialOffers();
        Task<ProgramUsage> GetUsageById(int usageId);
        Task<List<ProgramUsage>> GetAllUsages();
        Task<List<ProgramUsage>> GetUserUsageHistory(int userId);
        Task<bool> CancelServiceUsage(int usageId);
        Task<FitnessProgram> GetProgramById(int programId);
        Task<List<FitnessProgram>> GetAllPrograms();
        Task<FitnessProgram> CreateProgram(string name, string description, decimal price, string genderTarget, bool isSpecialOffer, decimal specialOfferPrice);
        Task<bool> UpdateProgram(int programId, string name, string description, decimal price, string genderTarget, bool isSpecialOffer, decimal specialOfferPrice);
        Task<bool> DeleteProgram(int programId);
        Task<decimal> GetTotalRevenueFromServices(DateTime startDate, DateTime endDate);
        Task<List<FitnessProgram>> GetMostPopularPrograms(int count);
    }
}