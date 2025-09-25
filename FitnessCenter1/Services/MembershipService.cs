using Microsoft.EntityFrameworkCore;
using FitnessCenter1.Context;
using FitnessCenter1.Entities;
using FitnessCenter1.Services.Abstract;

namespace FitnessCenter1.Services
{
    public class MembershipService : BaseService, IMembershipService
    {
        private readonly IEmailService _emailService;

        public MembershipService(FitnessCenterDbContext context, IEmailService emailService) : base(context)
        {
            _emailService = emailService;
        }

        public MembershipService(FitnessCenterDbContext context) : base(context)
        {
        }

        public async Task<Membership> CreateMembership(int userId, string type, DateTime startDate, DateTime endDate, decimal price)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) throw new Exception("User not found");

            if (user.Money < price)
                throw new Exception("Insufficient funds for membership");

            var membership = new Membership
            {
                UserId = userId,
                Type = type,
                StartDate = startDate,
                EndDate = endDate,
                Price = price
            };

            user.Money -= price;

            _context.Memberships.Add(membership);
            await _context.SaveChangesAsync();

            string membershipDetails = $"Type: {type}\n" +
                                     $"Start Date: {startDate:yyyy-MM-dd}\n" +
                                     $"End Date: {endDate:yyyy-MM-dd}\n" +
                                     $"Price: {price:C}";
            if (string.IsNullOrEmpty(user.Email))
                throw new Exception("User email is not available.");


            await _emailService.SendMembershipConfirmationEmail(user.Email, membershipDetails);

            return membership;
        }

        public async Task<bool> CancelMembership(int membershipId)
        {
            var membership = await _context.Memberships.FindAsync(membershipId);
            if (membership == null) return false;

            var daysUsed = (DateTime.Now - membership.StartDate).Days;
            var totalDays = (membership.EndDate - membership.StartDate).Days;
            var refundAmount = daysUsed < totalDays ?
                membership.Price * (totalDays - daysUsed) / totalDays : 0;

            if (refundAmount > 0)
            {
                var user = await _context.Users.FindAsync(membership.UserId);
                user.Money += refundAmount;
            }

            _context.Memberships.Remove(membership);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<Membership> GetMembershipById(int membershipId)
        {
            return await _context.Memberships
                .Include(m => m.User)
                .FirstOrDefaultAsync(m => m.MembershipId == membershipId);
        }

        public async Task<List<Membership>> GetUserMemberships(int userId)
        {
            return await _context.Memberships
                .Where(m => m.UserId == userId)
                .Include(m => m.User)
                .ToListAsync();
        }

        public async Task<List<Membership>> GetAllMemberships()
        {
            return await _context.Memberships
                .Include(m => m.User)
                .ToListAsync();
        }

        public async Task<bool> CheckMembershipStatus(int userId)
        {
            var activeMembership = await _context.Memberships
                .Where(m => m.UserId == userId && m.EndDate >= DateTime.Now)
                .FirstOrDefaultAsync();

            return activeMembership != null;
        }

        public async Task<bool> ExtendMembership(int membershipId, int additionalDays)
        {
            var membership = await _context.Memberships.FindAsync(membershipId);
            if (membership == null) return false;

            var extensionPrice = additionalDays * (membership.Price /
                (membership.EndDate - membership.StartDate).Days);

            var user = await _context.Users.FindAsync(membership.UserId);
            if (user.Money < extensionPrice)
                throw new Exception("Insufficient funds for extension");

            user.Money -= extensionPrice;
            membership.EndDate = membership.EndDate.AddDays(additionalDays);
            membership.Price += extensionPrice;

            return await _context.SaveChangesAsync() > 0;
        }
    }
}