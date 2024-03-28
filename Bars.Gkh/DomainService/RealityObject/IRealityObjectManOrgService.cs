namespace Bars.Gkh.DomainService
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using Bars.Gkh.Entities;

    /// <summary>
    /// Сервис для работы УО МКД
    /// </summary>
    public interface IRealityObjectManOrgService
    {
        /// <summary>
        /// Получить словарь актуальных управляющих организаций по домам
        /// </summary>
        /// <param name="byDate">На дату</param>
        IDictionary<long, ManagingOrganization> GetActualManagingOrganizations(DateTime? byDate = null);

        /// <summary>
        /// Получить актуальные управляющие организации по домам
        /// </summary>
        /// <param name="byDate">На дату</param>
        IQueryable<ManOrgContractRealityObject> GetActualManagingOrganizationsQuery(DateTime? byDate = null);
    }
}