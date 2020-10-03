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

        public string Trigger => "pa"; // stands for "project admin"

        public string Name => "Set Current Project/TODOs";

        public string Description => "Set the current project and todo items";

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

            var commandRegex = new Regex(@"^\!pa\s(\w*)\s(\w*)(?:\s([\w\s]+))?$");
            var cmdParts = commandRegex.Match(chatMessage.Message);

            var commandType = cmdParts.Groups[1].Value; // Group 0 is the full match - start at index 1
            var subCommand = cmdParts.Groups[2].Value;
            var commandData = cmdParts.Groups[3]?.Value;

            _logger.LogInformation($"Project command received: type = '{commandType}' - command = '{subCommand}'");

            if (commandType == "project" && subCommand == "set")
            {
                _projectService.SetProject(commandData);
            }
            else if (commandType == "todo")
            {
                switch(subCommand)
                {
                    case "add":
                        _projectService.AddTodo(commandData);
                        break;
                    case "complete":
                        if (!int.TryParse(commandData, out var completeId))
                        {
                            return;
                        }

                        _projectService.CompleteTodo(completeId, true);
                        break;
                    case "resume":
                        if (!int.TryParse(commandData, out var uncompleteId))
                        {
                            return;
                        }

                        _projectService.CompleteTodo(uncompleteId, false);
                        break;
                    case "remove":
                        if (!int.TryParse(commandData, out var removeId))
                        {
                            return;
                        }

                        _projectService.RemoveTodo(removeId);
                        break;
                    case "clear":
                        _projectService.ClearTodos();
                        break;
                }

                var items = _projectService.GetTodoItems();
            }
        }
    }
}
