using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Task2.NewFolder;

namespace Task2.Controllers
{
    public class Query4Controller : Controller
    {

      
        public IActionResult Index()
        {
            var result = ServiceData.GetUserTodoList();
            return View("QueryView",result);
        }
    }
}