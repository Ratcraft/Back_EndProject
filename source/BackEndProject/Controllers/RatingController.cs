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

namespace Controllers
{
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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Rating>>> GetRates()
        {
            return await _context.Rating.ToListAsync();
        }

        [HttpGet("{id}")]
        public IActionResult GetRatesById(int id)
        {
            var user = _userService.GetById(id);
            var model = _mapper.Map<Rating>(user);
            return Ok(model);
        }

        [HttpPost]
        public async Task<ActionResult<AddRate>> Add_Rates(AddRate rateDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var rate = new Rating()
            {
                Rate = rateDTO.User_Rate
            };
            await _context.Rating.AddAsync(rate);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetRates", new { id = rate.Id }, rateDTO);
        }
    }
}
