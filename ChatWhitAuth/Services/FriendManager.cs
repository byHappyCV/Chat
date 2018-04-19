using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ChatWhitAuth.Interfaces;
using ChatWhitAuth.Models;

namespace ChatWhitAuth.Services
{
    public class FriendManager : IFriendManager
    {
        private static List<UserDTO> Users;
        private readonly IUnitOfWork _unitOfWork;
        public FriendManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void UserJoin(List<UserDTO> users)
        {
            Users = users;
        }
        public void UserLeave(string conId)
        {
            Users.Remove(Users.Find(c => c.ConnectionId == conId));
        }

        public string GetUserNamebyId(string id)
        {
            return _unitOfWork.UsersRepo.GetAll().FirstOrDefault(c => c.Id == id)?.UserName;
        }
        public bool IsFriends(string person1, string person2)
        {
            var fr = _unitOfWork.FriendsRepo.GetAll().ToList().Where(c => c.UserId == person1);
            var check = fr.Any(c => c.FriendId == person2);
            return check;
        }
        public bool IsUserOnline(string id)
        {
            return Users.Any(c => c.UserId == id);
        }

        public string GetUserConId(string id)
        {
            var s = Users.FirstOrDefault(c => c.UserId == id);
            if (s != null)
                return s.ConnectionId;
            return "";
        }
        public List<UserDTO> GetFriends(string id)
        {
            var userid = Users.Find(c => c.ConnectionId == id).UserId;
            var list = _unitOfWork.FriendsRepo.GetAll().Where(c => c.UserId == userid).ToList();
            List<UserDTO> friends = new List<UserDTO>();
            foreach (var v in list)
            {
                friends.Add(new UserDTO
                {
                    UserId = v.FriendId,
                    UserName = _unitOfWork.UsersRepo.Get(c => c.Id == v.FriendId).First().UserName,
                    Status = IsUserOnline(v.FriendId),
                });
            }
            return friends;
        }

        public void SendRequest(FriendRequest req)
        {
            _unitOfWork.RequestsRepo.Insert(req);
            _unitOfWork.Save();
        }

        public int GetRequestId(string fromId, string toId)
        {
            return _unitOfWork.RequestsRepo.GetAll().ToList()
                .Find(c => c.FromId == fromId && c.ToId == toId).Id;
        }

        public List<FriendRequestDTO> GetRequests(string id)
        {
            var list = _unitOfWork.RequestsRepo.GetAll().ToList().Where(c => c.ToId == id);
            List<FriendRequestDTO> requests = new List<FriendRequestDTO>();
            foreach (var v in list)
            {
                requests.Add(new FriendRequestDTO
                {
                    ToId = v.ToId,
                    FromId = v.FromId,
                    Id = v.Id,
                    FromName = _unitOfWork.UsersRepo.GetAll().First(c => c.Id == v.FromId).UserName
                });
            }
            return requests;
        }

        public bool AddFriend(int reqId, string fromId, string toId, bool answer)
        {
            if (answer)
            {
                Friendship request = new Friendship
                {
                    UserId = toId,
                    FriendId = fromId
                };
                Friendship reques1 = new Friendship
                {
                    UserId = fromId,
                    FriendId = toId
                };
                _unitOfWork.FriendsRepo.Insert(request);
                _unitOfWork.FriendsRepo.Insert(reques1);
                var delreq = _unitOfWork.RequestsRepo.GetById(reqId);
                _unitOfWork.RequestsRepo.Delete(delreq);
                _unitOfWork.Save();
                return true;
            }
            else
            {
                var delreq = _unitOfWork.RequestsRepo.GetById(reqId);
                _unitOfWork.RequestsRepo.Delete(delreq);
                _unitOfWork.Save();
                return false;
            }
        }

        public void DeleteFriend(string userId)
        {
            var friend = _unitOfWork.FriendsRepo.GetAll().ToList().Find(c => c.UserId == userId);
            var friend1 = _unitOfWork.FriendsRepo.GetAll().ToList().Find(c => c.FriendId == userId);
            _unitOfWork.FriendsRepo.Delete(friend);
            _unitOfWork.FriendsRepo.Delete(friend1);
            _unitOfWork.Save();
        }

        public UserDTO SearchFriend(string friendName, string userId)
        {
            var friend = _unitOfWork.UsersRepo.GetAll().ToList().Find(c => c.UserName == friendName);
            if (friend != null)
            {
                if (!IsFriends(friend.Id, userId))
                {
                    return new UserDTO
                    {
                        UserId = friend.Id,
                        UserName = friend.UserName
                    };
                }
            }
            return null;
        }

        private bool isDisposed = false;

        protected virtual void Grind(bool disposing)
        {
            if (!isDisposed)
            {
                if (disposing)
                {
                    _unitOfWork.Dispose();
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