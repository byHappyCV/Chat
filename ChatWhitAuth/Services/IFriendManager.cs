using System.Collections.Generic;
using ChatWhitAuth.Models;

namespace ChatWhitAuth.Services
{
    public interface IFriendManager
    {
        void UserJoin(List<UserDTO> users);
        void UserLeave(string conId);
        string GetUserNamebyId(string id);
        bool IsFriends(string person1, string person2);
        bool IsUserOnline(string id);
        string GetUserConId(string id);
        List<UserDTO> GetFriends(string id);
        void SendRequest(FriendRequest req);
        int GetRequestId(string fromId, string toId);
        List<FriendRequestDTO> GetRequests(string id);
        bool AddFriend(int reqId, string fromId, string toId, bool answer);
        void DeleteFriend(string userId);
        UserDTO SearchFriend(string friendName, string userId);
        void Dispose();
    }
}