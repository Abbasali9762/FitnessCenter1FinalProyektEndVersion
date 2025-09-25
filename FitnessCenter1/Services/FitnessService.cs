using Microsoft.EntityFrameworkCore;
using FitnessCenter1.Context;
using FitnessCenter1.Entities;
using FitnessCenter1.Services.Abstract;

namespace FitnessCenter1.Services
{
    public class FitnessService : BaseService, IFitnessService
    {
        private readonly IEmailService _emailService;
        private readonly INotificationService _notificationService;

        public FitnessService(FitnessCenterDbContext context, IEmailService emailService, INotificationService notificationService) : base(context)
        {
            _emailService = emailService;
            _notificationService = notificationService;
        }

        public FitnessService(FitnessCenterDbContext context) : base(context)
        {
        }

        public async Task<ProgramUsage> UseFitnessService(int userId, int programId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                throw new Exception("User not found");

            var program = await _context.FitnessPrograms.FindAsync(programId);
            if (program == null)
                throw new Exception("Program not found");

            if (program.GenderTarget != "Both" && program.GenderTarget != user.Gender)
                throw new Exception($"This program is only for {program.GenderTarget} users");

            decimal price = program.IsSpecialOffer ?
                program.SpecialOfferPrice :
                program.Price;

            if (user.Money < price)
                throw new Exception("Not enough money");

            var usage = new ProgramUsage
            {
                UserId = userId,
                FitnessProgramId = programId,
                UsageTime = DateTime.Now,
                PricePaid = price
            };

            user.Money -= price;

            _context.ProgramUsages.Add(usage);
            await _context.SaveChangesAsync();

            await _notificationService.CreateNotification(
                userId,
                "Program Usage",
                $"You successfully used {program.Name} program. Amount paid: {price:C}",
                "Info"
            );

            return usage;
        }

        public async Task<List<FitnessProgram>> GetProgramsByGender(string gender)
        {
            return await _context.FitnessPrograms
                .Where(fp => fp.GenderTarget == gender || fp.GenderTarget == "Both")
                .OrderBy(fp => fp.Name)
                .ToListAsync();
        }

        public async Task<List<FitnessProgram>> GetSpecialOffers()
        {
            return await _context.FitnessPrograms
                .Where(fp => fp.IsSpecialOffer)
                .OrderBy(fp => fp.Name)
                .ToListAsync();
        }

        public async Task<ProgramUsage> GetUsageById(int usageId)
        {
            return await _context.ProgramUsages
                .Include(pu => pu.User)
                .Include(pu => pu.FitnessProgram)
                .FirstOrDefaultAsync(pu => pu.ProgramUsageId == usageId);
        }

        public async Task<List<ProgramUsage>> GetAllUsages()
        {
            return await _context.ProgramUsages
                .Include(pu => pu.User)
                .Include(pu => pu.FitnessProgram)
                .OrderByDescending(pu => pu.UsageTime)
                .ToListAsync();
        }

        public async Task<List<ProgramUsage>> GetUserUsageHistory(int userId)
        {
            return await _context.ProgramUsages
                .Where(pu => pu.UserId == userId)
                .Include(pu => pu.FitnessProgram)
                .OrderByDescending(pu => pu.UsageTime)
                .ToListAsync();
        }

        public async Task<bool> CancelServiceUsage(int usageId)
        {
            var usage = await _context.ProgramUsages
                .Include(pu => pu.User)
                .Include(pu => pu.FitnessProgram)
                .FirstOrDefaultAsync(pu => pu.ProgramUsageId == usageId);

            if (usage == null)
                return false;

            if (DateTime.Now - usage.UsageTime < TimeSpan.FromHours(1))
            {
                usage.User.Money += usage.PricePaid;

                await _notificationService.CreateNotification(
                    usage.UserId,
                    "Program Cancellation",
                    $"{usage.FitnessProgram.Name} program was cancelled. {usage.PricePaid:C} has been refunded to your account.",
                    "Info"
                );
            }

            _context.ProgramUsages.Remove(usage);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<FitnessProgram> GetProgramById(int programId)
        {
            return await _context.FitnessPrograms.FindAsync(programId);
        }

        public async Task<List<FitnessProgram>> GetAllPrograms()
        {
            return await _context.FitnessPrograms
                .OrderBy(fp => fp.GenderTarget)
                .ThenBy(fp => fp.Name)
                .ToListAsync();
        }

        public async Task<FitnessProgram> CreateProgram(string name, string description, decimal price, string genderTarget, bool isSpecialOffer, decimal specialOfferPrice)
        {
            if (genderTarget != "Male" && genderTarget != "Female" && genderTarget != "Both")
                throw new Exception("Gender target can only be 'Male', 'Female' or 'Both'");

            var program = new FitnessProgram
            {
                Name = name,
                Description = description,
                Price = price,
                GenderTarget = genderTarget,
                IsSpecialOffer = isSpecialOffer,
                SpecialOfferPrice = isSpecialOffer ? specialOfferPrice : price
            };

            _context.FitnessPrograms.Add(program);
            await _context.SaveChangesAsync();

            return program;
        }

        public async Task<bool> UpdateProgram(int programId, string name, string description, decimal price, string genderTarget, bool isSpecialOffer, decimal specialOfferPrice)
        {
            if (genderTarget != "Male" && genderTarget != "Female" && genderTarget != "Both")
                throw new Exception("Gender target can only be 'Male', 'Female' or 'Both'");

            var program = await _context.FitnessPrograms.FindAsync(programId);
            if (program == null)
                return false;

            program.Name = name;
            program.Description = description;
            program.Price = price;
            program.GenderTarget = genderTarget;
            program.IsSpecialOffer = isSpecialOffer;
            program.SpecialOfferPrice = isSpecialOffer ? specialOfferPrice : price;

            _context.FitnessPrograms.Update(program);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteProgram(int programId)
        {
            var program = await _context.FitnessPrograms.FindAsync(programId);
            if (program == null)
                return false;

            var hasUsage = await _context.ProgramUsages
                .AnyAsync(pu => pu.FitnessProgramId == programId);

            if (hasUsage)
                throw new Exception("Cannot delete this program because it has been used");

            _context.FitnessPrograms.Remove(program);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<decimal> GetTotalRevenueFromServices(DateTime startDate, DateTime endDate)
        {
            return await _context.ProgramUsages
                .Where(pu => pu.UsageTime >= startDate && pu.UsageTime <= endDate)
                .SumAsync(pu => pu.PricePaid);
        }

        public async Task<List<FitnessProgram>> GetMostPopularPrograms(int count)
        {
            return await _context.ProgramUsages
                .GroupBy(pu => pu.FitnessProgramId)
                .Select(g => new
                {
                    ProgramId = g.Key,
                    UsageCount = g.Count()
                })
                .OrderByDescending(x => x.UsageCount)
                .Take(count)
                .Join(_context.FitnessPrograms,
                      usage => usage.ProgramId,
                      program => program.FitnessProgramId,
                      (usage, program) => program)
                .ToListAsync();
        }
    }
}