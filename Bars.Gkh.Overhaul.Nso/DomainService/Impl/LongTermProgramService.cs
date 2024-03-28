namespace Bars.Gkh.Overhaul.Nso.DomainService.Impl
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Overhaul.Nso.Entities;

    using Castle.Windsor;

    public class LongTermProgramService : ILongTermProgramService
    {
        #region Dependency injection

        public IWindsorContainer Container { get; set; }
        public IDomainService<DpkrCorrectionStage2> DpkrCorrectionStage2Domain { get; set; }

        #endregion

        public IDataResult List(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();

            var realityObjectId = baseParams.Params.GetAs<long>("realityObjectId");

            var data = DpkrCorrectionStage2Domain.GetAll()
                .Where(x => x.RealityObject.Id == realityObjectId)
                .Select(x => new
                {
                    x.Id,
                    CommonEstateObject = x.Stage2.CommonEstateObject.Name,
                    Year = x.PlanYear,
                    x.Stage2.Sum
                })
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}