using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatingApp.API.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Controllers
{
    // ** "[controller]" below is the first part of class name = "Values" **
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        // "Init field from param" - allows you to create readonly value you can use throughout program. 
        private readonly DataContext _context;

        // Constructor to - Inject DataContext into Controller
        public ValuesController(DataContext context)
        {
            _context = context;

        }

        // GET api/values
        // Return IActionResult - allows us to return HTTP strings to client - "OK" HTTP 200
        // Asynchronous method - 1. Simple 2. Doesnt affect performance
        [HttpGet]
        public async Task<IActionResult> GetValues()
        {
            // Retrieve a list of values from DB and not return static values. 
            var values = await _context.Values.ToListAsync();

            return Ok(values);
        }

        // GET api/values/5
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetValue(int id)
        {
            // FirstOrDefault - returns default (null) - no exception
            // - Takes a lambda expression - area function to match id we are passing in
            var value = await _context.Values.FirstOrDefaultAsync(x => x.Id == id);

            return Ok(value);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}