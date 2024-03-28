namespace Bars.Gkh.Reforma.Interface.DataCollectors
{
    using Bars.B4;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Reforma.ReformaService;
    using Bars.GkhDi.Entities;

    /// <summary>
    /// Сервис сбора информации для отправки в Реформу (988)
    /// </summary>
    public interface IManOrg988DataCollector
    {
        /// <summary>
        /// Сбор данных по УО для выгрузки изменений
        /// </summary>
        /// <param name="currentProfile">Текущий профиль УО на Реформе</param>
        /// <param name="organization">УО</param>
        /// <param name="period">Период раскрытия</param>
        /// <returns></returns>
        IDataResult<CollectCompanyProfile988DataResult> CollectCompanyProfile988Data(
             CompanyProfileData988 currentProfile,
            ManagingOrganization organization,
            PeriodDi period);
    }

    /// <summary>
    ///     Результат сбора профиля УО (988)
    /// </summary>
    public class CollectCompanyProfile988DataResult
    {
        /// <summary>
        ///     Конструктор
        /// </summary>
        /// <param name="profileData">Профиль</param>
        /// <param name="collectedFiles">Собранные данные</param>
        public CollectCompanyProfile988DataResult(
            CompanyProfileData988 profileData,
            ICollectedFile<CompanyProfileData988>[] collectedFiles)
        {
            this.ProfileData = profileData;
            this.CollectedFiles = collectedFiles;
        }

        /// <summary>
        ///     Собранные файлы
        /// </summary>
        public ICollectedFile<CompanyProfileData988>[] CollectedFiles { get; private set; }

        /// <summary>
        ///     Профиль
        /// </summary>
        public CompanyProfileData988 ProfileData { get; private set; }
    }
}