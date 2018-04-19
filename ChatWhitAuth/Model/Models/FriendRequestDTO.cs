using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;

namespace ChatWhitAuth.Models
{
    public class FriendRequestDTO
    {
        public int Id { get; set; }
        public string FromId { get; set; }
        public string ToId { get; set; }
        public string FromName { get; set; }
    }
}