namespace Bars.Gkh.Overhaul.Nso.DomainService
{
    using System.Linq;
    using Bars.Gkh.Overhaul.Nso.Entities;
    using Bars.B4;

    public interface IPublishProgramService
    {
        /// <summary>
        /// Проверить можно ли создать новую опубликованную программу
        /// </summary>
        IDataResult GetValidationForCreatePublishProgram(BaseParams baseParams);

        /// <summary>
        /// Проверить можно ли подписывать
        /// </summary>
        IDataResult GetValidationForSignEcp(BaseParams baseParams);

        /// <summary>
        /// Поулчение данных для подписания ЭЦП
        /// </summary>
        IDataResult GetDataToSignEcp(BaseParams baseParams);

        /// <summary>
        /// Сохранение результатов подписи 
        /// </summary>
        IDataResult SaveSignedResult(BaseParams baseParams);

        /// <summary>
        /// Получение опубликованной программы
        /// </summary>
        IDataResult GetPublishedProgram(BaseParams baseParams);

        /// <summary>
        /// Источник данных для подписания ЭЦП
        /// </summary>
        IQueryable<PublishedProgramRecord> GetPublishedProgramRecords(Entities.PublishedProgram program);

        /// <summary>
        /// Удаление опубликованной программы
        /// </summary>
        IDataResult DeletePublishedProgram(BaseParams baseParams);
    }
}