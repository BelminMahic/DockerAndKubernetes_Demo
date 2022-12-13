using DockerAndKubernetes.WebApi.Models;

namespace DockerAndKubernetes.WebApi.Repositories
{
    public interface ITodoItemRepository
    {
        IEnumerable<TodoItem> GetTodoItem();
        Task<bool> UpdateTodoItem(TodoItem todoItem);
        Task<bool> CreateTodoItem(TodoItem todoItem);
        Task<bool> DeleteTodoItem(int id);
    }
}
