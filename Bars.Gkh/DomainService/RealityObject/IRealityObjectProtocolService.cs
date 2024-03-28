namespace Bars.Gkh.DomainService
{
    using Bars.B4;

    public interface IRealityObjectProtocolService
    {
        IDataResult GetProtocolByRealityObjectId(BaseParams baseParams);
    }
}