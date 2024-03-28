namespace Bars.GkhCr.Regions.Tatarstan.ViewModel.ObjectOutdoorCr
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;
    using Bars.GkhCr.Regions.Tatarstan.DomainService;
    using Bars.GkhCr.Regions.Tatarstan.Entities.ObjectOutdoorCr;

    public class ObjectOutdoorCrViewModel : BaseViewModel<ObjectOutdoorCr>
    {
        /// <inheritdoc />
        public override IDataResult List(IDomainService<ObjectOutdoorCr> domainService, BaseParams baseParams)
        {
            var service = this.Container.Resolve<IObjectOutdoorCrService>();
            using (this.Container.Using(service))
            {
                return service.GetList(domainService, baseParams);
            }
        }

        /// <inheritdoc />
        public override IDataResult Get(IDomainService<ObjectOutdoorCr> domainService, BaseParams baseParams)
        {
            var entity = domainService.Get(baseParams.Params.GetAsId());
            if (entity == null)
            {
                return base.Get(domainService, baseParams);
            }

            var realityObjectDomain = this.Container.ResolveDomain<RealityObject>();

            using (this.Container.Using(realityObjectDomain))
            {
                return new BaseDataResult(new
                {
                    entity.Id,
                    entity.DateEndBuilder,
                    entity.DateStartWork,
                    entity.DateAcceptGji,
                    entity.DateStopWorkGji,
                    entity.DateGjiReg,
                    entity.DateEndWork,
                    entity.СommissioningDate,
                    entity.SumDevolopmentPsd,
                    entity.SumSmr,
                    entity.SumSmrApproved,
                    entity.Description,
                    entity.MaxAmount,
                    entity.FactAmountSpent,
                    entity.FactStartDate,
                    entity.FactEndDate,
                    entity.WarrantyEndDate,
                    RealityObjects = string.Join("; ",
                        realityObjectDomain.GetAll().Where(x => x.Outdoor.Id == entity.RealityObjectOutdoor.Id).Select(x => x.Address)),
                    entity.GjiNum,
                    entity.RealityObjectOutdoor?.MunicipalityFiasOktmo?.Municipality,
                    entity.RealityObjectOutdoorProgram,
                    OutdoorProgramName = entity.RealityObjectOutdoorProgram?.Name,
                    entity.BeforeDeleteRealityObjectOutdoorProgram,
                    entity.RealityObjectOutdoor,
                    RealityObjectOutdoorName = entity.RealityObjectOutdoor?.Name,
                    RealityObjectOutdoorCode = entity.RealityObjectOutdoor?.Code,
                    entity.State
                });
            }
        }
    }
}
