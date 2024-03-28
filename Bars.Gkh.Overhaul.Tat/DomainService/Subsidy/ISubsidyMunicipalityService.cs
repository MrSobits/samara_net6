namespace Bars.Gkh.Overhaul.Tat.DomainService
{
    using Bars.B4;

    public interface ISubsidyMunicipalityService
    {
        IDataResult GetSubsidy(BaseParams baseParams);

        IDataResult CalcValues(BaseParams baseParams);

        IDataResult CorrectDpkr(BaseParams baseParams);
    }
}