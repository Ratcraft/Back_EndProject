using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Message
    {
        [Key]
        int id { get; set; }

        [Required]
        int idSender { get; set; }

        [Required]

        int idReceiver { get; set; }

        [Required]
        string description { get; set; }

        [Required]
        string message { get; set; }
    }
}