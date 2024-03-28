namespace Bars.Gkh.Modules.ClaimWork.DomainService
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Entities;

    /// <summary>
    /// Интерфейс для получения информации о иске заявления
    /// </summary>
    public interface ILawsuitInfoService
    {
        /// <summary>
        /// Получить дату заявки
        /// </summary>
        Dictionary<long, DateTime> GetReviewDate(IQueryable<BaseClaimWork> claimWorkQuery);

        /// <summary>
        /// Получить основания ПИР, имеющие дату рассмотрения заявки
        /// </summary>
        IQueryable<BaseClaimWork> GetClaimWorkQueryHasReviewDate();
    }
}
