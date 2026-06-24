using EmployeeTaskManagement.Models;
using EmployeeTaskManagement.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeTaskManagement.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ITaskRepository _taskRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(ITaskRepository taskRepository, UserManager<ApplicationUser> userManager)
        {
            _taskRepository = taskRepository;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var isManager = await _userManager.IsInRoleAsync(user!, "Manager");

            IEnumerable<TaskItem> tasks;

            if (isManager)
                tasks = await _taskRepository.GetAllTasksAsync();
            else
                tasks = await _taskRepository.GetTasksByUserAsync(user!.Id);

            ViewBag.TotalTasks = tasks.Count();
            ViewBag.TodoTasks = tasks.Count(t => t.Status == Models.TaskStatus.ToDo);
            ViewBag.InProgressTasks = tasks.Count(t => t.Status == Models.TaskStatus.InProgress);
            ViewBag.DoneTasks = tasks.Count(t => t.Status == Models.TaskStatus.Done);
            ViewBag.IsManager = isManager;
            ViewBag.UserName = user!.FullName;

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}