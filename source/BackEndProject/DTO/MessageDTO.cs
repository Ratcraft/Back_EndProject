using System;
using System.ComponentModel.DataAnnotations;

namespace DTO
{
    public class MessageDTO
    {
        [Required]
        string emailReceiver { get; set; }

        [Required]
        string subject { get; set; }

        [Required]
        string message { get; set; }
    }
}