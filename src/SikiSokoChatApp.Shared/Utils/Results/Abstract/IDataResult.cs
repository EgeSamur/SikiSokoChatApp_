namespace SikiSokoChatApp.Shared.Utils.Results.Abstract;

public interface IDataResult<T> : IResult
{
    T Data { get; }
}