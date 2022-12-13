using DockerAndKubernetes.WebApi.Models;
using DockerAndKubernetes.WebApi.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace DockerAndKubernetes.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PresentationController : ControllerBase
    {
        private readonly ITodoItemRepository _repository;
        public PresentationController(ITodoItemRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        [HttpGet]
        public async Task<ActionResult<TodoItem>> GetTodoItem()
        {
            var discount = _repository.GetTodoItem();
            var list = new List<TodoItem>();
            foreach (var item in discount)
            {
                list.Add(item);
            }
            return Ok(list);
        }

        [HttpPost]
        [ProducesResponseType(typeof(TodoItem), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<TodoItem>> CreateTodoItem([FromBody] TodoItem todoItem)
        {
            await _repository.CreateTodoItem(todoItem);
            //return CreatedAtRoute("GetTodoItem", new { name = todoItem.Name }, todoItem);
            return Ok();
        }

        [HttpPut]
        [ProducesResponseType(typeof(TodoItem), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<TodoItem>> UpdateTodoItem([FromBody] TodoItem todoItem)
        {
            return Ok(await _repository.UpdateTodoItem(todoItem));
        }

        [HttpDelete]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<bool>> DeleteTodoItem(int id)
        {
            return Ok(await _repository.DeleteTodoItem(id));
        }
    }
}
