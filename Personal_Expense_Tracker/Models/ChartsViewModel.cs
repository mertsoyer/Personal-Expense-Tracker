using System;
using System.Collections.Generic;

namespace Personal_Expense_Tracker.Models
{
    public class ChartsViewModel
    {
        public ChartsViewModel()
        {
            gelirler = new List<Class>();
            giderler = new List<Class>();
        }
        public List<Class> gelirler { get; set; }
        public List<Class> giderler { get; set; }
       
    }
}
