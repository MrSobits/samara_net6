namespace Bars.Gkh.MetaValueConstructor.DomainModel
{
    using Bars.Gkh.MetaValueConstructor.Enums;

    /// <summary>
    /// Интерфейс мета-информации
    /// </summary>
    public interface IDataMetaInfo : IHasNameCode, IHasId
    {
        /// <summary>
        /// Вес объекта
        /// </summary>
        decimal? Weight { get; }

        /// <summary>
        /// Формула расчета
        /// </summary>
        string Formula { get; }

        /// <summary>
        /// Уровень объекта
        /// </summary>
        int Level { get; }

        /// <summary>
        /// Тип значения
        /// </summary>
        DataValueType DataValueType { get; }

        /// <summary>
        /// Минимальная длина (для строки)
        /// </summary>
        int? MinLength { get; }

        /// <summary>
        /// Максимальная длина (для строки)
        /// </summary>
        int? MaxLength { get; }

        /// <summary>
        /// Знаков после запятой
        /// </summary>
        int? Decimals { get; }

        /// <summary>
        /// Обязательный
        /// </summary>
        bool Required { get; }

        /// <summary>
        /// Источник данных
        /// <para>Если указан, значит данные будут не заполняться, а тянуться из системы</para>
        /// </summary>
        string DataFillerName { get; set; }
    }
}