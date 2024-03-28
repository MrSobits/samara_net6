namespace Bars.Gkh.ViewModel.RealityObject
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.FIAS;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Domain;
    using Bars.Gkh.DomainService.RealityObjectOutdoor;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities.RealityObj;
    using Bars.Gkh.Utils;

    public class RealityObjectOutdoorViewModel : BaseViewModel<RealityObjectOutdoor>
    {
        /// <inheritdoc />
        public override IDataResult List(IDomainService<RealityObjectOutdoor> domainService, BaseParams baseParams)
        {
            var roOutdoorService = this.Container.Resolve<IRealityObjectOutdoorService>();
            using (this.Container.Using(roOutdoorService))
            {
                return roOutdoorService.GetList(domainService, baseParams);
            }
        }
            
        /// <inheritdoc />
        public override IDataResult Get(IDomainService<RealityObjectOutdoor> domainService, BaseParams baseParams)
        {
            var entity = domainService.Get(baseParams.Params.GetAsId());
            if (entity == null)
            {
                return base.Get(domainService, baseParams);
            }

            var fiasDomain = this.Container.ResolveDomain<Fias>();
            using (this.Container.Using(fiasDomain))
            {
                var fiasOffName = fiasDomain.GetAll()
                    .FirstOrDefault(x => x.AOGuid == entity.MunicipalityFiasOktmo.FiasGuid
                        && x.ActStatus == FiasActualStatusEnum.Actual)?.OffName;

                return new BaseDataResult(new
                {
                    entity.Id,
                    entity.Name,
                    entity.Code,
                    entity.Area,
                    entity.AsphaltArea,
                    entity.Description,
                    entity.RepairPlanYear,
                    MunicipalityFiasOktmo = new
                    {
                        entity.MunicipalityFiasOktmo.Id,
                        OffName = fiasOffName
                    }
                });
            }
        }
    }
}
