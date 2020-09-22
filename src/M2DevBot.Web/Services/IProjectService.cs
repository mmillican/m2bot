using System.IO;
using System.Linq;
using System.Text.Json;
using M2DevBot.Web.Models;
using Microsoft.Extensions.Logging;

namespace M2DevBot.Web.Services
{
    // TODO: Cache the stuffs

    public interface IProjectService
    {
        string GetProjectName();

        void SetProject(string name);

        void AddTodo(string name);
        void CompleteTodo(int number);
        void RemoveTodo(int number);
    }

    public class ProjectService : IProjectService
    {
        private const string FILE_NAME = "C:\\_Dev\\projects.json";

        private readonly ILogger<ProjectService> _logger;

        public ProjectService(ILogger<ProjectService> logger)
        {
            _logger = logger;
        }

        public string GetProjectName()
        {
            var project = GetProject();
            return project?.Name ?? "Working on ASP.NET Core + Vue.js Things";
        }

        public void SetProject(string name)
        {
            _logger.LogInformation($"Set project = {name}");

            var project = GetProject();
            project.Name = name;

            Save(project);
        }

        public void AddTodo(string name)
        {
            _logger.LogInformation($"Add todo {name}");

            var project = GetProject();
            project.TodoItems.Add(new TodoItem { Name = name });

            Save(project);
        }

        public void CompleteTodo(int number)
        {
            var project = GetProject();

            var itemIndex = number - 1;

            if (itemIndex < 0 || project.TodoItems.Count < number)
            {
                return;
            }

            project.TodoItems[itemIndex].IsComplete = true;

            Save(project);
        }

        public void RemoveTodo(int number)
        {
            var project = GetProject();

            var itemIndex = number - 1;

            if (itemIndex < 0 || project.TodoItems.Count < number)
            {
                return;
            }

            project.TodoItems.RemoveAt(itemIndex);

            Save(project);
        }

        public void Save(Project project)
        {
            // TODO: Probably should use a using and stream

            var testdata = JsonSerializer.Serialize(project);
            _logger.LogInformation($"Saving project data: {testdata}");

            var data = JsonSerializer.SerializeToUtf8Bytes(project);
            File.WriteAllBytes(FILE_NAME, data);
        }

        private Project GetProject()
        {
            // TODO: Probably should use a using and stream
            // TODO: Probably should lock this to make sure we don't have any overwrites

            if (!File.Exists(FILE_NAME))
            {
                _logger.LogInformation($"File {FILE_NAME} does not exist; returning empty project");

                return new Project();
            }

            var fileContents = File.ReadAllText(FILE_NAME);
            _logger.LogInformation($"File contents: {fileContents}");

            var data = JsonSerializer.Deserialize<Project>(fileContents) ?? new Project();

            return data;
        }
    }

}
