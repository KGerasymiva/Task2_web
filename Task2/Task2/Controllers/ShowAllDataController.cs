using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Task2.NewFolder;

namespace Task2.Controllers
{
    public class ShowAllDataController : Controller
    {
        public IActionResult Index()
        {
            var data = ServiceData.GetAllData();

            return View(data);
        }
    }
}