using System;
using System.Collections.Generic;
using zadatak1;

namespace zadatak2.ViewModels
{
    public class TodoViewModel
    {
        public Guid ItemId { get; set; }
        public string Text { get; set; }
        public string DeadlineWarningText { get; set; }
        public List<string> labels { get; set; }
        public string DateCompleted { get; set; }
        public string DateDue { get; set; }
        public string LinkText;

        public TodoViewModel(TodoItem item)
        {
            Text = item.Text;
            ItemId = item.Id;
            labels = item.Labels.ConvertAll(lab=>lab.Value);
            DateCompleted = dateToString(item.DateCompleted);
            DateDue = "";
            LinkText = item.DateCompleted == null ? "Mark as completed" : "Remove from completed";

            if (item.DateDue.HasValue)
            {
                DateDue = dateToString(item.DateDue);
                DateTime currentDate = new DateTime(DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.Day);

                int daysLeft = (item.DateDue - currentDate).Value.Days;
                if (daysLeft == 0)
                {
                    DeadlineWarningText = "(danas!)";
                }
                else
                {
                    if (daysLeft < 0)
                    {
                        DeadlineWarningText = "(prije ";
                    }
                    else
                    {
                        DeadlineWarningText = "(za ";
                    }
                    if (Math.Abs(daysLeft)%10 == 1 && Math.Abs(daysLeft) % 100!=11)
                    {
                        DeadlineWarningText += Math.Abs(daysLeft).ToString() + " dan!)";
                    }
                    else
                    {
                        DeadlineWarningText += Math.Abs(daysLeft).ToString() + " dana!)";
                    }
                }
            }
            else
            {
                DeadlineWarningText = "";
            }

            string dateToString(DateTime? date)
            {
                if (date == null)
                {
                    return "";
                }
                return date.ToString().Split(' ')[0];
            }
        }
    }
}
