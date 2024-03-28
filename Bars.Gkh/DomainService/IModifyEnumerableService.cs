namespace Bars.Gkh.DomainService
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    /// <summary>
    /// Сервис модификации коллекции <see cref="IEnumerable{T}"/>
    /// </summary>
    public interface IModifyEnumerableService
    {
        /// <summary>
        /// Замена подстроки в строковом поле
        /// </summary>
        /// <param name="collection"><see cref="IEnumerable{T}"/> коллекция</param>
        /// <param name="oldValue">Искомая подстрока</param>
        /// <param name="newValue">Заменяемая подстрока</param>
        /// <param name="propertySelector">Модифицируемые поля</param>
        IEnumerable<T> ReplaceProperty<T>(IEnumerable<T> collection, string oldValue, string newValue, params Expression<Func<T, string>>[] propertySelector)
            where T : new();

        /// <summary>
        /// Удалить подстроку в строковом поле
        /// </summary>
        /// <param name="collection"><see cref="IEnumerable{T}"/> коллекция</param>
        /// <param name="strValue">Искомая подстрока</param>
        /// <param name="propertySelector">Модифицируемые поля</param>
        IEnumerable<T> ReplaceProperty<T>(IEnumerable<T> collection, string strValue, params Expression<Func<T, string>>[] propertySelector)
            where T : new();
    }
}