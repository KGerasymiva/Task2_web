using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Task2.NewFolder;

namespace Task2.Controllers
{
    public class Query6Controller : Controller
    {
        public IActionResult Index()
        {
            ViewBag.Count = NewFolder.ServiceData.Comments.Count;
            return View();
        }

        [HttpPost]
        public IActionResult Index(int id)
        {
            ViewBag.id = id;
            var result = ServiceData.GetPostInfo6(id);
            return View("QueryView", result);
        }
    }
}