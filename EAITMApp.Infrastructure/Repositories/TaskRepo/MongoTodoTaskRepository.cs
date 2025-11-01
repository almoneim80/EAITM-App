using EAITMApp.Application.Interfaces;
using EAITMApp.Domain.Entities;
using EAITMApp.Infrastructure.Configurations;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace EAITMApp.Infrastructure.Repositories.TaskRepo
{
    public class MongoTodoTaskRepository : ITodoTaskRepository
    {
        private readonly IMongoCollection<TodoTask> _collection;
        public MongoTodoTaskRepository(MongoDbSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _collection = database.GetCollection<TodoTask>(settings.CollectionName);
        }

        /// <inheritdoc/>
        public async Task<TodoTask> AddAsync(TodoTask task)
        {
            Console.WriteLine("<------------- Add Task To Mongo -------------->");

            await _collection.InsertOneAsync(task);
            return task;
        }

        /// <inheritdoc/>
        public async Task<TodoTask?> GetByIdAsync(Guid id)
        {
            var filter = Builders<TodoTask>.Filter.Eq(t => t.Id, id);
            return await _collection.Find(filter).FirstOrDefaultAsync();
        }

        /// <inheritdoc/>
        public async Task<List<TodoTask>> GetAllAsync()
        {
            var filter = Builders<TodoTask>.Filter.Empty;
            return await _collection.Find(filter).ToListAsync();
        }

        /// <inheritdoc/>
        public async Task UpdateAsync(TodoTask task)
        {
            var filter = Builders<TodoTask>.Filter.Eq(t => t.Id, task.Id);
            await _collection.ReplaceOneAsync(filter, task);
        }

        /// <inheritdoc/>
        public async Task DeleteAsync(Guid id)
        {
            var filter = Builders<TodoTask>.Filter.Eq(t => t.Id, id);
            await _collection.DeleteOneAsync(filter);
        }
    }
}
