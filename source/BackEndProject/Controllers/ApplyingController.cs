using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data;
using Microsoft.EntityFrameworkCore;
using Models;
using DTO;
using Microsoft.AspNetCore.Authorization;

namespace Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class ApplyingController : ControllerBase
    {
        private readonly Context _context;
        public ApplyingController(Context context)
        {
            _context = context;
        }

        [AllowAnonymous]
        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<ApplyingJob>>> GetApplying()
        {
            return await _context.ApplyingJob.ToListAsync();
        }

        [AllowAnonymous]
        [HttpGet("get/{id}")]
        public async Task<ActionResult<ApplyingJob>> GetApplying_byId(int id)
        {
            var applying = await _context.ApplyingJob.ToListAsync<ApplyingJob>();

            var applying_by_id = applying.Find(x => x.id == id);

            if (applying_by_id == null)
            {
                return NotFound();
            }

            return Ok(applying_by_id);
        }

        [AllowAnonymous]
        [HttpGet("get/idApplicant/{idApplicant}")]
        public async Task<ActionResult<IEnumerable<ApplyingJob>>> GetApplying_byIdApplicant(int idApplicant)
        {
            var applying = await _context.ApplyingJob.ToListAsync<ApplyingJob>();//.Where(x => x.idApplicant == idApplicant);

            var applying_by_id = applying.Where(x => x.idApplicant == idApplicant);

            if (applying_by_id == null)
            {
                return NotFound();
            }

            return Ok(applying_by_id);
        }

        [AllowAnonymous]
        [HttpGet("get/idJobOffer/{idJobOffer}")]
        public async Task<ActionResult<IEnumerable<ApplyingJob>>> GetApplying_byIdJobOffer(int idJobOffer)
        {
            var applying = await _context.ApplyingJob.ToListAsync<ApplyingJob>();

            var applying_by_id = applying.Where(x => x.idApplicant == idJobOffer);

            if (applying_by_id == null)
            {
                return NotFound();
            }

            return Ok(applying_by_id);
        }

        [Authorize(Roles = AccessLevel.Boss)]
        [HttpPost("create")]
        public async Task<ActionResult<ApplyingJob>> PostApplying(ApplyingDTO applying_dto)
        {
            var job_offer = await _context.Joboffer.ToListAsync();
            if (job_offer.Find(j => j.id == applying_dto.idJobOffer) == null)
                return BadRequest("Job Offer does not exist!");

            var user = await _context.User.ToListAsync();
            if (user.Find(u => u.id == applying_dto.idApplicant) == null)
                return BadRequest("User does not exist!");

            var app = await _context.ApplyingJob.ToListAsync();

            ApplyingJob applying = new ApplyingJob
            {
                idApplicant = applying_dto.idApplicant,
                idJobOffer = applying_dto.idJobOffer,
                applyingDate = DateTime.Now,
                hired = false
            };

            _context.ApplyingJob.Add(applying);
            await _context.SaveChangesAsync();

            return CreatedAtAction("PostApplying", applying.id, applying);
        }

        /* modify
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
        */
        
        [HttpPut("hired")]
        public async Task<IActionResult> SetHiredApplying(int idBoss, int idApplying, bool hired)
        {
            var applyings = await _context.ApplyingJob.ToListAsync();
            ApplyingJob applying = applyings.Find(a => a.id == idApplying);

            if (applying == null)
                return BadRequest("Applying does not exist! (id incorrect : idApplying)");

            var bosses = await _context.Boss.ToListAsync();
            if (bosses.Find(b => b.id == idBoss) == null)
                return BadRequest("Boss does not exist! (id incorrect : idBoss)");

            _context.Entry(applying).State = EntityState.Modified;

            return CreatedAtAction("PutApplying", idApplying, $"set hired on \"{hired}\"");
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult<ApplyingJob>> DeleteApplying (int id)
        {
            var applyings = await _context.ApplyingJob.ToListAsync();
            ApplyingJob applying = applyings.Find(a => a.id == id);

            if (applying == null)
                return BadRequest("Applying does not exist! (id incorrect)");

            _context.ApplyingJob.Remove(applying);
            await _context.SaveChangesAsync();

            return CreatedAtAction("DeleteApplying", applying);
        }



    }
}
