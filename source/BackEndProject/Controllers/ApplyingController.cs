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
using AutoMapper;
using Microsoft.AspNetCore.Authorization;

namespace Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class ApplyingController : ControllerBase
    {
        private readonly Context _context;
        private readonly IMapper _mapper;
        public ApplyingController(Context context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [Authorize(Roles = AccessLevel.Employer)]
        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<ApplyingJob>>> GetApplying()
        {
            return await _context.ApplyingJob.ToListAsync();
        }

        [Authorize(Roles = AccessLevel.User)]
        [HttpGet("my_applying")]
        public IActionResult GetApplying_my_applying(int id)
        {
            List<ApplyingJob> applyingJobs = _context.ApplyingJob.ToList();
            List<Joboffer> joboffers = _context.Joboffer.ToList();

            List<Joboffer> result = new List<Joboffer>();
            foreach (var item in applyingJobs)
            {
                if(item.idApplicant == id){
                    foreach(var job in joboffers)
                    {
                        if(job.id == item.idJobOffer){
                            result.Add(job);
                        }
                    }
                }
            }
            return Ok(result);
        }

        [Authorize(Roles = AccessLevel.User)]
        [HttpGet("my_hired_job")]
        public IActionResult GetApplying_hired_job(int id)
        {
            List<ApplyingJob> applyingJobs = _context.ApplyingJob.ToList();
            List<Joboffer> joboffers = _context.Joboffer.ToList();

            List<Joboffer> result = new List<Joboffer>();
            foreach (var item in applyingJobs)
            {
                if(item.idApplicant == id){
                    if(item.hired)
                    {
                        foreach(var job in joboffers)
                        {
                            if(job.id == item.idJobOffer){result.Add(job);}
                        }
                    }
                    
                }
            }
            return Ok(result);
        }

        [Authorize(Roles = AccessLevel.Employer)]
        [HttpGet("applying_user")]
        public IActionResult GetApplying_applying_user(int idJobOffer)
        {
            List<ApplyingJob> applyingJobs = _context.ApplyingJob.ToList();
            List<User> users = _context.User.ToList();

            List<User> result = new List<User>();
            foreach (var item in applyingJobs)
            {
                if(item.idJobOffer == idJobOffer){
                    foreach(var us in users)
                    {
                        if(us.id == item.idApplicant){result.Add(us);}
                    }
                }
            }
            return Ok(result);
        }

        [Authorize(Roles = AccessLevel.User)]
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
        [Authorize(Roles = AccessLevel.Employer)]
        [HttpPut("hired")]
        public async Task<IActionResult> SetHiredApplying(int idApplying, bool hired)
        {
            var applyings = await _context.ApplyingJob.ToListAsync();
            ApplyingJob applying = applyings.Find(a => a.id == idApplying);

            if (applying == null)
                return BadRequest("Applying does not exist! (id incorrect : idApplying)");

            _context.ApplyingJob.SingleOrDefault(x => x.id == idApplying).hired = hired;
            _context.Entry(applying).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return Ok();
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
