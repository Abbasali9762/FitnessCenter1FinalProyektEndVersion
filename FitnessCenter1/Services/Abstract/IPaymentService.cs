using FitnessCenter1.Entities;

namespace FitnessCenter1.Services.Abstract
{
    public interface IPaymentService
    {
        Task<Payment> CreatePayment(int userId, string paymentType, decimal amount, string description);
        Task<bool> ProcessPayment(int paymentId);
        Task<bool> CancelPayment(int paymentId);
        Task<Payment> GetPaymentById(int paymentId);
        Task<List<Payment>> GetUserPayments(int userId);
        Task<List<Payment>> GetAllPayments();
        Task<decimal> GetTotalRevenue(DateTime startDate, DateTime endDate);
    }
}