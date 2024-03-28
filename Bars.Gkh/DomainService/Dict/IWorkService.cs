namespace Bars.Gkh.DomainService
{
    using Bars.B4;
    using Bars.Gkh.Entities.Dicts;

    public interface IWorkService
    {
        IDataResult ListWorksRealityObjectByPeriod(BaseParams baseParams);

        IDataResult ListWithoutPaging(BaseParams baseParams);

        /// <summary>
        /// Добавить связанные записи справочника "Работы по содержанию и ремонту МКД"
        /// </summary>
        /// <param name="baseParams">Параметры</param>
        /// <param name="domainService">Доменный сервис сущности Work</param>
        /// <returns>Результат выполнения операции</returns>
        IDataResult AddContentRepairMkdWorks(BaseParams baseParams, IDomainService<Work> domainService);

        /// <summary>
        /// Удалить связанную запись справочника "Работы по содержанию и ремонту МКД"
        /// </summary>
        /// <param name="baseParams">Параметры</param>
        /// <param name="domainService">Доменный сервис сущности Work</param>
        /// <returns>Результат выполнения операции</returns>
        IDataResult DeleteContentRepairMkdWork(BaseParams baseParams, IDomainService<Work> domainService);
    }
}