namespace EAITMApp.Infrastructure.Factories
{
    public interface IRepositoryFactory<T> where T : class
    {
        IEnumerable<T> CreateRepository();
    }
}
