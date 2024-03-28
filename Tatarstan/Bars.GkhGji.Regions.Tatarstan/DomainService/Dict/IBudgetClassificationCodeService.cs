namespace Bars.GkhGji.Regions.Tatarstan.DomainService.Dict
{
    using Bars.B4;

    /// <summary>
    /// Сервис для работы с КБК
    /// </summary>
    public interface IBudgetClassificationCodeService
    {
        /// <summary>
        /// Получить доп информацию о КБК
        /// </summary>
        IDataResult GetInfo(BaseParams baseParams);

        /// <summary>
        /// Сохранить МО для КБК
        /// </summary>
        IDataResult SaveMunicipalities(BaseParams baseParams);

        /// <summary>
        /// Создать или обновить КБК
        /// </summary>
        IDataResult SaveOrUpdate(BaseParams baseParams);
    }
}
