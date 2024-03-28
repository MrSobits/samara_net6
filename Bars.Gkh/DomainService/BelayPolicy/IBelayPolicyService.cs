namespace Bars.Gkh.DomainService
{
    using Bars.B4;

    public interface IBelayPolicyService
    {
        IDataResult GetInfo(BaseParams baseParams);
    }
}