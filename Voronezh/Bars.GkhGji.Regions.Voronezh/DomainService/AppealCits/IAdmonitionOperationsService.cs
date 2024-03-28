namespace Bars.GkhGji.Regions.Voronezh.DomainService
{
    using Entities;
    using System.Collections;

    using B4;

    public interface IAdmonitionOperationsService
    {
        IDataResult ListDocsForSelect(BaseParams baseParams);

        IDataResult SaveAppeal(BaseParams baseParams);
        IDataResult SaveViolations(BaseParams baseParams);

        IDataResult RemoveRelated(long admonId, long appealNumber);
    }
}