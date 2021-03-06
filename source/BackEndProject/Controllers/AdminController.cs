using System.Collections.Generic;
using System.Threading.Tasks;
using Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using AutoMapper;
using System.Linq;
using DTO;
using Microsoft.AspNetCore.Authorization;

namespace Controllersa
{
    [Authorize(Roles = AccessLevel.Admin)]
    [ApiController]
    [Route("[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly Context _context;
        private IMapper _mapper;
        public AdminController(Context context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpPut("ban")]
        public async Task<IActionResult> Ban(int id)
        {
            var user = await _context.User.ToListAsync();
            User member = user.Find(a => a.id == id);
            if(member == null){return NotFound();}
            member.levelAccess = AccessLevel.Ban;
            await _context.SaveChangesAsync();

            _context.Entry(member).State = EntityState.Modified;

            return NoContent();
        } 

        [HttpPut("un_ban")]
        public async Task<IActionResult> un_Ban(int id)
        {
            var user = await _context.User.ToListAsync();
            User member = user.Find(a => a.id == id);
            if(member == null){return NotFound();}
            member.levelAccess = AccessLevel.Ban;
            await _context.SaveChangesAsync();

            _context.Entry(member).State = EntityState.Modified;

            return NoContent();
        } 

        [HttpDelete("hard_ban")]
        public async Task<ActionResult<User>> hard_Ban(int id)
        {
            var user = await _context.User.SingleOrDefaultAsync(x => x.id == id);
            if(user == null){return NotFound();}

            _context.User.Remove(user);
            await _context.SaveChangesAsync();

            return user;
        }

        [HttpGet("raw_users")]
        public async Task<ActionResult<IEnumerable<User>>> GetRawUsers()
        {
            return await _context.User.ToListAsync();
        }

    }
}