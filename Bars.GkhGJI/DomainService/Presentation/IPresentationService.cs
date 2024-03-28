using System.Linq;
using Bars.GkhGji.Entities;

namespace Bars.GkhGji.DomainService
{
    using Microsoft.AspNetCore.Mvc;

    using B4;

    public interface IPresentationService
    {
        IDataResult GetInfo(BaseParams baseParams);

        /// <summary>
        /// Наложение фильтра по операторам
        /// </summary>
        /// <returns></returns>
        IQueryable<Presentation> GetFilteredByOperator(IDomainService<Presentation> domainService);

        /// <summary>
        /// Выгрузить в Excel
        /// </summary>
        ActionResult Export(BaseParams baseParams);
    }
}