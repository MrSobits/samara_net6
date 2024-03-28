namespace Bars.GkhRf.DomainService
{
    using Bars.B4;

    public interface IRealObjForTransferService
    {
        IDataResult List(BaseParams baseParams);
    }
}