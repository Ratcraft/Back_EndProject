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
    [Authorize]
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

        [Authorize(Roles = AccessLevel.Admin)]
        [HttpPut("ban")]
        public async Task<IActionResult> Ban(int id)
        {
            var user = await _context.User.ToListAsync();
            User member = user.Find(a => a.id == id);

            member.isbanned = true;
            await _context.SaveChangesAsync();

            _context.Entry(user).State = EntityState.Modified;

            return NoContent();
        } 

    }
}