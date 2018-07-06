using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Task2.Models
{
    public class Query5Model
    {
        public User User { get; set; }
        public Post LastPost { get; set; }
        public int? LastPostCommentCount { get; set; }
        public int? UndoneTasksCount { get; set; }
        public Post PopPostByComments { get; set; }
        public Post PopPostByLikes { get; set; }
        
    }
}
