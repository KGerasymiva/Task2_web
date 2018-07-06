using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Task2.Models
{
    public class Query6Model
    {
        public Post Post { get; set; }
        public Comment LongestComment { get; set; }
        public Comment LikestComment { get; set; }
        public int? LikesCount { get; set; }

    }
}
