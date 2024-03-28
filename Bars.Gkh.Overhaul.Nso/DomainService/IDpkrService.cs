namespace Bars.Gkh.Overhaul.Nso.DomainService
{
    using Bars.B4;

    public interface IDpkrService
    {
        IDataResult GetYears(BaseParams baseParams);

        IDataResult GetMunicipality(BaseParams baseParams);

        IDataResult GetRealityObjects(BaseParams baseParams);

        ListDataResult GetRecords(BaseParams baseParams);
    }
}