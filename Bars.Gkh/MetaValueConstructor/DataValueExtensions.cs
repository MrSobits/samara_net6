namespace Bars.Gkh.MetaValueConstructor
{
    using Bars.B4.Utils;
    using Bars.Gkh.MetaValueConstructor.DomainModel;

    /// <summary>
    /// Расширения для <see cref="IDataValue"/>
    /// </summary>
    public static class DataValueExtensions
    {
        /// <summary>
        /// Вернуть значение приведенное к типу
        /// </summary>
        /// <param name="element">Объект</param>
        /// <param name="defaultValue">Значение по умолчанию</param>
        /// <typeparam name="T">Тип</typeparam>
        /// <returns>Значение атрибута</returns>
        public static T GetValueAs<T>(this IDataValue element, T defaultValue = default(T))
        {
            return element.Value.To(defaultValue);
        }
    }
}