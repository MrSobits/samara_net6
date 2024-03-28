using System.Linq;

namespace Bars.GkhGji.DomainService
{
    using B4;
    using Bars.GkhGji.DomainService.Contracts;
    using Entities;

    public interface IDecisionService
    {
        DecisionInfo GetInfo(long documentId);

        IDataResult GetInfo(BaseParams baseParams);

        IDataResult ListView(BaseParams baseParams);

        IDataResult ListNullInspection(BaseParams baseParams);

        IQueryable<ViewDecision> GetViewList();

        IDataResult AddDisposalControlMeasures(BaseParams baseParams);

        IDataResult AddExperts(BaseParams baseParams);

        IDataResult AddControlLists(BaseParams baseParams);

        IDataResult AddInspectionReasons(BaseParams baseParams);

        IDataResult AddAdminRegulations(BaseParams baseParams);

        IDataResult AddSurveySubjects(BaseParams baseParams);

        IDataResult AddProvidedDocs(BaseParams baseParams);

    }
}