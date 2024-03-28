namespace Bars.Gkh.Modules.ClaimWork.DomainService
{
    using System.Collections;
    using System.Collections.Generic;
    using B4.Modules.Quartz;
    using B4;
    using Entities;
    using Enums;

    public interface IBaseClaimWorkService<T> : IBaseClaimWorkService where T : BaseClaimWork
    {
    }

    public interface IBaseClaimWorkService : ITask
    {
        /// <summary>
        /// Тип основания ПИР
        /// </summary>
        ClaimWorkTypeBase ClaimWorkTypeBase { get; }

        /// <summary>
        /// Метод получения списка 
        /// </summary>
        IList GetList(BaseParams baseParams, bool isPaging, ref int totalCount);

        /// <summary>
        /// Метод для обновления статусов оснований (Необходим для запуска с Quartz)
        /// </summary>
        void UpdateStatesWithNewScope();

        /// <summary>
        /// Метод для обновления статусов оснований
        /// </summary>
        IDataResult UpdateStates(BaseParams baseParams, bool inTask = false);

        /// <summary>
        /// Метод для обновления статусов оснований
        /// </summary>
        IDataResult PauseState(BaseParams baseParams);

        /// <summary>
        /// Метод для массового создания документов
        /// </summary>
        IDataResult MassCreateDocs(BaseParams baseParams);

        /// <summary>
        /// Метод для документов которые возможно формировать
        /// </summary>
        IDataResult GetDocsTypeToCreate();

        /// <summary>
        /// Метод для получения типов документов, которые необходимо создать
        /// </summary>
        IEnumerable<ClaimWorkDocumentType> GetNeedDocs(BaseParams baseParams);
    }
}