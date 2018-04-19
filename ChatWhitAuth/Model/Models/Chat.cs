using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChatWhitAuth.Models
{
    public class Chat
    {
        public List<Message> Messages { get; set; }
        public string FirstUserName { get; set; }
        public string SecondUserName { get; set; }
    }
}