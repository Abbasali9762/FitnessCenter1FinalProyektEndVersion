using FitnessCenter1.Entities;

namespace FitnessCenter1.Services.Abstract
{
    public interface IEquipmentService
    {
        Task<Equipment> AddEquipment(string name, string description, int quantity, string category, DateTime purchaseDate);
        Task<bool> UpdateEquipment(int equipmentId, int availableQuantity, DateTime? maintenanceDate);
        Task<bool> RemoveEquipment(int equipmentId);
        Task<Equipment> GetEquipmentById(int equipmentId);
        Task<List<Equipment>> GetAllEquipment();
        Task<List<Equipment>> GetEquipmentByCategory(string category);
        Task<List<Equipment>> GetEquipmentNeedingMaintenance();
    }
}