namespace Bars.GkhGji.Regions.Chelyabinsk.DomainService
{
    using System.Collections;

    using B4;
    
    public interface ILicenseReissuancePersonService
    {
        IDataResult AddPersons(BaseParams baseParams);

        IDataResult AddProvDocs(BaseParams baseParams);
    }
}