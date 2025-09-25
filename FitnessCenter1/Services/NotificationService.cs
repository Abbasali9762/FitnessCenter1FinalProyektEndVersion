using Microsoft.EntityFrameworkCore;
using FitnessCenter1.Context;
using FitnessCenter1.Entities;
using FitnessCenter1.Services.Abstract;

namespace FitnessCenter1.Services
{
    public class NotificationService : BaseService, INotificationService
    {
        public NotificationService(FitnessCenterDbContext context) : base(context)
        {
        }

        public async Task<Notification> CreateNotification(int userId, string title, string message, string type)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) throw new Exception("User not found");

            var notification = new Notification
            {
                UserId = userId,
                Title = title,
                Message = message,
                Type = type,
                CreatedDate = DateTime.Now,
                IsRead = false
            };

            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();

            return notification;
        }

        public async Task<bool> MarkAsRead(int notificationId)
        {
            var notification = await _context.Notifications.FindAsync(notificationId);
            if (notification == null) return false;

            notification.IsRead = true;
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteNotification(int notificationId)
        {
            var notification = await _context.Notifications.FindAsync(notificationId);
            if (notification == null) return false;

            _context.Notifications.Remove(notification);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<Notification> GetNotificationById(int notificationId)
        {
            return await _context.Notifications
                .Include(n => n.User)
                .FirstOrDefaultAsync(n => n.NotificationId == notificationId);
        }

        public async Task<List<Notification>> GetUserNotifications(int userId)
        {
            return await _context.Notifications
                .Where(n => n.UserId == userId)
                .Include(n => n.User)
                .OrderByDescending(n => n.CreatedDate)
                .ToListAsync();
        }

        public async Task<List<Notification>> GetUnreadNotifications(int userId)
        {
            return await _context.Notifications
                .Where(n => n.UserId == userId && !n.IsRead)
                .Include(n => n.User)
                .OrderByDescending(n => n.CreatedDate)
                .ToListAsync();
        }

        public async Task<int> GetUnreadNotificationCount(int userId)
        {
            return await _context.Notifications
                .CountAsync(n => n.UserId == userId && !n.IsRead);
        }
    }
}