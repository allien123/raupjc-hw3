using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Isam.Esent.Interop;
using zadatak1;

namespace zadatak2.ViewModels
{
    public class CompletedViewModel
    {
        public List<TodoViewModel> TodoModels { get; set; }

        public CompletedViewModel(List<TodoViewModel> todoModels)
        {
            TodoModels = todoModels;
        }
    }
}

