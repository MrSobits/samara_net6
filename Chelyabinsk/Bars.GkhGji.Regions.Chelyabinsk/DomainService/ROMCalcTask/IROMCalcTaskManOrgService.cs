namespace Bars.GkhGji.Regions.Chelyabinsk.DomainService
{
    using Entities;
    using System.Collections;

    using B4;
    
    public interface IROMCalcTaskManOrgService
    {
        IDataResult AddManOrg(BaseParams baseParams);
    }
}