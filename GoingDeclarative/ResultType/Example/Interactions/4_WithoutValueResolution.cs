
using GoingDeclarative;
using GoingDeclarative.ResultType.Example.Domain;

using System.Collections.Generic;
using System.Linq;

namespace GoingDeclarative.ResultType.Example.Interactions;

public class WithoutValueResolution
{
    public static Result<string> GetUserName(int userId)
    {
        Result<User> userResult = Repository.GetUser(userId);

        return userResult.Map(user => user.Name);
    }

    public static Result<List<string>> GetFriendsUserNames(int userId)
    {
        Result<User> userResult = Repository.GetUser(userId);

        // we don't want this.
        Result<Result<List<Friend>>> mappedValue = userResult
            .Map(user => Repository.GetUserFriends(user));

        Result<List<Friend>> userFriends = userResult
            .Bind(user => Repository.GetUserFriends(user));

        return userFriends
            .Map(friends => friends.Select(friend => friend.Name).ToList());
    }

    public static Result<bool> MarkAsFriends(int userId, int otherUserId)
    {
        return Repository.GetUser(userId).Bind(
            user => Repository.GetUser(otherUserId).Bind(
                otherUser => Repository.MarkAsFriends(user, otherUser)));
    }

    public static Result<bool> Mark3AsFriends(
        int userId, int secondUserId, int thirdUserId)
    {
        return Repository.GetUser(userId).Bind(
            user => Repository.GetUser(secondUserId).Bind(
                second => Repository.GetUser(thirdUserId).Bind(
                    third => Repository.MarkAsFriends(user, second, third))));
    }
}
