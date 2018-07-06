using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Task2.Models;
using Task2.NewFolder;

namespace Task2.Controllers
{
    public class ShowPostController : Controller
    {
 
        public IActionResult ShowPost(int id)
        {
            var post = ServiceData.GetPostById(id);

            return View(post);
        }

    }
}