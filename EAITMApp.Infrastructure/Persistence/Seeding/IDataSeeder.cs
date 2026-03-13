namespace EAITMApp.Infrastructure.Persistence.Seeding
{
    /// <summary>
    /// Defines a contract for database seeding logic.
    /// Implementations of this interface are responsible for populating the database with initial, 
    /// mandatory, or lookup data in an idempotent manner.
    /// </summary>
    public interface IDataSeeder
    {
        /// <summary>
        /// Gets the execution priority of the seeder. 
        /// Seeders with lower values are executed first. 
        /// Useful for maintaining referential integrity between dependent entities.
        /// </summary>
        int Order => 0;

        /// <summary>
        /// Executes the seeding logic.
        /// Implementation should ensure that data is not duplicated upon multiple executions.
        /// </summary>
        Task SeedAsync(CancellationToken cancellationToken = default);
    }
}
