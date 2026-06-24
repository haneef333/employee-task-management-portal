using EmployeeTaskManagement.Models;
using EmployeeTaskManagement.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeTaskManagement.Controllers
{
    [Authorize]
    public class TaskController : Controller
    {
        private readonly ITaskRepository _taskRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public TaskController(ITaskRepository taskRepository, UserManager<ApplicationUser> userManager)
        {
            _taskRepository = taskRepository;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(string? status, string? priority)
        {
            var user = await _userManager.GetUserAsync(User);
            var isManager = await _userManager.IsInRoleAsync(user!, "Manager");

            IEnumerable<TaskItem> tasks;

            if (isManager)
                tasks = await _taskRepository.GetAllTasksAsync();
            else
                tasks = await _taskRepository.GetTasksByUserAsync(user!.Id);

            if (!string.IsNullOrEmpty(status) && Enum.TryParse<Models.TaskStatus>(status, out var taskStatus))
                tasks = tasks.Where(t => t.Status == taskStatus);

            if (!string.IsNullOrEmpty(priority) && Enum.TryParse<TaskPriority>(priority, out var taskPriority))
                tasks = tasks.Where(t => t.Priority == taskPriority);

            ViewBag.IsManager = isManager;
            ViewBag.CurrentStatus = status;
            ViewBag.CurrentPriority = priority;

            return View(tasks);
        }

        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Create()
        {
            ViewBag.Users = await _userManager.GetUsersInRoleAsync("Employee");
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Create(TaskItem task, string assignedToUserId)
        {
            var user = await _userManager.GetUserAsync(User);
            task.CreatedByUserId = user!.Id;
            task.AssignedToUserId = assignedToUserId;

            await _taskRepository.AddTaskAsync(task);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Details(int id)
        {
            var task = await _taskRepository.GetTaskByIdAsync(id);
            if (task == null) return NotFound();
            return View(task);
        }

        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Edit(int id)
        {
            var task = await _taskRepository.GetTaskByIdAsync(id);
            if (task == null) return NotFound();
            ViewBag.Users = await _userManager.GetUsersInRoleAsync("Employee");
            return View(task);
        }

        [HttpPost]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Edit(TaskItem task, string assignedToUserId)
        {
            var existing = await _taskRepository.GetTaskByIdAsync(task.Id);
            if (existing == null) return NotFound();

            existing.Title = task.Title;
            existing.Description = task.Description;
            existing.Status = task.Status;
            existing.Priority = task.Priority;
            existing.DueDate = task.DueDate;
            existing.AssignedToUserId = assignedToUserId;

            await _taskRepository.UpdateTaskAsync(existing);
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Delete(int id)
        {
            await _taskRepository.DeleteTaskAsync(id);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateStatus(int id, Models.TaskStatus status)
        {
            var task = await _taskRepository.GetTaskByIdAsync(id);
            if (task == null) return NotFound();

            task.Status = status;
            await _taskRepository.UpdateTaskAsync(task);
            return RedirectToAction("Index");
        }
    }
}