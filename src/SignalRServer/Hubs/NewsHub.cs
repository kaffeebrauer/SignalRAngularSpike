using System;
using Microsoft.AspNetCore.SignalR;
using SignalRServer.DTO;
using System.Threading.Tasks;
using SignalRServer.Providers;

namespace SignalRServer.Hubs
{
    public class NewsHub : Hub
    {
        private readonly NewsStore _newsStore;

        public NewsHub(NewsStore newsStore)
        {
            _newsStore = newsStore;
        }

        public Task Send(NewsItem newsItem)
        {
            if (!_newsStore.GroupExists(newsItem.Equity))
            {
                throw new Exception("cannot send a news item to a group which does not exist.");
            }

            _newsStore.CreateNewItem(newsItem);
            return Clients.Group(newsItem.Equity).SendAsync("Send", newsItem);
        }

        public async Task JoinGroup(string equity)
        {
            if (!_newsStore.GroupExists(equity))
            {
                throw new Exception("cannot join a group which does not exist.");
            }

            await Groups.AddToGroupAsync(Context.ConnectionId, equity);
            await Clients.Group(equity).SendAsync("JoinGroup", equity);

            var history = _newsStore.GetAllNewsItems(equity);
            await Clients.Client(Context.ConnectionId).SendAsync("History", history);
        }

        public async Task LeaveGroup(string groupName)
        {
            if (!_newsStore.GroupExists(groupName))
            {
                throw new Exception("cannot leave a group which does not exist.");
            }

            await Clients.Group(groupName).SendAsync("LeaveGroup", groupName);
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        }
    }
}
