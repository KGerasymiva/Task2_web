using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Task2.Models
{
    public class Query4Model
    {
        public List<Tuple<User, List<Todo>>> TodoList { get; set; }
        //public string UserName { get; set; }
        //public IEnumerable<string> Todos { get; set; }
    }
}
