namespace Bars.GkhGji.Regions.Tatarstan.DomainService
{
    using Bars.B4;

    /// <summary>
    /// Сервис работы с MandatoryReqsNormativeDoc
    /// </summary>
    public interface IMandatoryReqsNormativeDocService
    {
        /// <summary>
        /// Добавление, обновление и удаление MandatoryReqsNormativeDoc
        /// </summary>
        IDataResult AddUpdateDeleteNpa(BaseParams baseParams);
    }
}
