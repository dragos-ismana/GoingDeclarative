
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace GoingDeclarative.ResultType;

#region Async
[AsyncMethodBuilder(typeof(ResultMethodBuilder<>))]
#endregion
public abstract record Result<T>()
{
    public record Ok(T Value) : Result<T>;
    public record Error(Exception Exception) : Result<T>;
}

public static class Result
{
    public static Result<T> Ok<T>(T value)
        => new Result<T>.Ok(value);
    public static Result<T> Err<T>(Exception exception)
        => new Result<T>.Error(exception);

    public static TR Match<T, TR>(
        this Result<T> input,
        Func<T, TR> ok,
        Func<Exception, TR> err) => input switch
        {
            Result<T>.Ok { Value: var value } => ok(value),
            Result<T>.Error { Exception: var exception } => err(exception),
            _ => throw new InvalidOperationException("Cannot happen!")
        };


    public static Result<TR> Map<T, TR>(
        this Result<T> input, Func<T, TR> mapper)
    {
        return input.Match(
            ok: value => Result.Ok(mapper(value)),
            err: exception => Err<TR>(exception));
    }

    public static Result<TR> Bind<T, TR>(
        this Result<T> input, Func<T, Result<TR>> binder)
    {
        return input.Match(
            ok: value => binder(value),
            err: exception => Result.Err<TR>(exception));
    }

    public static T IfError<T>(
        this Result<T> input,
        Func<Exception, T> onError)
    {
        return input.Match(value => value, onError);
    }
}


public static class ResultExtensions
{
    public static Result<IEnumerable<T>> Traverse<T>(this IEnumerable<Result<T>> results)
    {
        var elements = new List<T>();
        foreach (var result in results)
        {
            if (result is Result<T>.Error err)
            {
                return Result.Err<IEnumerable<T>>(err.Exception);
            }
            else if (result is Result<T>.Ok ok)
            {
                elements.Add(ok.Value);
            }
        }
        return Result.Ok(elements.AsEnumerable());
    }
}
