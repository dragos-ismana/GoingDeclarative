using System;

namespace GoingDeclarative.ResultType.Example.Domain;

public record User(int Id, string Name);
public record Location(int Id, string City);
public record Friend(int Id, string Name);


public class MissingUser : Exception { }
public class MissingFriend : Exception { }