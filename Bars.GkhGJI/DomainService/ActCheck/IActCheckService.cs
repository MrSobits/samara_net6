namespace Bars.GkhGji.DomainService
{
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.GkhGji.Entities;

    public interface IActCheckService
    {
        IDataResult GetInfo(BaseParams baseParams);

        IDataResult ListView(BaseParams baseParams);

        IDataResult ListForStage(BaseParams baseParams);

        IQueryable<ViewActCheck> GetViewList();
        
        IDataResult AddActCheckControlMeasures(BaseParams baseParams);
        
        /// <summary>
        /// Выгрузить в Excel
        /// </summary>
        ActionResult Export(BaseParams baseParams);
    }
}