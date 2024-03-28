namespace Bars.Gkh.DomainService
{
    using Bars.B4;

    public interface IPoliticAuthorityService
    {
        IDataResult AddMunicipalities(BaseParams baseParams);

        IDataResult GetInfo(BaseParams baseParams);
    }
}