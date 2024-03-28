namespace Bars.GkhGji.Regions.Chelyabinsk.DomainService
{
    using System;

    using Bars.B4;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.Chelyabinsk.Entities;

    /// <summary>
    /// Клиент сервиса <see cref="EaisCitizensAppealService"/>
    /// </summary>
    public interface ICitizensAppealServiceClient : IDisposable
    {
        /// <summary>
        /// Экспорт сведений о приеме в работу обращения
        /// </summary>
        IDataResult ExportInfoAcceptWorkResult(AppealCits appealCits, bool isAccept = true);

        /// <summary>
        /// Экспорт сведений о завершении работы по обращению
        /// </summary>
        /// <returns><see cref="EaisCitizensAppealService.ImportInfoDataTransferResult"/></returns>
        IDataResult ExportInfoCompletionOfWorkResult(AppealCits appealCits, bool isCompletion = true);

        /// <summary>
        /// Экспорт сведений об отмене обращения
        /// </summary>
        IDataResult ExportInfoCitizensAppealCancelResult(AppealCits appealCits, bool isCancel = true);

        /// <summary>
        /// Перезапустить отправку данных по обращению
        /// </summary>
        /// <param name="appealCitsTransferResultId">
        /// Идентификатор <see cref="AppealCitsTransferResult"/>
        /// </param>
        IDataResult RestartAppealCitsTransfer(long appealCitsTransferResultId);
    }
}