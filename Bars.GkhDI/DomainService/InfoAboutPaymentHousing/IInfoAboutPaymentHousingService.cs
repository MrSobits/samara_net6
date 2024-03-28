namespace Bars.GkhDi.DomainService
{
    using Bars.B4;

    public interface IInfoAboutPaymentHousingService
    {
        IDataResult SaveInfoAboutPaymentHousing(BaseParams baseParams);
    }
}