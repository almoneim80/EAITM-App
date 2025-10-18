namespace EAITMApp.Infrastructure.Memory
{
    public class SecureMemoryService : ISecureMemoryService
    {
        /// <inheritdoc/>
        public void ClearBytes(byte[] data)
        {
            if (data == null) return;
            Array.Clear(data, 0, data.Length);
        }

        /// <inheritdoc/>
        public void ClearChar(char[] data)
        {
            if (data == null) return;
            for (int i = 0; i < data.Length; i++)
                data[i] = '\0';
        }
    }
}
