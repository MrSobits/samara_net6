namespace Bars.B4.Modules.Analytics.Filters
{
    using System;

    /// <summary>
    /// 
    /// </summary>
    public interface IMacros
    {
        /// <summary>
        /// Ключ макроса, для использования в выражениях. Разрешены только буквы.
        /// </summary>
        string Key { get; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Type GetValueType();
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        object GetValue();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class Macros<T> : IMacros
    {
        public abstract string Key { get; }
        public Type GetValueType()
        {
            return typeof(T);
        }

        public abstract object GetValue();
    }
}
