using Microsoft.EntityFrameworkCore;
using FitnessCenter1.Context;
using FitnessCenter1.Entities;
using FitnessCenter1.Services.Abstract;

namespace FitnessCenter1.Services
{
    public class EquipmentUsageService : BaseService, IEquipmentUsageService
    {
        public EquipmentUsageService(FitnessCenterDbContext context) : base(context)
        {
        }

        public async Task<EquipmentUsage> StartUsage(int userId, int equipmentId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) throw new Exception("User not found");

            var equipment = await _context.Equipment.FindAsync(equipmentId);
            if (equipment == null) throw new Exception("Equipment not found");
            if (!equipment.IsAvailable) throw new Exception("Equipment not available");

            var usage = new EquipmentUsage
            {
                UserId = userId,
                EquipmentId = equipmentId,
                StartTime = DateTime.Now
            };

            equipment.AvailableQuantity--;

            _context.EquipmentUsages.Add(usage);
            await _context.SaveChangesAsync();

            return usage;
        }

        public async Task<EquipmentUsage> EndUsage(int usageId)
        {
            var usage = await _context.EquipmentUsages
                .Include(eu => eu.Equipment)
                .FirstOrDefaultAsync(eu => eu.EquipmentUsageId == usageId);

            if (usage == null) throw new Exception("Usage record not found");

            usage.EndTime = DateTime.Now;
            usage.Equipment.AvailableQuantity++;

            await _context.SaveChangesAsync();
            return usage;
        }

        public async Task<EquipmentUsage> GetUsageById(int usageId)
        {
            return await _context.EquipmentUsages
                .Include(eu => eu.User)
                .Include(eu => eu.Equipment)
                .FirstOrDefaultAsync(eu => eu.EquipmentUsageId == usageId);
        }

        public async Task<List<EquipmentUsage>> GetUserUsageHistory(int userId)
        {
            return await _context.EquipmentUsages
                .Where(eu => eu.UserId == userId)
                .Include(eu => eu.Equipment)
                .ToListAsync();
        }

        public async Task<List<EquipmentUsage>> GetAllUsageHistory()
        {
            return await _context.EquipmentUsages
                .Include(eu => eu.User)
                .Include(eu => eu.Equipment)
                .ToListAsync();
        }

        public async Task<List<EquipmentUsage>> GetCurrentUsages()
        {
            return await _context.EquipmentUsages
                .Where(eu => eu.EndTime == null)
                .Include(eu => eu.User)
                .Include(eu => eu.Equipment)
                .ToListAsync();
        }
    }
}