using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace zadatak1
{
    public class TodoSqlRepository : ITodoRepository
    {
        private readonly TodoDbContext _context;

        public TodoSqlRepository(TodoDbContext context)
        {
            _context = context;
        }

        public TodoItem Get(Guid todoId, Guid userId)
        {
            TodoItem item = _context.TodoItems.Where(i=>i.Id.Equals(todoId)).FirstOrDefault();
            if (item == null)
            {
                return item;
            }

            if (item.UserId != userId)
            {
                throw new TodoAccessDeniedException($"The user with id={userId} is not the owner of item with id={todoId}.");
            }
            return item;
        }

        public void Add(TodoItem todoItem)
        {
            if (_context.TodoItems.Find(todoItem.Id)!=null)
            {
                throw new DuplicateTodoItemException(todoItem.Id);
            }
            _context.TodoItems.Add(todoItem);
            _context.SaveChanges();
        }

        public bool Remove(Guid todoId, Guid userId)
        {
            TodoItem item = Get(todoId, userId);
            if (item == null)
            {
                return false;
            }

            _context.TodoItems.Remove(item);
            _context.SaveChanges();
            return true;
        }

        public void Update(TodoItem todoItem, Guid userId)
        {
            if (!_context.TodoItems.Contains(todoItem))
            {
                Add(todoItem);
                return; 
            }
            if (todoItem.UserId != userId)
            {
                throw new TodoAccessDeniedException($"The user with id={userId} is not the owner of item={todoItem}.");
            }
            _context.Entry(todoItem).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public bool MarkAsCompleted(Guid todoId, Guid userId)
        {
            TodoItem item = Get(todoId, userId);
            if (item == null)
            {
                return false;
            }

            item.MarkAsCompleted();
            _context.SaveChanges();
            return true;
        }

        public bool MarkAsNotCompleted(Guid todoId, Guid userId)
        {
            TodoItem item = Get(todoId, userId);
            if (item == null)
            {
                return false;
            }

            item.MarkAsNotCompleted();
            _context.SaveChanges();
            return true;
        }

        public List<TodoItem> GetAll(Guid userId)
        {
            return _context.TodoItems.Where(i=>i.UserId==userId).OrderByDescending(i=>i.DateCreated).ToList();
        }

        public List<TodoItem> GetActive(Guid userId)
        {
            return _context.TodoItems.Where(i => i.UserId == userId && (i.DateCompleted==null)).Include(i=>i.Labels).ToList();
        }

        public List<TodoItem> GetCompleted(Guid userId)
        {
            return _context.TodoItems.Where(i => i.UserId == userId && (i.DateCompleted != null)).Include(i => i.Labels).ToList();
        }

        public List<TodoItem> GetFiltered(Func<TodoItem, bool> filterFunction, Guid userId)
        {
            return _context.TodoItems.Where(i => i.UserId == userId && filterFunction(i)).ToList();
        }

        public TodoItemLabel GetLabel(string label)
        {
            return _context.TodoLabel.Where(lab => lab.Value.Equals(label)).Include(lab=>lab.LabelTodoItems).FirstOrDefault();
        }
    }
}
