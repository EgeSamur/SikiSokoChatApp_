namespace SikiSokoChatApp.Shared.Persistence.Abstraction;

public interface IQuery<T>
{
    IQueryable<T> Query();
}