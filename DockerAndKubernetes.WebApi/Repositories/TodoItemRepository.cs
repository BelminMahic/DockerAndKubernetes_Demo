using Dapper;
using DockerAndKubernetes.WebApi.Models;
using Npgsql;

namespace DockerAndKubernetes.WebApi.Repositories
{
    public class TodoItemRepository : ITodoItemRepository
    {

        private readonly IConfiguration _configuration;

        public TodoItemRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }
               

        public async Task<bool> CreateTodoItem(TodoItem todoItem)
        {
            using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

            var affected = await connection.ExecuteAsync
                ("INSERT INTO TodoItem (Name, IsComplete) VALUES (@Name, @IsComplete)",
                new { Name = todoItem.Name, IsComplete = todoItem.IsComplete });

            if (affected == 0)
                return false;

            return true;
        }

        public async Task<bool> DeleteTodoItem(int id)
        {
            using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

            var affected = await connection.ExecuteAsync
                ("DELETE FROM TodoItem WHERE Id=@id", new { Id = id });

            if (affected == 0)
                return false;

            return true;
        }

        public IEnumerable<TodoItem> GetTodoItem()
        {
            using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

            var todoItem = connection.QueryAsync<TodoItem>
                ("SELECT * FROM TodoItem");

            return todoItem.Result.ToList();

        }

        public async Task<bool> UpdateTodoItem(TodoItem todoItem)
        {
            using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

            var affected = await connection.ExecuteAsync
                ("UPDATE TodoItem SET Name=@Name, IsComplete=@IsComplete WHERE Id=@Id",
                new { Name = todoItem.Name, IsComplete = todoItem.IsComplete, Id = todoItem.Id });

            if (affected == 0)
                return false;
            return true;
        }
    }
}
