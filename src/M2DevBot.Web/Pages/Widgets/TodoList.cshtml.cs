using System.Collections.Generic;
using M2DevBot.Web.Models;
using M2DevBot.Web.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace M2DevBot.Web.Pages.Widgets
{
    public class TodoListWidgetModel : PageModel
    {
        private readonly IProjectService _projectService;

        public string ProjectName { get; set; }
        public List<TodoItem> TodoItems { get; set; }

        public TodoListWidgetModel(IProjectService projectService)
        {
            _projectService = projectService;
        }

        public void OnGet()
        {
            ProjectName = _projectService.GetProjectName();
            TodoItems = _projectService.GetTodoItems();
        }
    }
}
