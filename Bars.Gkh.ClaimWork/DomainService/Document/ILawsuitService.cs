﻿namespace Bars.Gkh.ClaimWork.DomainService.Document
{
    using System.Collections;
    using B4;

    /// <summary>
    /// Интерфейс для уведомлений
    /// </summary>
    public interface ILawsuitService
    {
        /// <summary>
        /// Получений списка актов
        /// </summary>
        /// <param name="baseParams"></param>
        /// <param name="usePaging"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        IList GetList(BaseParams baseParams, bool usePaging, out int totalCount);
    }
}