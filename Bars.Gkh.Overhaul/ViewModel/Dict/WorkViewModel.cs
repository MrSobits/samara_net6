namespace Bars.Gkh.Overhaul.ViewModel
{
    using System.Linq;

    using B4;
    using Gkh.Entities.Dicts;
    using Entities;

    /// <summary>
    /// View model работы
    /// </summary>
    public class WorkViewModel : Gkh.ViewModel.WorkViewModel
    {
        /// <inheritdoc />
        public override IDataResult Get(IDomainService<Work> domainService, BaseParams baseParams)
        {
            var finSourceService = this.Container.Resolve<IDomainService<WorkTypeFinSource>>();
            var id = baseParams.Params.GetAs<long>("id");
            var work = domainService.GetAll().FirstOrDefault(x => x.Id == id);

            if(work != null)
            {
                var sourceIds = finSourceService.GetAll()
                    .Where(x => x.Work.Id == id)
                    .Select(x => x.TypeFinSource);

                return new BaseDataResult(new
                {
                    work.Id,
                    work.Name,
                    work.Code,
                    work.ReformCode,
                    work.GisGkhCode,
                    work.GisGkhGuid,
                    work.GisCode,
                    work.WorkAssignment,
                    work.UnitMeasure,
                    work.Normative,
                    work.Description,
                    work.Consistent185Fz,
                    work.DateEnd,
                    work.IsPSD,
                    work.TypeWork,
                    work.IsAdditionalWork,
                    work.IsConstructionWork,
                    FinSources = sourceIds,
                    work.IsActual,
                    work.WithinShortProgram
                });
            }

            return new BaseDataResult();
        }
    }
}
