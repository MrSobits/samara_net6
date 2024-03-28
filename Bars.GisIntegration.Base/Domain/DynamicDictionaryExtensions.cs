namespace Bars.GisIntegration.Base.Domain
{
    using Bars.B4.Utils;

    public static class DynamicDictionaryExtensions
    {
        public static long GetAsId(this DynamicDictionary dictionary, string key = "id", bool ignorecase = true)
        {
            if (key.IsEmpty())
            {
                return 0;
            }

            return dictionary.GetAs<long>(key, ignoreCase: ignorecase);
        }
    }
}