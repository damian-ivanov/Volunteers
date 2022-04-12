using System.Collections.Generic;
using System.Threading.Tasks;
using Volunteers.Models.Notifications;

namespace Volunteers.Services.Notifications

{
    public interface INotificationsService
    {
        public Task<int> GetNotificationsCount(string userId);

        public Task<IEnumerable<ProjectNotificationViewModel>> ListNotifications(string userName);

    }
}