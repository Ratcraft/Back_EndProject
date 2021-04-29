using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DTO;
using Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Models;
using Services;
using Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private IUserService _userService;
        private IMapper _mapper;
        public IConfiguration Configuration;
        private readonly Context _context;
        private readonly IEmailService _emailService;

        public UserController(
            Context context,
            IUserService userService,
            IMapper mapper,
            IConfiguration configuration,
            IEmailService emailService)
        {
            _context = context;
            _userService = userService;
            _mapper = mapper;
           Configuration = configuration;
           _emailService = emailService;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate(AuthenticateModel model)
        {
            var user = _userService.Authenticate(model.Username, model.Password);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(Configuration["Secret"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.id.ToString()),
                    new Claim(ClaimTypes.Role, user.levelAccess.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            // return basic user info and authentication token
            return Ok(new
            {
                Id = user.id,
                Username = user.userName,
                FirstName = user.firstName,
                LastName = user.lastName,
                Token = tokenString
            });
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register([FromBody]CreateUser model)
        {
            // map model to entity
            var user = _mapper.Map<User>(model);

            try
            {
                // create user
                _userService.Create(user, model.Password);
                return Ok();
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet]
        public IActionResult GetUser()
        {
            var user = _context.User.ToList();
            var model = _mapper.Map<IList<UserViewModel>>(user);
            return Ok(model);
        }

        [HttpPost("create_account")]
        public async Task<ActionResult<User>> PostUsers(User user)
        {
            _context.User.Add(user);
            await _context.SaveChangesAsync();

            int b = 0;
            foreach (var people in _context.User){b++;}

            return CreatedAtAction("PostUser", new {id = b}, user);
        }

        [HttpPut("modify")]
        public async Task<IActionResult> Modify_User(int id, User profile)
        {
            if(id != profile.id){return BadRequest();}
            _context.Entry(profile).State = EntityState.Modified;
            try{await _context.SaveChangesAsync();}
            catch(DbUpdateConcurrencyException)
            {
                if(_context.User.FirstOrDefaultAsync(x => x.id == id) == null){return NotFound();}
                else{throw;}
            }

            return NoContent();
        }
    }
}