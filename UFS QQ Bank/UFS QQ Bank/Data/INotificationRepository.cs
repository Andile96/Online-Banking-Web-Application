
using UFS_QQ_Bank.Models;


namespace UFS_QQ_Bank.Data
{
    public interface INotificationRepository:IRepositoryBase<Notification>
    {
        Task AddNotification(string userId, string message);
        Task<List<Notification>> GetNotifications(string userId);
        Task MarkAsRead(string notificationId);
    }
}
