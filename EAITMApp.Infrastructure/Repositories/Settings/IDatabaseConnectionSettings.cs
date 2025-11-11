namespace EAITMApp.Infrastructure.Repositories.Settings
{
    public interface IDatabaseConnectionSettings
    {
        string Type { get; set; } // اسم نوع قاعدة البيانات: "Postgres", "MongoDb", "MySQL", إلخ
        Dictionary<string, object> Options { get; set; } // كل الإعدادات الخاصة بالداتا ستور
    }
}
