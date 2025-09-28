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
        Task<bool> UpdateUserProfile(int userId, string name, string surname, string email, string gender, bool isCar);
        Task<bool> ChangePassword(int userId, string currentPassword, string newPassword);
        Task<User> GetUserByEmail(string email);
        Task<User> GetUserByUsername(string username);
        Task<int> GetTotalUsersCount();
        Task<List<User>> GetUsersByGender(string gender);
        Task<bool> CheckUsernameExists(string username);
        Task<bool> CheckEmailExists(string email);
        Task<decimal> GetUserBalance(int userId);
        Task<bool> DeductMoney(int userId, decimal amount);
    }
}