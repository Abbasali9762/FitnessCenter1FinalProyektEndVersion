using FitnessCenter1.Entities;

namespace FitnessCenter1.Services.Abstract
{
    public interface IEquipmentUsageService
    {
        Task<EquipmentUsage> StartUsage(int userId, int equipmentId);
        Task<EquipmentUsage> EndUsage(int usageId);
        Task<EquipmentUsage> GetUsageById(int usageId);
        Task<List<EquipmentUsage>> GetUserUsageHistory(int userId);
        Task<List<EquipmentUsage>> GetAllUsageHistory();
        Task<List<EquipmentUsage>> GetCurrentUsages();
    }
}