using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatWhitAuth.Interfaces;
using ChatWhitAuth.Models;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ChatWhitAuth.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IBaseRepository<ApplicationUser> UsersRepo { get; }
        IBaseRepository<FriendRequest> RequestsRepo { get; }
        IBaseRepository<Friendship> FriendsRepo { get; }
        int Save();
    }
}
