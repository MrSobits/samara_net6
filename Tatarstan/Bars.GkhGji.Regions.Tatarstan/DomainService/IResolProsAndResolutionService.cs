namespace Bars.GkhGji.Regions.Tatarstan.DomainService
{
    using System.Collections.Generic;

    using Bars.B4;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.Tatarstan.Entities;

    public interface IResolProsAndResolutionService
    {
        IList<ResolProsAndResolution> GetList(BaseParams baseParam, IDomainService<ResolPros> resolProsService, bool toExcel, bool useResolProsRoService);
    }
}