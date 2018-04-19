using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ChatWhitAuth.Interfaces;
using ChatWhitAuth.Models;
using ChatWhitAuth.Repositories;
using Microsoft.AspNet.SignalR.Hubs;

namespace ChatWhitAuth
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext context;

        private IBaseRepository<FriendRequest> requestsRepo;
        private IBaseRepository<ApplicationUser> usersRepo;
        private IBaseRepository<Friendship> friendsRepo;

        public UnitOfWork(ApplicationDbContext context)
        {
            this.context = context;
        }

        public IBaseRepository<ApplicationUser> UsersRepo
        {
            get
            {
                if (usersRepo == null) { usersRepo = new BaseRepository<ApplicationUser>(context); }
                return usersRepo;
            }
        }

        public IBaseRepository<FriendRequest> RequestsRepo
        {
            get
            {
                if (requestsRepo == null) { requestsRepo = new BaseRepository<FriendRequest>(context); }
                return requestsRepo;
            }
        }

        public IBaseRepository<Friendship> FriendsRepo
        {
            get
            {
                if (friendsRepo == null) { friendsRepo = new BaseRepository<Friendship>(context); }
                return friendsRepo;
            }
        }

        public int Save()
        {
            return context.SaveChanges();
        }

        private bool isDisposed = false;

        protected virtual void Grind(bool disposing)
        {
            if (!isDisposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            isDisposed = true;
        }

        public void Dispose()
        {
            Grind(true);
            GC.SuppressFinalize(this);
        }
    }
}