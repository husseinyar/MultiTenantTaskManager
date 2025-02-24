using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MultiTenantTaskManager.Data;
using MultiTenantTaskManager.Models;
using System.Security.Claims;

namespace MultiTenantTaskManager.Controllers
{
    [Route("api/tasks")]
    [ApiController]
   
    public class TaskController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TaskController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody] TaskItem taskItem)
        {
            var userTenantId = User.FindFirst("TenantId")?.Value;
            if (string.IsNullOrEmpty(userTenantId))
                return Unauthorized(new { message = "User does not belong to a tenant" });

            taskItem.TenantId = userTenantId;
            _context.Tasks.Add(taskItem);
            await _context.SaveChangesAsync();

            return Ok(taskItem);
        }

        [HttpGet]
        public async Task<IActionResult> GetTasks()
        {
            var userTenantId = User.FindFirst("TenantId")?.Value;
            if (string.IsNullOrEmpty(userTenantId))
                return Unauthorized(new { message = "User does not belong to a tenant" });

            var tasks = await _context.Tasks.Where(t => t.TenantId == userTenantId).ToListAsync();
            return Ok(tasks);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(int id, [FromBody] TaskItem updatedTask)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null) return NotFound();

            task.Title = updatedTask.Title;
            task.Description = updatedTask.Description;
            task.Status = updatedTask.Status;
            task.AssignedToUserId = updatedTask.AssignedToUserId;

            await _context.SaveChangesAsync();
            return Ok(task);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null) return NotFound();

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Task deleted" });
        }
    }
}
