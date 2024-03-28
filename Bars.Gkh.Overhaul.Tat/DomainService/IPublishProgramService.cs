namespace Bars.Gkh.Overhaul.Tat.DomainService
{
    using System.Linq;
    using Bars.B4;
    using Bars.Gkh.Overhaul.Tat.Entities;

    public interface IPublishProgramService
    {
        /// <summary>
        /// Проверить можно ли создать новую опубликованную программу
        /// </summary>
        IDataResult GetValidationForCreatePublishProgram(BaseParams baseParams);

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

        /// <summary>
        /// Список муниципальных образований по которым есть записи в PublishedProgramRecord
        /// </summary>
        IDataResult PublishedProgramMunicipalityList(BaseParams baseParams);
    }
}