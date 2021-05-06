using System;
using System.ComponentModel.DataAnnotations;

namespace DTO
{
    public class MessageDTO
    {
        [Required]
        public string mailSender { get; set; }

        [Required]
        public string mailReceiver { get; set; }

        [Required]
        public string subject { get; set; }

        [Required]
        public string message { get; set; }
    }
}