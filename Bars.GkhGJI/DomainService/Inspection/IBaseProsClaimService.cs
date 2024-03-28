namespace Bars.GkhGji.DomainService
{
    using Bars.B4;

    public interface IBaseProsClaimService
    {
        IDataResult GetInfo(BaseParams baseParams);

        IDataResult ChangeInspState(BaseParams baseParams);
    }
}
