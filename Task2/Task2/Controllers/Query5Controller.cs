﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Task2.NewFolder;

namespace Task2.Controllers
{
    public class Query5Controller : Controller
    {
        public IActionResult Index()
        {
            ViewBag.Count = NewFolder.ServiceData.Users.Count;
            return View();
        }

        [HttpPost]
        public IActionResult Index(int id)
        {
            ViewBag.id = id;
            var result = ServiceData.GetUserInfo5(id);
            return View("QueryView", result);
        }
    }
}