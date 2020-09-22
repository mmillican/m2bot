using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using TwitchLib.Client.Interfaces;
using TwitchLib.Client.Models;

namespace M2DevBot.Web.Commands
{
    public class HelpCommand : IChatCommand
    {
        private readonly IServiceProvider _serviceProvider;

        public string Trigger => "help";
        public string Name => "Help";

        public string Description => "Get info about what commands are available.";

        public HelpCommand(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void Handle(ITwitchClient twitchClient, ChatMessage chatMessage)
        {
            var commands = _serviceProvider.GetServices<IChatCommand>()
                .Where(x => x.Trigger != "help")
                .OrderBy(x => x.Trigger);

            var commandDescriptions = new List<string>();

            foreach(var command in commands)
            {
                commandDescriptions.Add($"!{command.Trigger}: {command.Description}");
            }

            var response = string.Join(" // ", commandDescriptions);

            twitchClient.SendMessage(chatMessage.Channel, response);
        }
    }
}
