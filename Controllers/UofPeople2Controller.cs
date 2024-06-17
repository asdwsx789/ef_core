using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace EF_WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UofPeople2Controller : Controller
    {
        [HttpGet("Get")]
        public IActionResult Get()
        {
            return Ok("get1");
        }

        [HttpGet("Get2")]
        public IActionResult Get2()
        {
            return Ok("get2");
        }
    }
}