namespace Bars.Gkh.Reforma.Interface.DataCollectors
{
    using Bars.B4;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Reforma.ReformaService;
    using Bars.GkhDi.Entities;

    /// <summary>
    /// Сервис сбора информации для отправки в Реформу
    /// </summary>
    public interface IManOrgDataCollector
    {
        /// <summary>
        /// Сбор данных по УО для создания новой компании
        /// </summary>
        /// <param name="entity">
        /// УО
        /// </param>
        /// <param name="period">Период</param>
        /// <returns>Результат выполнения</returns>
        IDataResult<NewCompanyProfileData> CollectNewCompanyProfileData(ManagingOrganization entity, PeriodDi period);

        /// <summary>
        /// Сбор данных по УО для выгрузки изменений
        /// </summary>
        /// <param name="currentProfile">Текущая анкета УО</param>
        /// <param name="organization">УО</param>
        /// <param name="period">Период раскрытия</param>
        /// <returns>Результат выполнения</returns>
        IDataResult<CollectCompanyProfileDataResult> CollectCompanyProfileData(CompanyProfileData currentProfile, ManagingOrganization organization, PeriodDi period);
    }

    /// <summary>
    ///     Результат сбора профиля УО (988)
    /// </summary>
    public class CollectCompanyProfileDataResult
    {
        /// <summary>
        ///     Конструктор
        /// </summary>
        /// <param name="profileData">Профиль</param>
        /// <param name="collectedFiles">Собранные данные</param>
        public CollectCompanyProfileDataResult(
            CompanyProfileData profileData,
            ICollectedFile<CompanyProfileData>[] collectedFiles)
        {
            this.ProfileData = profileData;
            this.CollectedFiles = collectedFiles;
        }

        /// <summary>
        ///     Собранные файлы
        /// </summary>
        public ICollectedFile<CompanyProfileData>[] CollectedFiles { get; private set; }

        /// <summary>
        ///     Профиль
        /// </summary>
        public CompanyProfileData ProfileData { get; private set; }
    }
}