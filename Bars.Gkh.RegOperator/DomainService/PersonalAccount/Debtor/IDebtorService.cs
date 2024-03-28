namespace Bars.Gkh.RegOperator.DomainService.PersonalAccount
{
    using System;
    using System.Collections;
    using System.Linq;

    using B4;

    using Bars.Gkh.RegOperator.Modules.ClaimWork.Entity;

    /// <summary>
    /// Сервис должников
    /// </summary>
    public interface IDebtorService
    {
        /// <summary>
        /// Создать должников
        /// </summary>
        /// <param name="baseParams">baseParams</param>
        IDataResult Create(BaseParams baseParams);

        /// <summary>
        /// Очистить реестр
        /// </summary>
        /// <param name="baseParams">baseParams</param>
        IDataResult Clear(BaseParams baseParams);

        /// <summary>
        /// Получить список должников
        /// </summary>
        /// <param name="baseParams">baseParams</param>
        /// <param name="paging">paging</param>
        /// <param name="totalCount">totalCount</param>
        IList GetList(BaseParams baseParams, bool paging, out int totalCount);

        /// <summary>
        /// Создание ПИР
        /// </summary>
        /// <param name="baseParams">baseParams</param>
        IDataResult CreateClaimWorks(BaseParams baseParams);

        /// <summary>
        /// Обновить судебные учереждения
        /// </summary>
        /// <param name="baseParams"> baseParams </param>
        /// <returns> IDataResult </returns>
        IDataResult UpdateJurInstitution(BaseParams baseParams);
        
        /// <summary>
        /// Детализация операций по периоду
        /// </summary>
        Bars.Gkh.DataResult.ListDataResult<DebtorPaymentsDetail> GetPaymentsOperationDetail(BaseParams baseParams);
        
        IQueryable<ViewDebtorExport> GetListQuery(BaseParams baseParams, out int totalCount);
    }
    
    public class DebtorPaymentsDetail
    {
        public long TransferId { get; set; }

        public string Name { get; set; }

        public DateTime Date { get; set; }

        public decimal Amount { get; set; }

        public string Period { get; set; }

        public string PaymentSource { get; set; }

    }
}