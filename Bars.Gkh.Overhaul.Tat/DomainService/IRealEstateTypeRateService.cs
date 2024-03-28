namespace Bars.Gkh.Overhaul.Tat.DomainService
{
    using B4;

    public interface IRealEstateTypeRateService
    {
        IDataResult CalculateRates(BaseParams baseParams); 
    }
}