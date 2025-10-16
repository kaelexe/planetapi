using Microsoft.AspNetCore.Mvc;
using PlanetApi.Data;

namespace PlanetApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TasksController : ControllerBase
{
    private readonly PlanetContext _context;

    public TasksController(PlanetContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult GetTasks()
    {
        var tasks = _context.Tasks.ToList();
        return Ok(tasks);
    }

    [HttpGet("{id}")]
    public IActionResult GetTask(int id)
    {
        var task = _context.Tasks.FirstOrDefault(t => t.Id == id);
        if (task == null)
        {
            return NotFound();
        }
        return Ok(task);
    }

    [HttpPost]
    public IActionResult CreateTask([FromBody] Models.TaskItem task)
    {
        if (task == null || string.IsNullOrWhiteSpace(task.Title))
        {
            return BadRequest("Task title is required.");
        }

        _context.Tasks.Add(task);
        _context.SaveChanges();

        return CreatedAtAction(nameof(GetTask), new { id = task.Id }, task);
    }

    [HttpPut("{id}")]
    public IActionResult UpdateTask(int id, [FromBody] Models.TaskItem updatedTask)
    {
        if (updatedTask == null || id != updatedTask.Id || string.IsNullOrWhiteSpace(updatedTask.Title))
        {
            return BadRequest("Invalid task data.");
        }

        var existingTask = _context.Tasks.FirstOrDefault(t => t.Id == id);
        if (existingTask == null)
        {
            return NotFound();
        }

        existingTask.Title = updatedTask.Title;
        existingTask.Description = updatedTask.Description;
        existingTask.IsCompleted = updatedTask.IsCompleted;

        _context.SaveChanges();

        return NoContent();
    }

    [HttpPatch("{id}/complete")]
    public IActionResult MarkTaskAsCompleted(int id)
    {
        var task = _context.Tasks.FirstOrDefault(t => t.Id == id);
        if (task == null)
        {
            return NotFound();
        }

        task.IsCompleted = true;
        _context.SaveChanges();

        return NoContent();
    }
}
