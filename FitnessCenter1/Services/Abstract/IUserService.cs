using FitnessCenter1.Entities;

namespace FitnessCenter1.Services.Abstract
{
    public interface IUserService
    {
        Task<User> RegisterUser(string name, string surname, string username, string password, string email, string gender, bool isCar, decimal initialMoney);
        Task<User> LoginUser(string username, string password);
        Task<bool> ForgotPassword(string email);
        Task<bool> VerifyOTP(string email, string otp);
        Task<bool> ResetPassword(string email, string newPassword);
        Task<User> GetUserById(int userId);
        Task<List<User>> GetAllUsers();
        Task<bool> UpdateUser(User user);
        Task<bool> DeleteUser(int userId);
        Task<bool> AddMoney(int userId, decimal amount);
    }
}