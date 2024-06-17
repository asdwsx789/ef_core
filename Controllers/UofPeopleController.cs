#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EF_WebApi.Data;
using System.Globalization;
using Newtonsoft.Json;
using EF_WebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace EF_WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UofPeopleController : Controller
    {
        private readonly ErpsyncContext _context;
        private readonly ShandbContext _shancontest;

        public UofPeopleController(ErpsyncContext context, ShandbContext shancontext)
        {
            _context = context;
            _shancontest = shancontext;
        } 

        [HttpGet(Name = "GetusAPI")]
        public IActionResult Get()
        {
            if(!_shancontest.uof_people.Any()) return BadRequest("{Error: table not data is null.}"); 

            var up3 = (from ufp_g in (
                            from ufp in _shancontest.uof_people
                            orderby ufp.WRITE_TIME 
                            select new {
                                time = DateTime.ParseExact(ufp.WRITE_TIME, "yyyy-MM-dd HH:mm:ss", CultureInfo.CurrentCulture).ToString("yyyyMMdd-HH"),
                                people = Int32.Parse(ufp.UOF_PEOPLE)
                            }).ToList()
                        group ufp_g by ufp_g.time into g
                        select new {
                            people = g.Average(x => x.people),
                            time = g.Key
                        }
                    ).ToList();

            //string json = JsonConvert.SerializeObject(up3);

            SyncTable();

            return Ok();
        }

        private void SyncTable()
        {
            var mss_date = (from us in _context.uof_people select us).ToList();

            foreach(var item in mss_date)
            {
                if(!_shancontest.uof_people.Any(p => p.WRITE_TIME == item.WRITE_TIME))
                {
                    _shancontest.Database.ExecuteSqlRaw("INSERT INTO uof_people (WRITE_TIME, UOF_PEOPLE) VALUES ({0}, {1})", item.WRITE_TIME, item.UOF_PEOPLE);
                }
            }
        }

        [HttpGet("GetAllList")]
        public IActionResult GetPeopleAllList()
        {
            if(!_shancontest.uof_people.Any()) return BadRequest("{Error: table not data is null.}");

            var allList = (from ufp in _shancontest.uof_people
                            orderby ufp.WRITE_TIME
                            select new {
                                time = DateTime.ParseExact(ufp.WRITE_TIME, "yyyy-MM-dd HH:mm:ss", CultureInfo.CurrentCulture).ToString("yyyyMMdd"),
                                people = Int32.Parse(ufp.UOF_PEOPLE)
                            }
                        ).ToList();

            string json = JsonConvert.SerializeObject(allList);

            return Ok(json);
        }

        [HttpGet("GetPeopleListForHr")]
        public IActionResult GetPeopleListForHr()
        {
            if(!_shancontest.uof_people.Any()) return BadRequest("{Error: table not data is null.}");

            var allList = (from ufp in _shancontest.uof_people
                            orderby ufp.WRITE_TIME
                            select new {
                                time = DateTime.ParseExact(ufp.WRITE_TIME, "yyyy-MM-dd HH:mm:ss", CultureInfo.CurrentCulture).ToString("yyyyMMddHH"),
                                people = Int32.Parse(ufp.UOF_PEOPLE)
                            }
                        ).ToList();

            string json = JsonConvert.SerializeObject(allList);

            return Ok(json);
        }

        [HttpGet("GetNotFoundData")]
        public IActionResult GetNotFoundData()
        {
            if(!_context.uof_people.Any()) return BadRequest("{Error: table not data is null.}");

            // Get sqlserver and mysql uof_people table
            var mss = (from tb1 in _context.uof_people
                        orderby tb1.WRITE_TIME
                        select new {
                            tb1.WRITE_TIME,
                            tb1.UOF_PEOPLE
                        }
                    ).ToList();

            var msq = (from tb2 in _shancontest.uof_people
                        orderby tb2.WRITE_TIME
                        select new {
                            tb2.WRITE_TIME,
                            tb2.UOF_PEOPLE
                        }
                    ).ToList();

            // select mysql.uofpeole no data
            var nodata = (from tb3 in mss 
                        join tb4 in msq on tb3.WRITE_TIME equals tb4.WRITE_TIME into gj
                        from subgroup in gj.DefaultIfEmpty()
                        where subgroup?.WRITE_TIME is null
                        select new {
                            tb3.WRITE_TIME,
                            tb3.UOF_PEOPLE
                        }
                    ).ToList();

            // new UofPerson list and insert data to mysql.uofpeople
            // var uofPeopleList = nodata.Select(item => new UofPerson {
            //     WRITE_TIME = item.WRITE_TIME,
            //     UOF_PEOPLE = item.UOF_PEOPLE
            // }).ToList();

            // _shancontest.uof_people.AddRange(uofPeopleList);
            // _shancontest.SaveChanges();

            foreach(var item in nodata)
            {
                _shancontest.Database.ExecuteSqlRaw("INSERT INTO uof_people (WRITE_TIME, UOF_PEOPLE) VALUES ({0}, {1})", item.WRITE_TIME, item.UOF_PEOPLE);
            }

            return Ok(nodata);
        }
    }
}