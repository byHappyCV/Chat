using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatWhitAuth.Models;
using ChatWhitAuth.Services;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.SignalR;
using Model.Models;

namespace ChatWhitAuth.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        private static List<UserDTO> Users;
        private static List<Message> Messages = new List<Message>();
        private static IFriendManager _friendManager;
        public ChatHub(IFriendManager friendManager)
        {
            _friendManager = friendManager;

        }
        public bool IsFriends(string person1, string person2)
        {
            return _friendManager.IsFriends(person1, person2);

        }
        public bool IsUserOnline(string id)
        {
            return _friendManager.IsUserOnline(id);
        }
        public string GetUserConId(string id)
        {
            return _friendManager.GetUserConId(id);
        }

        public override async Task OnConnected()
        {
            if (Users == null)
            {
                Users = new List<UserDTO>();
            }
            Users.Add(new UserDTO
            {
                UserName = Context.User.Identity.Name,
                ConnectionId = Context.ConnectionId,
                UserId = Context.User.Identity.GetUserId(),
                Status = true
            });
            _friendManager.UserJoin(Users);

            await ShowFriends(null);
            await ShowRequests();
            Clients.All.usersOnline();

            await base.OnConnected();
        }
        public override Task OnDisconnected(bool stopCalled)
        {
            _friendManager.UserLeave(Context.ConnectionId);
            return base.OnDisconnected(stopCalled);
        }

        public async Task SendPrivate(string message, string toId)
        {
            var myid = Context.User.Identity.GetUserId();
            Messages.Add(new Message
            {
                FromName = Context.User.Identity.Name,
                FromId = myid,
                ToId = toId,
                MessageText = message
            });
            if (IsFriends(toId, myid))
            {
                if (IsUserOnline(toId))
                {
                    await Clients.Client(GetUserConId(toId))
                        .getMessage(message, Context.User.Identity.Name, myid, toId);
                }
                await Clients.Client(Context.ConnectionId).getMessage(message, Context.User.Identity.Name, myid, myid);

            }
        }

        public async Task ShowPrivateChat(string username, string toid)
        {
            var messages = Messages.Where(c => (c.FromId == Context.User.Identity.GetUserId() && c.ToId == toid) || c.FromId == toid && c.ToId == Context.User.Identity.GetUserId()).ToList();
            await Clients.Caller.showPrivateChat(toid, messages, username);
        }

        public async Task SendRequest(string toId, string userName)
        {
            if (!IsFriends(toId, Context.User.Identity.GetUserId()))
            {
                FriendRequest req = new FriendRequest
                {
                    FromId = Context.User.Identity.GetUserId(),
                    ToId = toId,

                };
                _friendManager.SendRequest(req);
                var reqId = _friendManager.GetRequestId(req.FromId, req.ToId);
                await ShowFriends(null);
                await Clients.Client(GetUserConId(toId)).sendRequest(req.FromId, req.ToId, reqId, Context.User.Identity.Name);
            }
        }

        public async Task ShowRequests()
        {
            var requests = _friendManager.GetRequests(Context.User.Identity.GetUserId());
            await Clients.Caller.showRequests(requests);
        }

        public async Task Answer(int reqId, string fromId, string toId, bool answer)
        {
            if (_friendManager.AddFriend(reqId, fromId, toId, answer))
            {
                if (IsUserOnline(fromId))
                {
                    await ShowFriends(GetUserConId(fromId));
                }
                await ShowFriends(null);
            }
            else
            {
                if (IsUserOnline(fromId))
                {
                    await ShowFriends(GetUserConId(fromId));
                }
                await ShowFriends(null);
            }

        }

        public async Task DeleteFriend(string userId)
        {
            _friendManager.DeleteFriend(userId);
            if (IsUserOnline(userId))
            {
                await ShowFriends(GetUserConId(userId));
            }
            await ShowFriends(null);
        }

        public async Task ShowFriends(string id)
        {
            if (id == null)
            {
                id = Context.ConnectionId;
            }
            var friends = _friendManager.GetFriends(id);

            await Clients.Client(id).showFriends(friends);
            try
            {
                Clients.All.usersOnline();

            }
            catch (Exception e)
            {
                var v = e.Message;
            }

        }

        public void Search(string userName)
        {
            var user = _friendManager.SearchFriend(userName, Context.User.Identity.GetUserId());
            if (user != null)
            {
                Clients.Caller.searchResults(user);
            }
            else
            {
                Clients.Caller.noResults("No result");
            }
        }
    }
}