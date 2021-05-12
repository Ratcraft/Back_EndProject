using AutoMapper;
using Data;
using DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class RatingController : ControllerBase
    {
        private readonly Context _context;
        private IUserService _userService;
        private IMapper _mapper;
        public RatingController(IMapper mapper, IUserService userService, Context context)
        {
            _userService = userService;
            _context = context;
            _mapper = mapper;
        }

        [Authorize(Roles = AccessLevel.Admin + "," + AccessLevel.Employer + "," + AccessLevel.User)]
        [HttpGet]
        public IActionResult GetRating()
        {
            var user = _context.Rating.ToList();
            var model = _mapper.Map<IList<RateModelView>>(user);
            return Ok(model);
        }

        [Authorize(Roles = AccessLevel.User)]
        [HttpGet("my_rating")]
        public IActionResult Get_myRates(int id)
        {
            var user = _userService.GetById(id);
            var model = _mapper.Map<Rating>(user);
            return Ok(model);
        }

        [Authorize(Roles = AccessLevel.Admin + "," + AccessLevel.Employer)]
        [HttpPost]
        public async Task<ActionResult<AddRate>> Add_Rates(AddRate rateDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var rate = new Rating()
            {
                Rate = rateDTO.Rate,
                jobId = rateDTO.jobId,
                comment = rateDTO.comment
            };
            await _context.Rating.AddAsync(rate);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetRates", new { id = rate.id }, rateDTO);
        }
    }
}
