using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volunteers.Data;
using Volunteers.Models.Notifications;
using Volunteers.Services.Users;

namespace Volunteers.Services.Notifications

{
    public class NotificationsService : INotificationsService
    {
        private readonly VolunteersDbContext data;
        private readonly IUserService userService;

        public NotificationsService(VolunteersDbContext data, IUserService userService)
        {
            this.data = data;
            this.userService = userService;
        }

        public async Task<int> GetNotificationsCount(string userId)
        {

            var user = await userService.FindUserById(userId);
            var notifications = await this.data.Notifications.Include(u => u.Users).Where(n => !n.Users.Contains(user) && n.PublishedOn >= user.RegistrationDate).CountAsync();

            return notifications;
        }

        public async Task<IEnumerable<ProjectNotificationViewModel>> ListNotifications(string userName)
        {

            var user = await userService.FindUserByUsername(userName);
            var notifications = this.data.Notifications.Include(u => u.Users).Where(n => !n.Users.Contains(user) && n.PublishedOn >= user.RegistrationDate).Select(n => new ProjectNotificationViewModel
            {
                Id = n.Id,
                ProjectId = n.ProjectId,
                Title = n.Title,
                PublishedOn = n.PublishedOn,
            }).ToList();

            return notifications;
        }
    }
}