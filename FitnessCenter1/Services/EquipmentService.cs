using Microsoft.EntityFrameworkCore;
using FitnessCenter1.Context;
using FitnessCenter1.Entities;
using FitnessCenter1.Services.Abstract;

namespace FitnessCenter1.Services
{
    public class EquipmentService : BaseService, IEquipmentService
    {
        public EquipmentService(FitnessCenterDbContext context) : base(context)
        {
        }

        public async Task<Equipment> AddEquipment(string name, string description, int quantity, string category, DateTime purchaseDate)
        {
            var equipment = new Equipment
            {
                Name = name,
                Description = description,
                Quantity = quantity,
                AvailableQuantity = quantity,
                Category = category,
                PurchaseDate = purchaseDate,
                MaintenanceDate = purchaseDate.AddYears(1)
            };

            _context.Equipment.Add(equipment);
            await _context.SaveChangesAsync();

            return equipment;
        }

        public async Task<bool> UpdateEquipment(int equipmentId, int availableQuantity, DateTime? maintenanceDate)
        {
            var equipment = await _context.Equipment.FindAsync(equipmentId);
            if (equipment == null) return false;

            equipment.AvailableQuantity = availableQuantity;
            equipment.MaintenanceDate = maintenanceDate;

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> RemoveEquipment(int equipmentId)
        {
            var equipment = await _context.Equipment.FindAsync(equipmentId);
            if (equipment == null) return false;

            _context.Equipment.Remove(equipment);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<Equipment> GetEquipmentById(int equipmentId)
        {
            return await _context.Equipment.FindAsync(equipmentId);
        }

        public async Task<List<Equipment>> GetAllEquipment()
        {
            return await _context.Equipment.ToListAsync();
        }

        public async Task<List<Equipment>> GetEquipmentByCategory(string category)
        {
            return await _context.Equipment
                .Where(e => e.Category == category)
                .ToListAsync();
        }

        public async Task<List<Equipment>> GetEquipmentNeedingMaintenance()
        {
            return await _context.Equipment
                .Where(e => e.NeedsMaintenance)
                .ToListAsync();
        }
    }
}