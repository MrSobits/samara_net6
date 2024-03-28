namespace Bars.Gkh.Overhaul.Tat.DomainService
{
    using B4;

    public interface IProgramVersionService
    {
        IDataResult CopyProgram(BaseParams baseParams);

        IDataResult GetMainVersionByMunicipality(BaseParams baseParams);

        IDataResult GetDeletedEntriesList(BaseParams baseParams);

        IDataResult ListMainVersions(BaseParams baseParams);
    }
}
