namespace Bars.Gkh.Overhaul.Tat.DomainService
{
    using System.Linq;

    using B4;

    using Bars.Gkh.Overhaul.Tat.Entities;

    public interface IActualizeVersionService
    {
        IDataResult ActualizeFromShortCr(BaseParams baseParams);

        IDataResult ActualizeNewRecords(BaseParams baseParams);

        IDataResult ActualizeSum(BaseParams baseParams);

        IDataResult ActualizeYear(BaseParams baseParams);

        IDataResult ActualizeDeletedEntries(BaseParams baseParams);

        IDataResult ActualizeGroup(BaseParams baseParams);

        IDataResult ActualizeOrder(BaseParams baseParams);

        IQueryable<VersionRecordStage1> GetDeletedEntriesQueryable(long versionId, int actualizeStartYear);

        IDataResult GetWarningMessage(BaseParams baseParams);



    }
}
