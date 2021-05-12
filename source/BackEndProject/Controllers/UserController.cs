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
using Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Service;

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
                    new Claim(ClaimTypes.Role, user.levelAccess ?? "null")
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
                _userService.CreateUser(user, model.password);
                return Ok();
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize(Roles = AccessLevel.Admin + "," + AccessLevel.Employer + "," + AccessLevel.User)]
        [HttpGet]
        public IActionResult GetUser()
        {
            var user = _context.User.ToList();
            List<User> user_unbanned = new List<User>();
            foreach (var item in user)
            {
                if(item.levelAccess != AccessLevel.Ban){user_unbanned.Add(item);}
            }
            var model = _mapper.Map<IList<UserViewModel>>(user_unbanned);
            return Ok(model);
        }

        [Authorize(Roles = AccessLevel.Admin + "," + AccessLevel.Employer + "," + AccessLevel.User)]
        [HttpPut("update")]
        public IActionResult Update(int id, UpdateModel model)
        {
            //Finding who is logged in
            int logged_in_user = int.Parse(User.Identity.Name);

            // map model to entity and set id
            var user = _mapper.Map<User>(model);
            user.id = id;

            //Rejecting access if the logged in user is not same as the user updating information
            if(logged_in_user != id)
            {
                return BadRequest(new { message = "Access Denied" });
            }

            try
            {
                // update user 
                _userService.Update(user, model.CurrentPassword, model.NewPassword, model.ConfirmNewPassword);
                return Ok();
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        [AllowAnonymous]
        [HttpPost("forgotpassword")]
        public IActionResult ForgotPassword(ForgotPassword model)
        {
            return Ok(_userService.ForgotPassword(model.Username));
        }

        [Authorize(Roles = AccessLevel.Admin + "," + AccessLevel.Employer + "," + AccessLevel.User)]
        [HttpPost("email")]
        public async Task<IActionResult> SendEmail(SendEmailDTO model)
        {
            var emails = new List<string>();
            foreach (var item in model.emails)
            {
                emails.Add(item);
            }

            var response = await _emailService.SendEmailAsync(emails, model.Subject, model.Message);

            if (response.StatusCode == System.Net.HttpStatusCode.Accepted)
            {
                return Ok("Email sent " + response.StatusCode);
            }
            else
            {
                return BadRequest("Email sending failed " + response.StatusCode);
            }
        }
    }
}