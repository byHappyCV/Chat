using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChatWhitAuth.Models
{
    public class Message
    {
        public string FromName { get; set; }
        public string FromId { get; set; }
        public string ToId { get; set; }
        public string MessageText { get; set; }

    }
}