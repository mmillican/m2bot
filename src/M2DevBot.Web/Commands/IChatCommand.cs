using TwitchLib.Client.Interfaces;
using TwitchLib.Client.Models;

namespace M2DevBot.Web.Commands
{
    public interface IChatCommand
    {
        string Trigger { get; }

        string Name { get; }
        string Description { get; }

        void Handle(ITwitchClient twitchClient, ChatMessage chatMessage);
    }
}
