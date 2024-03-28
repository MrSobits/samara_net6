namespace Bars.Gkh.Overhaul.Nso.DomainService
{
    using B4;

    public interface IRealEstateTypeRateService
    {
        IDataResult CalculateRates(BaseParams baseParams);
    }
}