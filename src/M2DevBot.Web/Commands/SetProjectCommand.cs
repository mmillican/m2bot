using System.Text.RegularExpressions;
using M2DevBot.Web.Services;
using Microsoft.Extensions.Logging;
using TwitchLib.Client.Interfaces;

namespace M2DevBot.Web.Commands
{
    public class SetProjectCommand : IChatCommand
    {
        private const string MYSELF = "m2devdotnet";

        private readonly IProjectService _projectService;
        private readonly ILogger<SetProjectCommand> _logger;

        public string Trigger => "pa"; // stands for "project admin"

        public string Name => "Set Current Project/TODOs";

        public string Description => "Set the current project and todo items";

        public SetProjectCommand(IProjectService projectService,
            ILogger<SetProjectCommand> logger)
        {
            _projectService = projectService;
            _logger = logger;
        }

        public void Handle(ITwitchClient twitchClient, string channel, string message, string userName)
        {
            // TODO: only let host and moderators set project/todos
            if (userName != MYSELF)
            {
                return;
            }

            var commandRegex = new Regex(@"^\!pa\s(\w*)\s(\w*)\s(.*)$");
            var cmdParts = commandRegex.Match(message);

            var commandType = cmdParts.Groups[1].Value; // Group 0 is the full match - start at index 1
            var subCommand = cmdParts.Groups[2].Value;
            var commandData = cmdParts.Groups[3].Value;

            _logger.LogInformation($"Project command received: type = '{commandType}' - command = '{subCommand}'");

            if (commandType == "project" && subCommand == "set")
            {
                _projectService.SetProject(commandData);

                twitchClient.SendWhisper(MYSELF, $"Project set to '{commandData}'");
            }
            else if (commandType == "todo")
            {
                switch(subCommand)
                {
                    case "add":
                        _projectService.AddTodo(commandData);

                        twitchClient.SendWhisper(MYSELF, $"Todo item added for '{commandData}'");
                        break;
                    case "complete":
                        if (!int.TryParse(commandData, out var completeId))
                        {
                            return;
                        }

                        _projectService.CompleteTodo(completeId);

                        twitchClient.SendWhisper(MYSELF, $"Todo #{completeId} marked as complete");
                        break;
                    case "remove":
                        if (!int.TryParse(commandData, out var removeId))
                        {
                            return;
                        }

                        _projectService.RemoveTodo(removeId);
                        twitchClient.SendWhisper(MYSELF, $"Todo #{removeId} removed");
                        break;
                }
            }
        }
    }
}
