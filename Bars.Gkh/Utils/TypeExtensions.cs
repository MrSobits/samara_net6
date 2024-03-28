namespace Bars.Gkh.Utils
{
    using System;

    public static class TypeExtensions
    {
        /// <summary>
        /// Получение дефолтного экземпляра для типа.
        /// Аналог default(Тип) короче
        /// </summary>
        /// <param name="type">Тип</param>
        /// <returns>Дефолтный экземпляр типа</returns>
        public static object GetDefaultValue(this Type type)
        {
            return type.IsValueType ? Activator.CreateInstance(type) : null;
        }
    }
}