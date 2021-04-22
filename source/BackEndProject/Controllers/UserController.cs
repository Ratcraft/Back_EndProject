using Data;
using Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System;

namespace BackEndProject.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly Context _context;
        public UserController(Context context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUser()
        {
            return await _context.User.ToListAsync();
        }

        [HttpPost("create_account")]
        public async Task<ActionResult<User>> PostUsers(User user)
        {
            _context.User.Add(user);
            await _context.SaveChangesAsync();

            int b = 0;
            foreach (var people in _context.User){b++;}

            return CreatedAtAction("PostUser", new {id = b}, user);
        }

        [HttpPut("modify")]
        public async Task<IActionResult> Modify_User(int id, User profile)
        {
            if(id != profile.id){return BadRequest();}
            _context.Entry(profile).State = EntityState.Modified;
            try{await _context.SaveChangesAsync();}
            catch(DbUpdateConcurrencyException)
            {
                if(_context.User.FirstOrDefaultAsync(x => x.id == id) == null){return NotFound();}
                else{throw;}
            }

            return NoContent();
        }
    }
}