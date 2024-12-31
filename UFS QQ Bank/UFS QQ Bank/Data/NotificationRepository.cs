using DocumentFormat.OpenXml.Math;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

using UFS_QQ_Bank.Models;

namespace UFS_QQ_Bank.Data.DataAccess
{
    public class NotificationRepository : RepositoryBase<Notification>, INotificationRepository
    {
        

        public NotificationRepository(AppEntityDbContext db) : base(db)
        {
            
        }

        public async Task AddNotification(string userId, string message)
        {
            var notification = new Notification
            {
                UserID = userId,
                Message = message,
                Created_At = DateTime.Now,
                IsRead = false
            };
            _db.Notifications.Add(notification);
            await _db.SaveChangesAsync();
        }
       
        public async Task<List<Notification>> GetNotifications(string userId)
        {

            return await _db.Notifications
                   .Where(n => n.UserID == userId && !n.IsRead)
                   .OrderByDescending(n => n.Created_At)
                   .Take(2)
                   .ToListAsync();

        }

        public async Task MarkAsRead(string notificationId)
        {
            var notifications = await _db.Notifications
                                 .Where(n => n.UserID == notificationId && !n.IsRead)
                                 .ToListAsync();
            if (notifications != null && notifications.Any())
            {
                foreach (var notification in notifications)
                {
                    notification.IsRead = true;
                }
                await _db.SaveChangesAsync();
            }

        }
    }
}
