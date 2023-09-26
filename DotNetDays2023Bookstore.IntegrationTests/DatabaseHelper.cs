using DotNetDays2023Bookstore.Database;

namespace DotNetDays2023Bookstore.IntegrationTests;

public class DatabaseHelper : IAsyncLifetime
{
    private readonly Func<Task> _resetDatabase;
    protected readonly BookstoreContext DbContext;

    public DatabaseHelper(IntegrationTestFactory factory)
    {
        _resetDatabase = factory.ResetDatabase;
        DbContext = factory.Db;
    }
    
    public async Task AddAsync<T>(T entity) where T : class
    {
        await DbContext.AddAsync(entity);
        await DbContext.SaveChangesAsync();
    }

    public Task InitializeAsync() => Task.CompletedTask;

    public Task DisposeAsync() => _resetDatabase();
}