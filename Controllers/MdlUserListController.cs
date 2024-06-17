using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EF_WebApi.Models;
using EF_WebApi.Data;
using Microsoft.EntityFrameworkCore;

namespace EF_WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MdlUserListController : Controller
    {
        public List<MdlUser> mdlUsers = new List<MdlUser>();
        private readonly MoodleContext _context;

        public MdlUserListController(MoodleContext context)
        {
            _context = context;
        } 

        [HttpGet(Name = "GetAPI")]
        public IActionResult Get()
        {
            // mdlUsers = (from us in _context.mdl_user select new { us.Username}).ToList();

            var joint = from us in _context.mdl_user select new { us.Username };

            var mdlUsers = joint.Take(10).ToList();

            string tmp = string.Empty;
            int outfor = 0;

            foreach(var us in mdlUsers)
            {
                tmp += "-" + us.Username;

                outfor++;

                //if(outfor > 5) return Ok(tmp);
            }

            return Ok(tmp);
        }
    }
}