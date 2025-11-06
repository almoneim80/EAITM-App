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

        public IEnumerable<T> CreateRepository()
        {
            return _serviceProvider.GetServices<T>();
        }
    }
}