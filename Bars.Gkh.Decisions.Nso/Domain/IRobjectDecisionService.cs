namespace Bars.Gkh.Decisions.Nso.Domain
{
    using B4;

    public interface IRobjectDecisionService
    {
        IDataResult Get(BaseParams baseParams);

        IDataResult SaveOrUpdateDecisions(BaseParams baseParams);
    }
}