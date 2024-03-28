namespace Bars.Gkh.Overhaul.Tat.ViewModel
{
    using System;
    using System.Linq;
    using B4;
    using Entities;
    using Overhaul.Entities;

    public class ListServiceDecisionWorkPlanViewModel : BaseViewModel<ListServiceDecisionWorkPlan>
    {
        public IDomainService<StructuralElementWork> ElementWork { get; set; }

        public IDomainService<VersionRecordStage1> VersionStage1Domain { get; set; }

        public override IDataResult List(IDomainService<ListServiceDecisionWorkPlan> domainService, BaseParams baseParams)
        {
            var decisionId = baseParams.Params.GetAs<long>("decisionId");

            var loadParam = baseParams.GetLoadParam();

            var currentYear = DateTime.Now.Date.Year;

            var stage3Data = VersionStage1Domain.GetAll()
                .Where(x => x.Stage2Version.Stage3Version.ProgramVersion.IsMain)
                .Where(x => x.Stage2Version.Stage3Version.ProgramVersion.State.FinalState)
                .Where(x => x.Year >= currentYear)
                .Select(x => new
                {
                    RoId = x.RealityObject.Id,
                    Year = x.Stage2Version.Stage3Version.CorrectYear,
                    SeId = x.StrElement.Id
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
#warning неоптимально, делается запрос для каждой записи, а если убрать AsEnumerable то в оракле упадет
                .Select(x => new
                {
                    x.Id,
                    Work = x.WorkName,
                    x.FactYear,
                    PlanYear =
                        stage3Data
                            .Where(p => p.RoId == x.RoId)
                            .Where(p => ElementWork.GetAll()
                                .Where(ew => ew.StructuralElement.Id == p.SeId)
                                .Any(ew => ew.Job.Work.Id == x.WorkId))
                            .Select(p => (int?) p.Year)
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
                        PlanYear = x.PlanYear.HasValue ? x.PlanYear.Value.ToString("0000") : "-"
                    });

            return new ListDataResult(data.Order(loadParam).Paging(loadParam), data.Count());
        }
    }
}