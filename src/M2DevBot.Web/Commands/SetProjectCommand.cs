using System.Text.RegularExpressions;
using M2DevBot.Web.Hubs;
using M2DevBot.Web.Services;
using Microsoft.Extensions.Logging;
using TwitchLib.Client.Interfaces;
using TwitchLib.Client.Models;
using Microsoft.AspNetCore.SignalR;

namespace M2DevBot.Web.Commands
{
    public class SetProjectCommand : IChatCommand
    {
        private readonly IProjectService _projectService;
        private readonly ILogger<SetProjectCommand> _logger;

        public string Trigger => "set-project";

        public string Name => "Set Current Project";

        public string Description => "Set the current project";
        public bool ShowInHelp => false;

        public SetProjectCommand(IProjectService projectService,
            ILogger<SetProjectCommand> logger)
        {
            _projectService = projectService;
            _logger = logger;
        }

        public void Handle(ITwitchClient twitchClient, ChatMessage chatMessage)
        {
            if (!chatMessage.IsBroadcaster && !chatMessage.IsModerator)
            {
                return;
            }

            var commandRegex = new Regex(@"^\!set-project\s([\w\s.#+\/&]+)$");
            var cmdParts = commandRegex.Match(chatMessage.Message);

            var commandData = cmdParts.Groups[1]?.Value; // Group 0 is the full match - start at index 1

            _logger.LogInformation($"Set project command received - Project = {commandData}");

            _projectService.SetProject(commandData);
        }
    }
}
