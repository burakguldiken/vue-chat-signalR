using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace signalrApi
{
    public class ChatHub : Hub
    {
        public override Task OnConnectedAsync()
        {
            System.Console.WriteLine("New Connection Started: " + Context.ConnectionId);
            Clients.All.SendAsync("NewConnection", "New Connection Successfull", Context.ConnectionId);
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(System.Exception exception)
        {
            System.Console.WriteLine("Discconnected: " + Context.ConnectionId);
            Clients.All.SendAsync("DisconnectConnection", "Connection Destoryed", Context.ConnectionId);
            return base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessage(string message)
        {
            Console.WriteLine("Message received");
            await Clients.All.SendAsync("SendMessage", message);
        }

        public async Task JoinRoom(string name)
        {
            Console.WriteLine("User Joined");
            await Clients.All.SendAsync("JoinRoom", name);
        }
    }
}
