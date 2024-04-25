using System.Security.Claims;
using brokenaccesscontrol.Models;
using brokenaccesscontrol.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace brokenaccesscontrol.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ToDoItemController : ControllerBase
{

    private readonly ILogger<ToDoItemController> _logger;

    public ToDoItemController(ILogger<ToDoItemController> logger)
    {
        _logger = logger;
    }   

    // GET: api/ToDoItem
    
    [HttpGet]
    public async Task<IEnumerable<TodoItem>> GetToDoItems()
    {
        return await TodoItemRepository.GetToDoItems();
    }

  
    [HttpPost]
    public async Task<ActionResult> Post(TodoItemNewRequest todoItem)
    {
        try{
            var ret = await TodoItemRepository.Insert(todoItem, ((ClaimsIdentity)User.Identity).FindFirst("UserId").Value);

            if (ret)
                return Ok(new
                {
                    todoItem = todoItem,
                    message = "Success"
                });                     
            else
                throw new Exception("Error contact the system admin!!");

        }catch(Exception ex){
            _logger.LogError(ex, "General error");
            return StatusCode(500, "Internal server error");            
        }        
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetToDoItem(int id)
    {
        try{
            var todoItem = await TodoItemRepository.GetToDoItem(id);

            if (todoItem != null)
                return Ok(new
                {
                    todoItem
                });                     
            else
                return NotFound(new{
                    message = "TodoItem not found!!"
                });

        }catch(Exception ex){
            _logger.LogError(ex, "General error");
            return StatusCode(500, "Internal server error");            
        }     
    }    

    
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        try{
            var ret = await TodoItemRepository.Delete(id);

            if (ret)
                return Ok(new
                {
                    message = "Removed!"
                });                     
            else
                throw new Exception("Error contact the system admin!!");

        }catch(Exception ex){
            _logger.LogError(ex, "General error");
            return StatusCode(500, "Internal server error");            
        }        
    }
}