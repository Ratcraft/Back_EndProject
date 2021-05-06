using System.Collections.Generic;
using System.Threading.Tasks;
using Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BossController : ControllerBase
    {
        private readonly Context _context;
        public BossController(Context context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employer>>> GetBoss()
        {
            return await _context.Employer.ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<Employer>> PostBoss(Employer boss)
        {
            _context.Employer.Add(boss);
            await _context.SaveChangesAsync();

            int b = 0;
            foreach (var people in _context.Employer){b++;}

            return CreatedAtAction("PostBoss", new {id = b}, boss);
        }
    }
}