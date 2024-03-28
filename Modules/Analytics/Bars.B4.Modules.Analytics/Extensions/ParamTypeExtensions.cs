namespace Bars.B4.Modules.Analytics.Extensions
{
    using System;
    using Bars.B4.Modules.Analytics.Enums;

    /// <summary>
    /// 
    /// </summary>
    public static class ParamTypeExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Type ToCLRType(this ParamType type)
        {
            return type == ParamType.Bool? typeof(bool)
                : type == ParamType.Catalog? typeof(string)
                : type == ParamType.Date ? typeof(DateTime)
                : type == ParamType.Enum? typeof(Int32)
                : type == ParamType.Number? typeof(decimal)
                : typeof(string);
        }
    }
}
