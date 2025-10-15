using Microsoft.AspNetCore.Mvc;
using PlanetApi.Data;

namespace PlanetApi.Controllers
{
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
    }
}
