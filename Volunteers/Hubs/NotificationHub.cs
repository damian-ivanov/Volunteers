using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volunteers.Models;

namespace Volunteers.Hubs
{
    public class NotificationHub : Hub
    {   

        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", "test user", "test message");
        }

    }
}