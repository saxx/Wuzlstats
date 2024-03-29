namespace Wuzlstats.ExtensionMethods
{
    public static class StringExtensionMethods
    {
        public static bool IsNoE(this string s)
        {
            return string.IsNullOrEmpty(s);
        }
    }
}