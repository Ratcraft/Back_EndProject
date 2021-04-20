using System.Collections.Generic;
using System.Threading.Tasks;
using Data;
using Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackEndProject.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class JobofferController : ControllerBase
    {
        private readonly Context _context;
        public JobofferController(Context context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Joboffer>>> GetJoboffer()
        {
            return await _context.Joboffer.ToListAsync();
        }

        [HttpPost("create")]
        public async Task<ActionResult<User>> PostJoboffer(Joboffer job)
        {
            _context.Joboffer.Add(job);
            await _context.SaveChangesAsync();

            int b = 0;
            foreach (var people in _context.Joboffer){b++;}

            var boss = await _context.Boss.SingleOrDefaultAsync(x => x.userName == job.bossusername);
            boss.offers.Add(job.id);
            await _context.SaveChangesAsync();

            return CreatedAtAction("PostJoboffer", new {id = b}, job);
        }
    }
}