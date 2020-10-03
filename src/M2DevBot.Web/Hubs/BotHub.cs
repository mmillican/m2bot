using System.Collections.Generic;
using System.Threading.Tasks;
using M2DevBot.Web.Models;
using Microsoft.AspNetCore.SignalR;

namespace M2DevBot.Web.Hubs
{
    public interface IBotClient
    {
        Task ProjectUpdated(string name);

        Task TodoItemAdded(TodoItem item);
        Task TodoItemRemoved(TodoItem item);
        Task TodoItemStatusChanged(TodoItem item);
        Task TodosCleared();
    }

    public class BotHub : Hub<IBotClient>
    {

    }
}
