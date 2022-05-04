using GoingDeclarative;
using GoingDeclarative.ResultType.Example.Domain;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoingDeclarative.ResultType.Example.Interactions;

public class AsyncInteractions
{
    public static async Result<string> GetUserName(int userId)
    {
        User user = await Repository.GetUser(userId);
        return user.Name;
    }

    public static async Result<List<string>> GetFriendsUserNames(int userId)
    {
        User user = await Repository.GetUser(userId);
        List<Friend> friends = await Repository.GetUserFriends(user);
        return friends.Select(friend => friend.Name).ToList();
    }

    public static async Result<bool> MarkAsFriends(int userId, int otherUserId)
    {
        User user = await Repository.GetUser(userId);
        User other = await Repository.GetUser(otherUserId);
        bool response = await Repository.MarkAsFriends(user, other);
        return response;

    }

    public static async Result<bool> Mark3AsFriends(int userId, int secondUserId, int thirdUserId)
    {
        User user = await Repository.GetUser(userId);
        User secondUser = await Repository.GetUser(secondUserId);
        User thirdUser = await Repository.GetUser(thirdUserId);
        bool response = await Repository.MarkAsFriends(user, secondUser, thirdUser);
        return response;
    }

    // Interop between Task async  and Result Async
    public static async Task<Result<List<string>>> GetFriendsUserNamesTask(int userId)
    {
        var valueFromTask = await Task.FromResult(userId);

        User user = await Repository.GetUser(userId);
        List<Friend> friends = await Repository.GetUserFriends(user);

        return Result.Ok(friends.Select(friend => friend.Name).ToList());
    }
}