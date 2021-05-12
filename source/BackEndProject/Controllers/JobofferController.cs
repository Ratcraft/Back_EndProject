using System.Collections.Generic;
using System.Threading.Tasks;
using Data;
using Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using DTO;

namespace Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class JobofferController : ControllerBase
    {
        private readonly Context _context;
        public JobofferController(Context context)
        {
            _context = context;
        }

        [Authorize(Roles = AccessLevel.Admin + "," + AccessLevel.Employer + "," + AccessLevel.User)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Joboffer>>> GetJoboffer()
        {
            return await _context.Joboffer.ToListAsync();
        }

        [Authorize(Roles = AccessLevel.Admin + "," + AccessLevel.Employer)]
        [HttpPost("create")]
        public async Task<ActionResult<User>> PostJoboffer(Joboffer job)
        {
            _context.Joboffer.Add(job);
            await _context.SaveChangesAsync();

            int b = 0;
            foreach (var people in _context.Joboffer){b++;}

            await _context.SaveChangesAsync();

            return CreatedAtAction("PostJoboffer", new {id = b}, job);
        }

        [Authorize(Roles = AccessLevel.Admin + "," + AccessLevel.Employer)]
        [HttpDelete("removeoffer")]
        public async Task<ActionResult<Joboffer>> Delete_joboffer(string name)
        {
            var job = await _context.Joboffer.SingleOrDefaultAsync(x => x.name == name);
            if(job == null){return NotFound();}

            _context.Joboffer.Remove(job);
            await _context.SaveChangesAsync();

            return job;
        }

        [Authorize(Roles = AccessLevel.Admin + "," + AccessLevel.Employer)]
        [HttpPut("modify")]
        public async Task<IActionResult> Modify_Offer(int id, Joboffer joboffer)
        {
            if(id != joboffer.id){return BadRequest();}
            _context.Entry(joboffer).State = EntityState.Modified;
            try{await _context.SaveChangesAsync();}
            catch(DbUpdateConcurrencyException)
            {
                if(_context.Joboffer.FirstOrDefaultAsync(x => x.id == id) == null){return NotFound();}
                else{throw;}
            }

            return NoContent();
        }

    }
}