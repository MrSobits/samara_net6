namespace Bars.Gkh.Overhaul.Nso.DomainService
{
    using System.Linq;

    using B4;

    using Bars.Gkh.Overhaul.Nso.Entities;

    public interface IActualizeVersionService
    {
        // пока данном методе нет необходимости
        //IDataResult ActualizeFromShortCr(BaseParams baseParams);

        IDataResult ActualizeNewRecords(BaseParams baseParams);

        IDataResult ActualizeSum(BaseParams baseParams);

        IDataResult ActualizeYear(BaseParams baseParams);

        IDataResult ActualizeDeletedEntries(BaseParams baseParams);

        // пока нет необходимости в этом методе
        //IDataResult ActualizeGroup(BaseParams baseParams);

        IDataResult ActualizeOrder(BaseParams baseParams);

        IDataResult GetDeletedEntriesList(BaseParams baseParams);

        IQueryable<VersionRecordStage1> GetDeletedEntriesQueryable(long versionId, int actualizeStartYear);

        IDataResult GetWarningMessage(BaseParams baseParams);



    }
}
