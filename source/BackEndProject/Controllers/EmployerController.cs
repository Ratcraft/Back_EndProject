/*using System.Collections.Generic;
using System.Threading.Tasks;
using Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using DTO;
using Service;
using AutoMapper;
using AutoMapper.Configuration;
using Helpers;

namespace Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class EmployerController : ControllerBase
    {
        private IUserService _userService;
        private IMapper _mapper;
        public IConfiguration _configuration;
        private readonly Context _context;
        private readonly IEmailService _emailService;

        public EmployerController(
            Context context,
            IUserService userService,
            IMapper mapper,
            IConfiguration configuration,
            IEmailService emailService)
        {
            _context = context;
            _userService = userService;
            _mapper = mapper;
           _configuration = configuration;
           _emailService = emailService;
        }

        [Authorize(Roles = AccessLevel.Employer)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employer>>> GetBoss()
        {
            return await _context.Employer.ToListAsync();
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register([FromBody]CreateEmployer model)
        {
            // map model to entity
            var employer = _mapper.Map<Employer>(model);

            try
            {
                // create user
                _userService.CreateEmployer(employer, model.password);
                return Ok();
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}*/