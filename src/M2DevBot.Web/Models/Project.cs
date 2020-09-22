using System.Collections.Generic;

namespace M2DevBot.Web.Models
{
    public class Project
    {
        public string Name { get; set; }

        public List<TodoItem> TodoItems { get; set; } = new List<TodoItem>();
    }

    public class TodoItem
    {
        public int Order { get; set; }
        public string Name { get; set; }
        public bool IsComplete { get; set; }
    }
}
