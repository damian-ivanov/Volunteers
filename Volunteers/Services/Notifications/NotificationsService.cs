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

        public async Task<int> GetNotifications(string userId)
        {

            var user = await userService.FindUserById(userId);
            var notifications = await this.data.Notifications.Include(u => u.Users).Where(n => !n.Users.Contains(user)).CountAsync();

            return notifications;
        }
    }
}