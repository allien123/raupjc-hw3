using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace zadatak2.ViewModels
{
    public class IndexViewModel
    {
        public List<TodoViewModel> TodoModels { get; set; }

        public IndexViewModel(List<TodoViewModel> todoModels)
        {
            TodoModels = todoModels;
        }
    }
}
