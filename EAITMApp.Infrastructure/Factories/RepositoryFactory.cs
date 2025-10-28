using Microsoft.Extensions.DependencyInjection;

namespace EAITMApp.Infrastructure.Factories
{
    public class RepositoryFactory<T> : IRepositoryFactory<T> where T : class
    {
        private readonly IServiceProvider _serviceProvider;
        public RepositoryFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public T CreateRepository()
        {
            var repo = _serviceProvider.GetRequiredService<T>();
            return repo;
        }
    }
}
