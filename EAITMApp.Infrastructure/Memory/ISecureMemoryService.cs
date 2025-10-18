namespace EAITMApp.Infrastructure.Memory
{
    /// <summary>
    /// Defines methods to securely clear sensitive data from memory.
    /// </summary>
    public interface ISecureMemoryService
    {
        /// <summary>
        /// Clears the contents of a byte array from memory.
        /// </summary>
        /// <param name="data">The byte array containing sensitive information.</param>
        void ClearBytes(byte[] data);

        /// <summary>
        /// Clears the contents of a char array from memory.
        /// </summary>
        /// <param name="data">The char array containing sensitive information.</param>
        void ClearChar(char[] data);
    }
}
