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

        /// <summary>
        /// Indicates whether a specified string is null, empty, or consists only of white-space characters.
        /// </summary>
        /// <param name="value">The string to test.</param>
        /// <returns><value>true</value> if the value parameter is null or System.String.Empty, or if value consists exclusively of white-space characters.</returns>
        public static bool IsNullOrWhiteSpace(this string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }
    }
}
