namespace Bars.GisIntegration.Base.DataExtractors
{
    using System.Collections.Generic;
    using Bars.B4.Utils;

    /// <summary>
    /// Интерфейс компонента для извленения данных из сторонней системы
    /// </summary>
    /// <typeparam name="T">Тип данных сторонней системы</typeparam>
    public interface IDataSelector<TExternalEntity>
        where TExternalEntity : class
    {
        /// <summary>
        /// Получить сущности сторонней системы
        /// </summary>
        /// <param name="parameters">Параметры сбора данных</param>
        /// <returns>Сущности сторонней системы</returns>
        List<TExternalEntity> GetExternalEntities(DynamicDictionary parameters);
    }
}
