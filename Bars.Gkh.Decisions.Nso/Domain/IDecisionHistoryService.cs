namespace Bars.Gkh.Decisions.Nso.Domain
{
    using B4;

    public interface IDecisionHistoryService
    {
        IDataResult GetHistoryTree(BaseParams baseParams);

        IDataResult GetJobYearsHistory(BaseParams baseParams);
    }
}