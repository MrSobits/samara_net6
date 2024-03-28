namespace Bars.GkhGji.Regions.Habarovsk.DomainService
{
    using System.Collections;

    using B4;
    
    public interface ILicenseReissuancePersonService
    {
        IDataResult AddPersons(BaseParams baseParams);

        IDataResult AddProvDocs(BaseParams baseParams);
    }
}