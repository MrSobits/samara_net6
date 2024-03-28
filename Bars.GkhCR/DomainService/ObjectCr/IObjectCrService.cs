namespace Bars.GkhCr.DomainService
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.FormatDataExport.Domain;

    using Entities;

    /// <summary>
    /// Сервис для работы с <see cref="Entities.ObjectCr"/>
    /// </summary>
    public interface IObjectCrService
    {
        /// <summary>
        /// Получить отфильтрованный по Оператору список
        /// </summary>
        /// <returns></returns>
        IQueryable<ViewObjectCr> GetFilteredByOperator();

        /// <summary>
        /// Получить дома в программе
        /// </summary>
        IDataResult RealityObjectsByProgramm(BaseParams baseParams);

        /// <summary>
        /// Массовое изменение статуса
        /// </summary>
        IDataResult MassChangeState(BaseParams baseParams);

        /// <summary>
        /// Создать контракт
        /// </summary>
        IDataResult CreateContract(long objectId, long[] ids);

        /// <summary>
        /// Получить статусы
        /// </summary>
        IDataResult GetTypeStates(BaseParams baseParams);

        /// <summary>
        /// Получить тип изменения 
        /// </summary>
        IDataResult GetTypeChangeProgramCr(BaseParams baseParams);

        /// <summary>
        /// Получить подрядчиков
        /// </summary>
        IDataResult GetBuilders(BaseParams baseParams);

        /// <summary>
        /// Восстановить
        /// </summary>
        IDataResult Recover(BaseParams baseParams);

        /// <summary>
        /// Получить признак Используется добавление работ из ДПКР
        /// </summary>
        IDataResult UseAddWorkFromLongProgram(BaseParams baseParams);

        /// <summary>
        /// Получить дополнительные параметры
        /// </summary>
        IDataResult GetAdditionalParams(BaseParams baseParams);

        /// <summary>
        /// Получить список работ по Ид объекта
        /// </summary>
        IDataResult GetListCrObjectWorksByObjectId(BaseParams baseParams, bool isPaging, out int totalCount);

        /// <summary>
        /// Получить список работ по Ид программы
        /// </summary>
        IDataResult GetListDistinctWorksByProgramId(BaseParams baseParams, bool isPaging, out int totalCount);

        /// <summary>
        /// Получить список работ по программе и работам
        /// </summary>
        IDataResult GetListObjectCRByMassBuilderId(BaseParams baseParams, bool isPaging, out int totalCount);

        /// <summary>
        ///Добавить работы в предложение
        /// </summary>
        IDataResult AddOverhaulProposalWork(BaseParams baseParams);

        /// <summary>
        ///Добавить работы в предложение
        /// </summary>
        IDataResult AddMassBuilderContractWork(BaseParams baseParams);

        /// <summary>
        ///Смена статусов договоров каждого отдельного объекта в массовых договорах
        /// </summary>
        IDataResult ChangeBuildContractStateFromMassBuild(BaseParams baseParams);

        /// <summary>
        ///Добавить работы в объект КР массового договора
        /// </summary>
        IDataResult AddMassBuilderContractCRWork(BaseParams baseParams);

        /// <summary>
        /// Получить список договоров подряда
        /// </summary>
        IDataResult GetBuildContractsForMassBuild(BaseParams baseParams, bool isPaging, out int totalCount);

        /// <summary>
        ///Добавить работы в предложение
        /// </summary>
        IDataResult AddMassBuilderContractObjectCr(BaseParams baseParams);

        /// <summary>
        /// Получить подрядчиков
        /// </summary>
        IDataResult SendFKRToGZHIMail(BaseParams baseParams, Int64 taskId);

        /// Получить список без фильтрации по оператору
        /// </summary>
        IDataResult ListWithoutFilter(BaseParams baseParams);
    }
}