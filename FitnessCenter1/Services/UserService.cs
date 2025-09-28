using Microsoft.EntityFrameworkCore;
using FitnessCenter1.Context;
using FitnessCenter1.Entities;
using FitnessCenter1.Services.Abstract;

namespace FitnessCenter1.Services
{
    public class UserService : BaseService, IUserService
    {
        private readonly IEmailService _emailService;

        public UserService(FitnessCenterDbContext context, IEmailService emailService) : base(context)
        {
            _emailService = emailService;
        }

        public async Task<User> RegisterUser(string name, string surname, string username, string password, string email, string gender, bool isCar, decimal initialMoney)
        {
            if (await _context.Users.AnyAsync(u => u.Username == username))
                throw new Exception("Username already exists");

            if (await _context.Users.AnyAsync(u => u.Email == email))
                throw new Exception("Email already exists");

            if (gender != "Male" && gender != "Female")
                throw new Exception("Gender must be either Male or Female");

            string passwordHash = BCrypt.Net.BCrypt.HashPassword(password);
            string otp = GenerateOTP();

            var user = new User
            {
                Name = name,
                Surname = surname,
                Username = username,
                PasswordHash = passwordHash,
                Email = email,
                Gender = gender,
                IsCar = isCar,
                Money = initialMoney,
                HasEnteredGymToday = false,
                OTP = otp,
                OTPExpiry = DateTime.Now.AddMinutes(10)
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            await _emailService.SendRegistrationEmail(email, name);
            await _emailService.SendOTPEmail(email, otp);

            return user;
        }

        public async Task<User> LoginUser(string username, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
                throw new Exception("Invalid username or password");

            return user;
        }

        public async Task<bool> ForgotPassword(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null) return false;

            string otp = GenerateOTP();
            user.OTP = otp;
            user.OTPExpiry = DateTime.Now.AddMinutes(10);

            await _context.SaveChangesAsync();
            await _emailService.SendOTPEmail(email, otp);

            return true;
        }

        public async Task<bool> VerifyOTP(string email, string otp)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null || user.OTP != otp || user.OTPExpiry < DateTime.Now)
                return false;

            return true;
        }

        public async Task<bool> ResetPassword(string email, string newPassword)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null) return false;

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
            user.OTP = null;
            user.OTPExpiry = null;

            await _context.SaveChangesAsync();
            await _emailService.SendPasswordResetEmail(email);

            return true;
        }

        public async Task<User> GetUserById(int userId)
        {
            return await _context.Users.FindAsync(userId);
        }

        public async Task<List<User>> GetAllUsers()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<bool> UpdateUser(User user)
        {
            _context.Users.Update(user);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteUser(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return false;

            _context.Users.Remove(user);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> AddMoney(int userId, decimal amount)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return false;

            user.Money += amount;
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateUserProfile(int userId, string name, string surname, string email, string gender, bool isCar)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return false;

            user.Name = name;
            user.Surname = surname;
            user.Email = email;
            user.Gender = gender;
            user.IsCar = isCar;

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> ChangePassword(int userId, string currentPassword, string newPassword)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null || !BCrypt.Net.BCrypt.Verify(currentPassword, user.PasswordHash))
                return false;

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<User> GetUserByEmail(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User> GetUserByUsername(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<int> GetTotalUsersCount()
        {
            return await _context.Users.CountAsync();
        }

        public async Task<List<User>> GetUsersByGender(string gender)
        {
            return await _context.Users
                .Where(u => u.Gender == gender)
                .ToListAsync();
        }

        public async Task<bool> CheckUsernameExists(string username)
        {
            return await _context.Users.AnyAsync(u => u.Username == username);
        }

        public async Task<bool> CheckEmailExists(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email);
        }

        public async Task<decimal> GetUserBalance(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            return user?.Money ?? 0;
        }

        public async Task<bool> DeductMoney(int userId, decimal amount)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null || user.Money < amount) return false;

            user.Money -= amount;
            return await _context.SaveChangesAsync() > 0;
        }

        private string GenerateOTP()
        {
            Random random = new Random();
            return random.Next(100000, 999999).ToString();
        }
    }
}