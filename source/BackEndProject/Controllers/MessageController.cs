using System.Collections.Generic;
using System.Threading.Tasks;
using Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using AutoMapper;
using System.Linq;
using DTO;

namespace Controllers
{
    [Authorize("User,Employer,Admin")]
    [ApiController]
    [Route("[controller]")]
    public class MessageController : ControllerBase
    {
        private readonly Context _context;
        private IMapper _mapper;
        public MessageController(Context context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetMessage()
        {
            var msg = _context.Message.ToList();
            var model = _mapper.Map<IList<MessageDTO>>(msg);
            return Ok(model);
        }

        [HttpPost("send_message")]
        public async Task<ActionResult<Message>> PostMessage(Message msg)
        {
            _context.Message.Add(msg);
            await _context.SaveChangesAsync();

            int b = 0;
            foreach (var text in _context.Message){b++;}

            return CreatedAtAction("PostMessage", new {id = b}, msg);
        }

        [HttpGet("my_messages")]
        public IActionResult Get_myMessage(string mail){
            var msg = _context.Message.ToList();
            List<Message> bonjour = new List<Message>();
            foreach (var item in msg)
            {
                if(item.mailSender == mail || item.mailReceiver == mail){bonjour.Add(item);}
            }
            var model = _mapper.Map<IList<MessageDTO>>(bonjour);
            return Ok(model);
        }

        [HttpGet("my_sent_messages")]
        public IActionResult Get_sentMessage(string mail){
            var msg = _context.Message.ToList();
            List<Message> bonjour = new List<Message>();
            foreach (var item in msg)
            {
                if(item.mailSender == mail){bonjour.Add(item);}
            }
            var model = _mapper.Map<IList<MessageDTO>>(bonjour);
            return Ok(model);
        }

        [HttpGet("my_received_message")]
        public IActionResult Get_receivedMessage(string mail){
            var msg = _context.Message.ToList();
            List<Message> bonjour = new List<Message>();
            foreach (var item in msg)
            {
                if(item.mailReceiver == mail){bonjour.Add(item);}
            }
            var model = _mapper.Map<IList<MessageDTO>>(bonjour);
            return Ok(model);
        }
    }
}