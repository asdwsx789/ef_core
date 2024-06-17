using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace EF_WebApi.Controllers
{
    public class FromInfoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}