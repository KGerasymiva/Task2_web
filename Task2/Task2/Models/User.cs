using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Task2.Models
{
    public class User
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Name { get; set; }
        public string Avatar { get; set; }
        public string EMail { get; set; }
        public IEnumerable<Post> Posts { get; set; } = new List<Post>();
        public IEnumerable<Todo> Todos { get; set; } = new List<Todo>();

    }
}

