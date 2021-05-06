using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Message
    {
        [Key]
        public int id { get; set; }
        public string mailSender { get; set; }
        public string mailReceiver { get; set; }
        public string subject { get; set; }
        public string message { get; set; }
    }
}