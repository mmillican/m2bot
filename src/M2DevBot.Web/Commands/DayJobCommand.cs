using TwitchLib.Client.Interfaces;
using TwitchLib.Client.Models;

namespace M2DevBot.Web.Commands
{
    public class DayJobCommand : IChatCommand
    {
        public string Trigger => "job";
        public string Name => "Day job info";
        public string Description => "Find out what Matt does for work";

        public void Handle(ITwitchClient twitchClient, ChatMessage chatMessage)
        {
            twitchClient.SendMessage(chatMessage.Channel,
                @"Matt is currently a Lead Software Engineer at a large marketing
                firm, leading their internal app team.");
        }
    }
}
