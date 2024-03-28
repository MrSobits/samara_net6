namespace Bars.Gkh.Overhaul.Nso.DomainService.Impl
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Overhaul.Nso.Entities;
    using Bars.GkhCr.Entities;
    using Bars.GkhCr.Enums;

    using Castle.Windsor;

    public class ExecutionLongTermProgramService : IExecutionLongTermProgramService
    {
        #region Dependency injection

        public IWindsorContainer Container { get; set; }
        public IDomainService<ObjectCr> ObjectCrDomain { get; set; }
        public IDomainService<DpkrCorrectionStage2> DpkrCorrectionStage2Domain { get; set; }
        public IDomainService<TypeWorkCr> TypeWorkCrDomain { get; set; }
        public IDomainService<PerformedWorkAct> PerformedWorkActDomain { get; set; }

        #endregion

        public IDataResult List(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();

            var realityObjectId = baseParams.Params.GetAs<long>("realityObjectId");

            var dictWorkSum =
                TypeWorkCrDomain.GetAll()
                    .Where(x => x.ObjectCr.RealityObject.Id == realityObjectId
                            && (x.ObjectCr.ProgramCr.TypeProgramStateCr == TypeProgramStateCr.Active
                                || x.ObjectCr.ProgramCr.TypeProgramStateCr == TypeProgramStateCr.Complete))
                    .Select(x => new { x.Id, x.Sum })
                    .AsEnumerable()
                    .GroupBy(x => x.Id)
                    .ToDictionary(x => x.Key, y => y.Sum(x => x.Sum.HasValue ?  x.Sum.Value : 0M));

            var dictPerformActSum =
                PerformedWorkActDomain.GetAll()
                    .Where(x => x.ObjectCr.RealityObject.Id == realityObjectId
                                && (x.ObjectCr.ProgramCr.TypeProgramStateCr == TypeProgramStateCr.Active
                                    || x.ObjectCr.ProgramCr.TypeProgramStateCr == TypeProgramStateCr.Complete))
                    .Select(x => new {x.TypeWorkCr.Id, x.Sum})
                    .AsEnumerable()
                    .GroupBy(x => x.Id)
                    .ToDictionary(x => x.Key, y => y.Sum(x => x.Sum.HasValue ? x.Sum.Value : 0M));

            var data =
                TypeWorkCrDomain.GetAll()
                    .Where(x => x.ObjectCr.RealityObject.Id == realityObjectId
                                && (x.ObjectCr.ProgramCr.TypeProgramStateCr == TypeProgramStateCr.Active
                                    || x.ObjectCr.ProgramCr.TypeProgramStateCr == TypeProgramStateCr.Complete))
                    .AsEnumerable()
                    .Select(x => new
                    {
                        x.Id,
                        Object = x.ObjectCr.Id,
                        ObjectAddress = x.ObjectCr.RealityObject.Address,
                        ProgramName = x.ObjectCr.ProgramCr.Name,
                        Work = x.Work.Name,
                        Sum = dictWorkSum.ContainsKey(x.Id) ? dictWorkSum[x.Id] : 0M,
                        Perform = dictPerformActSum.ContainsKey(x.Id) ? dictPerformActSum[x.Id] : 0M,
                    })
                    .AsQueryable()
                    .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}