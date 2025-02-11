namespace SikiSokoChatApp.Application.Abstractions.Repositories;

public interface IUnitOfWork
{
    //ICityRepository CityRepository { get; }
    
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
    void Rollback();
}