using M2DevBot.Web.Services;
using TwitchLib.Client.Interfaces;
using TwitchLib.Client.Models;

namespace M2DevBot.Web.Commands
{
    public class ProjectCommand : IChatCommand
    {
        private readonly IProjectService _projectService;

        public string Trigger => "project";
        public string Name => "Project Info";
        public string Description => "Get information about M2's current project";

        public ProjectCommand(IProjectService projectService)
        {
            _projectService = projectService;
        }

        public void Handle(ITwitchClient twitchClient, ChatMessage chatMessage)
        {
            var project = _projectService.GetProjectName();

            twitchClient.SendMessage(chatMessage.Channel, $"M2 is currently working on {project}");
        }
    }
}
