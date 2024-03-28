namespace Bars.GkhGji.DomainService
{
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.GkhGji.Entities;

    public interface IPrescriptionService
    {
        IDataResult GetInfo(long? documentId);

        IDataResult ListView(BaseParams baseParams);

        IDataResult ListForStage(BaseParams baseParams);

        IQueryable<ViewPrescription> GetViewList();

        /// <summary>
        /// Выгрузить в Excel
        /// </summary>
        ActionResult Export(BaseParams baseParams);
    }
}