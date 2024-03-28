namespace Bars.GkhGji.DomainService
{
    using Bars.B4;

    public interface IBusinessActivityService
    {
        IDataResult CheckDateNotification(BaseParams baseParams);
    }
}