
using System.Text.RegularExpressions;
using M2DevBot.Web.Services;
using Microsoft.Extensions.Logging;
using TwitchLib.Client.Interfaces;
using TwitchLib.Client.Models;

namespace M2DevBot.Web.Commands
{
    public class TodoCommand : IChatCommand
    {
        private readonly IProjectService _projectService;
        private readonly ILogger<TodoCommand> _logger;

        public string Trigger => "todo";

        public string Name => "Manage Todos";

        public string Description => "Manage the stream todo list";
        public bool ShowInHelp => false;

        public TodoCommand(IProjectService projectService,
            ILogger<TodoCommand> logger)
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

            var commandRegex = new Regex(@"^\!todo\s(\w*)(?:\s([\w\s]+))?$");
            var cmdParts = commandRegex.Match(chatMessage.Message);

            var command = cmdParts.Groups[1].Value; // Group 0 is the full match - start at index 1
            var commandData = cmdParts.Groups[2]?.Value;

            _logger.LogInformation($"Todo command received: '{command}'");

            switch(command)
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
        }
    }
}
