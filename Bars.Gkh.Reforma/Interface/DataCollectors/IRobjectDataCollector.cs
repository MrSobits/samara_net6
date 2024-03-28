namespace Bars.Gkh.Reforma.Interface.DataCollectors
{
    using Bars.B4;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Reforma.ReformaService;
    using Bars.GkhDi.Entities;

    /// <summary>
    /// Сборщик данных по жилома дому
    /// </summary>
    public interface IRobjectDataCollector
    {
        /// <summary>
        /// Собирает данные по жилому дому
        /// </summary>
        /// <param name="currentProfile">Текущая анкета жилого дома</param>
        /// <param name="robject">Жилой дом</param>
        /// <param name="period">Период раскрытия</param>
        /// <returns>Результат</returns>
        IDataResult<HouseProfileData> CollectHouseProfileData(HouseProfileData currentProfile, RealityObject robject, PeriodDi period);
    }
}