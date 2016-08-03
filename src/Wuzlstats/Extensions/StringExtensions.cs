namespace Wuzlstats.Extensions
{
    /// <summary>
    /// Provides extension methods for an <see cref="string"/> type.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Indicates whether the specified string is null or an <see cref="string.Empty"/> string.
        /// </summary>
        /// <param name="s">The string to test.</param>
        /// <returns>Returns true if string is null or empty, else false</returns>
        public static bool IsNullOrEmpty(this string s)
        {
            return string.IsNullOrEmpty(s);
        }
    }
}
