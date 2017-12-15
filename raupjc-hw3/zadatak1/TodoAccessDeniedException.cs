using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zadatak1
{
    public class TodoAccessDeniedException : Exception
    {
        public TodoAccessDeniedException(String message) : base(message)
        {
            
        }
    }
}
