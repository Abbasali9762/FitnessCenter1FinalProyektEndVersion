using Microsoft.EntityFrameworkCore;
using FitnessCenter1.Context;
using FitnessCenter1.Entities;
using FitnessCenter1.Services.Abstract;

namespace FitnessCenter1.Services
{
    public class PaymentService : BaseService, IPaymentService
    {
        private readonly IEmailService _emailService;

        public PaymentService(FitnessCenterDbContext context, IEmailService emailService) : base(context)
        {
            _emailService = emailService;
        }

        public async Task<Payment> CreatePayment(int userId, string paymentType, decimal amount, string description)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) throw new Exception("User not found");

            var payment = new Payment
            {
                UserId = userId,
                PaymentType = paymentType,
                Amount = amount,
                Description = description,
                PaymentDate = DateTime.Now,
                Status = "Pending"
            };

            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();

            return payment;
        }

        public async Task<bool> ProcessPayment(int paymentId)
        {
            var payment = await _context.Payments
                .Include(p => p.User)
                .FirstOrDefaultAsync(p => p.PaymentId == paymentId);

            if (payment == null) return false;

            var user = payment.User;
            if (user == null) return false;

            if (user.Money < payment.Amount)
            {
                payment.Status = "Failed";
                await _context.SaveChangesAsync();
                return false;
            }

            user.Money -= payment.Amount;
            payment.Status = "Completed";
            payment.PaymentDate = DateTime.Now;

            await _context.SaveChangesAsync();

            try
            {
                string paymentDetails = $"Amount: {payment.Amount:C}\n" +
                                      $"Type: {payment.PaymentType}\n" +
                                      $"Date: {payment.PaymentDate:yyyy-MM-dd HH:mm}";

                if (!string.IsNullOrEmpty(user.Email))
                {
                    await _emailService.SendPaymentConfirmationEmail(user.Email, paymentDetails);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Email sending failed: {ex.Message}");
            }

            return true;
        }

        public async Task<bool> CancelPayment(int paymentId)
        {
            var payment = await _context.Payments.FindAsync(paymentId);
            if (payment == null || payment.Status != "Pending") return false;

            payment.Status = "Failed";
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<Payment> GetPaymentById(int paymentId)
        {
            return await _context.Payments
                .Include(p => p.User)
                .FirstOrDefaultAsync(p => p.PaymentId == paymentId);
        }

        public async Task<List<Payment>> GetUserPayments(int userId)
        {
            return await _context.Payments
                .Where(p => p.UserId == userId)
                .Include(p => p.User)
                .OrderByDescending(p => p.PaymentDate)
                .ToListAsync();
        }

        public async Task<List<Payment>> GetAllPayments()
        {
            return await _context.Payments
                .Include(p => p.User)
                .OrderByDescending(p => p.PaymentDate)
                .ToListAsync();
        }

        public async Task<decimal> GetTotalRevenue(DateTime startDate, DateTime endDate)
        {
            return await _context.Payments
                .Where(p => p.Status == "Completed" &&
                           p.PaymentDate >= startDate &&
                           p.PaymentDate <= endDate)
                .SumAsync(p => p.Amount);
        }
    }
}