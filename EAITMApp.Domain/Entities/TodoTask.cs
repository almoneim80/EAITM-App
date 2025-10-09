namespace EAITMApp.Domain.Entities
{
    public class TodoTask
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public string Title { get; private set; }
        public string Description { get; private set; }
        public bool IsCompleted { get; private set; }
        public DateTime CreatedAt { get; private set; }

        public TodoTask(string title, string description)
        {
            if(string.IsNullOrEmpty(title))
                throw new ArgumentNullException("Title can not be null");

            Title = title;
            Description = description ?? string.Empty;
            IsCompleted = false;
            CreatedAt = DateTime.UtcNow;
        }

        public void ToggleTaskCompleted()
        {
            IsCompleted = !IsCompleted;
        }

        public void UpdateTitle(string title)
        {
            if (string.IsNullOrEmpty(title))
                throw new ArgumentNullException("Title can not be null");

            Title = title;
        }

        public void UpdateDescription(string description)
        {
            Description = description ?? string.Empty;
        }

    }
}
