namespace Bars.Gkh.Regions.Tatarstan.Services.ServicesContracts.Fssp.CourtOrderGku
{
    using Bars.B4;

    /// <summary>
    /// Сервис загрузки/выгрузки информации об исполнительных производствах
    /// </summary>
    public interface ICourtOrderInfoImportService
    {
        /// <summary>
        /// Импорт информации об исполнительных производствах
        /// </summary>
        /// <param name="baseParams">Параметры импорта</param>
        /// <returns>Результат импорта</returns>
        IDataResult Import(BaseParams baseParams);
    }
}