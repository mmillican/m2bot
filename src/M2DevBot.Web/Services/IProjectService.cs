using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using M2DevBot.Web.Hubs;
using M2DevBot.Web.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace M2DevBot.Web.Services
{
    // TODO: Cache the stuffs

    public interface IProjectService
    {
        string GetProjectName();

        void SetProject(string name);
        List<TodoItem> GetTodoItems();

        void AddTodo(string name);
        void CompleteTodo(int number, bool isComplete);
        void RemoveTodo(int number);
        void ClearTodos();
    }

    public class ProjectService : IProjectService
    {
        private const string FILE_NAME = "C:\\_Stream\\projects.json";

        private readonly IHubContext<BotHub, IBotClient> _hubContext;
        private readonly ILogger<ProjectService> _logger;

        public ProjectService(IHubContext<BotHub, IBotClient> hubContext,
            ILogger<ProjectService> logger)
        {
            _hubContext = hubContext;
            _logger = logger;
        }

        public string GetProjectName()
        {
            var project = GetProject();
            return project?.Name ?? "Working on ASP.NET Core + Vue.js Things";
        }

        public List<TodoItem> GetTodoItems()
        {
            var project = GetProject();
            return project?.TodoItems ?? new List<TodoItem>();
        }

        public void SetProject(string name)
        {
            _logger.LogInformation($"Set project = {name}");

            var project = GetProject();
            project.Name = name;

            Save(project);

            _hubContext.Clients.All.ProjectUpdated(project.Name);
        }

        public void AddTodo(string itemName)
        {
            var project = GetProject();

            var nextSort = project.TodoItems != null && project.TodoItems.Any()
                ? project.TodoItems.Max(x => x.Order) + 1
                : 1;

            var item = new TodoItem
            {
                Name = itemName,
                Order = nextSort
            };

            _logger.LogInformation($"Add todo {item.Name}");

            project.TodoItems.Add(item);

            Save(project);

            _hubContext.Clients.All.TodoItemAdded(item);
        }

        public void CompleteTodo(int number, bool isComplete)
        {
            var project = GetProject();

            var itemIndex = number - 1;

            if (itemIndex < 0 || project.TodoItems.Count < number)
            {
                return;
            }

            var item = project.TodoItems[itemIndex];
            if (item == null)
            {
                return;
            }

            item.IsComplete = isComplete;

            Save(project);

            _hubContext.Clients.All.TodoItemStatusChanged(item);
        }

        public void ClearTodos()
        {
            var project = GetProject();
            project.TodoItems.Clear();

            Save(project);

            _hubContext.Clients.All.TodosCleared();
        }

        public void RemoveTodo(int number)
        {
            var project = GetProject();

            var itemIndex = number - 1;

            if (itemIndex < 0 || project.TodoItems.Count < number)
            {
                return;
            }

            var item = project.TodoItems[itemIndex];
            if (item == null)
            {
                return;
            }

            project.TodoItems.Remove(item);

            Save(project);

            _hubContext.Clients.All.TodoItemRemoved(item);
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
