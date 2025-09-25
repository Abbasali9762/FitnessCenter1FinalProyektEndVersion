using FitnessCenter1.Entities;

namespace FitnessCenter1.Services.Abstract
{
    public interface INotificationService
    {
        Task<Notification> CreateNotification(int userId, string title, string message, string type);
        Task<bool> MarkAsRead(int notificationId);
        Task<bool> DeleteNotification(int notificationId);
        Task<Notification> GetNotificationById(int notificationId);
        Task<List<Notification>> GetUserNotifications(int userId);
        Task<List<Notification>> GetUnreadNotifications(int userId);
        Task<int> GetUnreadNotificationCount(int userId);
    }
}