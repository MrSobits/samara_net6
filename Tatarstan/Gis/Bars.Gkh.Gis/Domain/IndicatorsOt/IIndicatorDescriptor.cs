namespace Bars.Gkh.Gis.Domain.IndicatorsOt
{
    /// <summary>
    /// Описатель показателя для сервиса Открытый Татарстан
    /// </summary>
    public interface IIndicatorDescriptor
    {
        /// <summary>Идентификатор показателя</summary>
        int Id { get; }

        /// <summary>Наименование</summary>
        string Title { get; }

        /// <summary>Идентификатор периодичности в системе </summary>
        TypeFrequency FrequencyId { get; }

        /// <summary>Код группы показателя</summary>
        int? GroupId { get; }

        /// <summary>Код  единицы измерения общероссийского 
        ///     классификатора единиц измерений (ОКЕИ)
        /// </summary>
        int MeasureId { get; }
    }
}
