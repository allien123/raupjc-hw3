using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using zadatak1;
using zadatak2.Data;
using zadatak2.ViewModels;

namespace zadatak2.Controllers
{
    [Authorize]
    public class TodoController : Controller
    {
        private readonly ITodoRepository _repository;
        private readonly UserManager<ApplicationUser> _userManager;


        public TodoController(ITodoRepository repository, UserManager<ApplicationUser> userManager)
        {
            _repository = repository;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            ApplicationUser usr = await _userManager.GetUserAsync(HttpContext.User);
            List<TodoItem> items = _repository.GetActive(usr.GuidId);

            List<TodoViewModel> todoModels = new List<TodoViewModel>();
            foreach (var item in items)
            {
                todoModels.Add(new TodoViewModel(item));
            }

            return View(new IndexViewModel(todoModels));
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> Add(AddTodoViewModel todoModel)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            ApplicationUser usr = await _userManager.GetUserAsync(HttpContext.User);
            TodoItem item = new TodoItem(todoModel.Text, usr.GuidId);
            item.DateCreated = DateTime.Now;
            item.DateDue = todoModel.DateDue;

            string[] labels = todoModel.labels.Split(';');
            item.Labels = new List<TodoItemLabel>();
            
            foreach (string lab in labels)
            {
                TodoItemLabel existingLab = _repository.GetLabel(lab);
                if (existingLab != null)
                {
                    //existingLab.LabelTodoItems.Add(item);
                    item.Labels.Add(existingLab);
                }
                else
                {
                    TodoItemLabel newLabel = new TodoItemLabel(lab);
                    newLabel.LabelTodoItems.Add(item);
                    item.Labels.Add(newLabel);
                }
            }
            _repository.Add(item);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Completed()
        {
            ApplicationUser usr = await _userManager.GetUserAsync(HttpContext.User);
            List<TodoItem> items = _repository.GetCompleted(usr.GuidId);

            List<TodoViewModel> todoModels = new List<TodoViewModel>();
            foreach (var item in items)
            {
                todoModels.Add(new TodoViewModel(item));
            }

            return View(new CompletedViewModel(todoModels));
        }

        [HttpGet("{itemGuid}")]
        public async Task<IActionResult> MarkAsCompleted(Guid itemGuid)
        {
            ApplicationUser usr = await _userManager.GetUserAsync(HttpContext.User);
            TodoItem item = _repository.Get(itemGuid, usr.GuidId);
            if (item == null)
            {
                return Redirect("todo/index");
            }
            if (item.DateCompleted.HasValue)
            {
                _repository.MarkAsNotCompleted(itemGuid, usr.GuidId);
                return Redirect("todo/completed");
            } else {
                _repository.MarkAsCompleted(itemGuid, usr.GuidId);
                return Redirect("todo/index");
            }
            
            
        }

    }

}