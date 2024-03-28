namespace Bars.Gkh.Reforma.Interface.DataCollectors
{
    using Bars.B4;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Reforma.Entities;
    using Bars.Gkh.Reforma.ReformaService;
    using Bars.GkhDi.Entities;

    /// <summary>
    /// Сборщик данных по жилома дому
    /// </summary>
    public interface IRobject988DataCollector
    {
        /// <summary>
        /// Собирает данные по жилому дому
        /// </summary>
        /// <param name="currentProfile">Текущая анкета жилого дома 988</param>
        /// <param name="robject">Жилой дом</param>
        /// <param name="period">Период раскрытия</param>
        /// <param name="manOrgId">Id УК</param>
        /// <returns>Результат</returns>
        IDataResult<CollectHouseProfile988DataResult> CollectHouseProfile988Data(
            HouseProfileData988 currentProfile,
            RefRealityObject robject,
            PeriodDi period,
            long manOrgId);
    }

    /// <summary>
    ///     Результат сбора профиля дома (988)
    /// </summary>
    public class CollectHouseProfile988DataResult
    {
        /// <summary>
        ///     Конструктор
        /// </summary>
        /// <param name="profileData">Профиль</param>
        /// <param name="collectedFiles">Собранные данные</param>
        public CollectHouseProfile988DataResult(
            HouseProfileData988 profileData,
            ICollectedFile<HouseProfileData988>[] collectedFiles)
        {
            this.ProfileData = profileData;
            this.CollectedFiles = collectedFiles;
        }

        /// <summary>
        ///     Собранные файлы
        /// </summary>
        public ICollectedFile<HouseProfileData988>[] CollectedFiles { get; private set; }

        /// <summary>
        ///     Профиль
        /// </summary>
        public HouseProfileData988 ProfileData { get; private set; }
    }
}