using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManager.Application.Common.Interfaces;
using TaskManager.Application.DTOs;
using TaskManager.Domain.Entities;

namespace TaskManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase{
    private readonly IApplicationDbContext _context;

    public TasksController(IApplicationDbContext context) => _context = context;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TaskResponse>>> GetTasks()
    {
        var taskResponses = await _context.Tasks
            .Select(task => new TaskResponse(
                task.Id,
                task.Title,
                task.IsCompleted,
                task.UserId
            ))
            .ToListAsync();

        return Ok(taskResponses);
    }

    [HttpPost]
    public async Task<ActionResult<TaskResponse>> CreateTask([FromBody] CreateTaskRequest request){
        var newTask = new TaskItem{
            Id = Guid.NewGuid().ToString(),
            Title = request.Title,
            UserId = request.UserId,
            IsCompleted = false
        };

        _context.Tasks.Add(newTask);
        await _context.SaveChangesAsync();

        var response = new TaskResponse(
            newTask.Id,
            newTask.Title,
            newTask.IsCompleted,
            newTask.UserId
        );

        return CreatedAtAction(nameof(GetTasks), response);
    }
    [HttpPut("{id}")]
    public async Task<ActionResult<TaskResponse>> UpdateTask([FromRoute] String id, [FromBody] UpdateTaskStatusRequest request){
        var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id);
        if (task == null){
            return NotFound();
        }
        task.Title = request.Title;
        task.IsCompleted = request.IsCompleted;
        await _context.SaveChangesAsync();

        var response = new TaskResponse(
            task.Id,
            task.Title,
            task.IsCompleted,
            task.UserId
        );

        return Ok(response);

    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTask([FromRoute] string id)
    {
        var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id);

        if (task == null)
        {
            return NotFound();
        }
        _context.Tasks.Remove(task);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TaskResponse>> GetTaskById([FromRoute] string id)
    {
        var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id);

        if (task == null)
        {
            return NotFound();
        }

        var response = new TaskResponse(
            task.Id,
            task.Title,
            task.IsCompleted,
            task.UserId
        );

        return Ok(response);
    }
}
