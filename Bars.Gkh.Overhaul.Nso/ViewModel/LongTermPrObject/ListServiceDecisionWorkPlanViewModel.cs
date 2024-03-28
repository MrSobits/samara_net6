namespace Bars.Gkh.Overhaul.Nso.ViewModel
{
    using System.Linq;
    using B4;
    using Entities;
    using Overhaul.Entities;

    public class ListServiceDecisionWorkPlanViewModel : BaseViewModel<ListServiceDecisionWorkPlan>
    {
        public IDomainService<StructuralElementWork> ElementWork { get; set; }

        public IDomainService<PublishedProgramRecord> PublishedDomain { get; set; }

        public IDomainService<ListServicesDecision> DecisionDomain { get; set; } 

        public override IDataResult List(IDomainService<ListServiceDecisionWorkPlan> domainService, BaseParams baseParams)
        {
            var decisionId = baseParams.Params.GetAs<long>("decisionId");

            var loadParam = baseParams.GetLoadParam();

            var publishedData = PublishedDomain.GetAll()
                    .Where(x => x.PublishedProgram.ProgramVersion.IsMain)
                    .Select(x => new 
                    {
                        x.Stage2.CommonEstateObject.Id,
                        x.PublishedYear,
                        RoId = x.Stage2.Stage3Version.RealityObject.Id
                    });

            var tempData = domainService.GetAll()
                .Where(x => x.ListServicesDecision.Id == decisionId)
                .Select(x => new
                {
                    x.Id,
                    WorkName = x.Work.Name,
                    WorkId = x.Work.Id,
                    x.FactYear,
                    RoId = x.ListServicesDecision.RealityObject.Id
                })
                .AsEnumerable()
                .Select(x => new
                {
                    x.Id,
                    Work = x.WorkName,
                    x.FactYear,
                    PlanYear =
                        publishedData
                            .Where(y => y.RoId == x.RoId)
                            .Where(p => ElementWork.GetAll()
                                    .Any(ew => ew.StructuralElement.Group.CommonEstateObject.Id == p.Id
                                               && ew.Job.Work.Id == x.WorkId))
                            .Select(p => (int?) p.PublishedYear)
                            .OrderBy(p => p)
                            .FirstOrDefault()
                })
                .AsQueryable()
                .Filter(loadParam, Container);

            var data =
                tempData
                    .Select(x => new
                    {
                        x.Id,
                        x.Work,
                        x.FactYear,
                        PlanYear = x.PlanYear.HasValue ? x.PlanYear.Value.ToString() : "-"
                    });

            return new ListDataResult(data.Order(loadParam).Paging(loadParam), data.Count());
        }
    }
}