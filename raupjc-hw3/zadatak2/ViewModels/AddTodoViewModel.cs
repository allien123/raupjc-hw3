using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace zadatak2.ViewModels
{
    public class AddTodoViewModel
    {
        [Required]
        public string Text { get; set; }
        [DataType(DataType.Date)]
        public DateTime? DateDue { get; set; }

        public string labels { get; set; }
    }
}
