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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Applying>>> GetApplying()
        {
            return await _context.Applying.ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<User>> PostApplying(Applying applying)
        {
            _context.Applying.Add(applying);
            await _context.SaveChangesAsync();

            int b = 0;
            foreach (var app in _context.Applying) { b++; }

            return CreatedAtAction("PostBoss", new { id = b }, applying);
        }
    }
}
