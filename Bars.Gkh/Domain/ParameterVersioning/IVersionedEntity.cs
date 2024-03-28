namespace Bars.Gkh.Domain.ParameterVersioning
{
    using System;
    using System.Collections.ObjectModel;
    using B4;

    /// <summary>
    /// Интерфейс для получения маппинга параметра для вычисления Начисления/Перерасчета... на сущность
    /// </summary>
    public interface IVersionedEntity
    {
        /// <summary>
        /// Получить маппинг параметра
        /// </summary>
        ReadOnlyCollection<ParameterEntity> GetParameterMap();

        /// <summary>
        /// Валидация изменения параметра.
        /// Иногда требуется провалидировать изменение параметра до обновления сущности (и соответственно валидации сущности)
        /// </summary>
        /// <param name="entity">Объект, чье свойство обновляем</param>
        /// <param name="value">Значение свойства</param>
        /// <param name="factDate">Дата фактической смены параметра</param>
        /// <param name="parameterName">Имя параметра</param>
        IDataResult Validate(object entity, object value, DateTime factDate, string parameterName);

        /// <summary>
        /// Пропустить поле для версионирования
        /// </summary>
        /// <param name="entity">Объект, чье свойство пропускаем</param>
        /// <param name="parameterName">Имя параметра</param>
        /// <returns></returns>
        bool SkipProperty(object entity, string parameterName);
    }
}