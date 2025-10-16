using Microsoft.AspNetCore.Mvc;
using PlanetApi.Data;
using PlanetApi.Models;

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
        var response = ApiResponse<IEnumerable<TaskItem>>.SuccessResponse(
            $"Retrieved {tasks.Count} task{(tasks.Count != 1 ? "s" : "")} successfully",
            tasks
        );
        return Ok(response);
    }

    [HttpGet("{id}")]
    public IActionResult GetTask(int id)
    {
        var task = _context.Tasks.FirstOrDefault(t => t.Id == id);
        if (task == null)
        {
            var errorResponse = ApiResponse<TaskItem>.ErrorResponse("Task not found");
            return NotFound(errorResponse);
        }

        var response = ApiResponse<TaskItem>.SuccessResponse("Task retrieved successfully", task);
        return Ok(response);
    }

    [HttpPost]
    public IActionResult CreateTask([FromBody] Models.TaskItem task)
    {
        if (task == null || string.IsNullOrWhiteSpace(task.Title))
        {
            var errorResponse = ApiResponse<TaskItem>.ErrorResponse("Task title is required");
            return BadRequest(errorResponse);
        }

        _context.Tasks.Add(task);
        _context.SaveChanges();

        var response = ApiResponse<TaskItem>.SuccessResponse("Task created successfully", task);
        return CreatedAtAction(nameof(GetTask), new { id = task.Id }, response);
    }

    [HttpPut("{id}")]
    public IActionResult UpdateTask(int id, [FromBody] Models.TaskItem updatedTask)
    {
        if (updatedTask == null || id != updatedTask.Id || string.IsNullOrWhiteSpace(updatedTask.Title))
        {
            var errorResponse = ApiResponse<TaskItem>.ErrorResponse("Invalid task data - please provide valid task information");
            return BadRequest(errorResponse);
        }

        var existingTask = _context.Tasks.FirstOrDefault(t => t.Id == id);
        if (existingTask == null)
        {
            var errorResponse = ApiResponse<TaskItem>.ErrorResponse("Task not found");
            return NotFound(errorResponse);
        }

        existingTask.Title = updatedTask.Title;
        existingTask.Description = updatedTask.Description;
        existingTask.IsCompleted = updatedTask.IsCompleted;

        _context.SaveChanges();

        var response = ApiResponse<TaskItem>.SuccessResponse("Task updated successfully", existingTask);
        return Ok(response);
    }

    [HttpPatch("{id}/complete")]
    public IActionResult MarkTaskAsCompleted(int id)
    {
        var task = _context.Tasks.FirstOrDefault(t => t.Id == id);
        if (task == null)
        {
            var errorResponse = ApiResponse<TaskItem>.ErrorResponse("Task not found");
            return NotFound(errorResponse);
        }

        task.IsCompleted = true;
        _context.SaveChanges();

        var response = ApiResponse<TaskItem>.SuccessResponse($"Task '{task.Title}' marked as completed");
        return Ok(response);
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteTask(int id)
    {
        var task = _context.Tasks.FirstOrDefault(t => t.Id == id);
        if (task == null)
        {
            var errorResponse = ApiResponse<TaskItem>.ErrorResponse("Task not found");
            return NotFound(errorResponse);
        }

        _context.Tasks.Remove(task);
        _context.SaveChanges();

        var response = ApiResponse<TaskItem>.SuccessResponse("Task deleted successfully");
        return Ok(response);
    }
}
