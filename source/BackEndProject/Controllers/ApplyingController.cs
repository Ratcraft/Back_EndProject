using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data;
using Microsoft.EntityFrameworkCore;
using Models;

namespace BackEndProject.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ApplyingController : ControllerBase
    {
        private readonly Context _context;
        public ApplyingController(Context context)
        {
            _context = context;
        }

        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<Applying>>> GetApplying()
        {
            return await _context.Applying.ToListAsync();
        }

        [HttpPost("create")]
        public async Task<ActionResult<Applying>> PostApplying(Applying applying)
        {
            _context.Applying.Add(applying);
            await _context.SaveChangesAsync();

            int b = 0;
            foreach (var app in _context.Applying) { b++; }

            return CreatedAtAction("PostApplying", new { id = b }, applying);
        }

        [HttpPut("modify")]
        public async Task<IActionResult> ModifyApplying(int id, Applying applying)
        {
            if (id != applying.idApplicant) { return BadRequest(); }
            _context.Entry(applying).State = EntityState.Modified;
            try { await _context.SaveChangesAsync(); }
            catch (DbUpdateConcurrencyException)
            {
                if (_context.Applying.FirstOrDefaultAsync(x => x.id == id) == null) { return NotFound(); }
                else { throw; }
            }

            return NoContent();
        }

        [HttpDelete("delete")]
        public async Task<ActionResult<Applying>> DeleteApplying (Applying applying)
        {
            _context.Applying.Remove(applying);
            await _context.SaveChangesAsync();

            return CreatedAtAction("DeleteApplying", applying);
        }



    }
}
