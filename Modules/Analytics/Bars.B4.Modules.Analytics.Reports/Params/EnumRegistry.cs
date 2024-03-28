namespace Bars.B4.Modules.Analytics.Reports.Params
{
    using System.Collections.Generic;

    public static class EnumRegistry
    {
        private static readonly Dictionary<string, Enum> Enums = new Dictionary<string, Enum>();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="enum"></param>
        public static void Add(Enum @enum)
        {
            Enums.Add(@enum.Id.ToLower(), @enum);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Enum Get(string id)
        {
            return Enums[id.ToLower()];
        }

        public static IEnumerable<Enum> GetAll()
        {
            return Enums.Values;
        }
    }
}
