namespace Bars.GkhGji.Regions.Tomsk.DomainService
{
    using Microsoft.AspNetCore.Mvc;

    using B4;

    public interface IDisposalViolationsService
    {
        ActionResult GetListNotRemovedViolations(BaseParams baseParams);
    }
}
