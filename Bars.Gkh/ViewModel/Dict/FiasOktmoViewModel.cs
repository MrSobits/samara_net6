namespace Bars.Gkh.ViewModel.Dict
{
    using Bars.B4;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities.Dicts;
    using System.Linq;
    using Bars.B4.Modules.FIAS;

    class FiasOktmoViewModel : BaseViewModel<FiasOktmo>
    {
        public IDomainService<Fias> FiasService { get; set; }
        public IDomainService<Entities.RealityObject> RoService { get; set; }

        public override IDataResult List(IDomainService<FiasOktmo> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var data = domainService.GetAll().Join(FiasService.GetAll(),
                x => x.FiasGuid,
                y => y.AOGuid,
                (x, y) => new
                {
                    x.Id,
                    Municipality = x.Municipality.Name,
                    MoOkato = x.Municipality.Okato,
                    MoOktmo = x.Municipality.Oktmo.ToString(),
                    y.OffName,
                    y.OKATO,
                    y.OKTMO,
                    x.FiasGuid
                })
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }

        public override IDataResult Get(IDomainService<FiasOktmo> domainService, BaseParams baseParams)
        {
            var entity = domainService.Get(baseParams.Params.GetAsId("id"));

            if (entity != null)
            {
                var fias =
                    FiasService.GetAll()
                        .FirstOrDefault(x => x.AOGuid == entity.FiasGuid && x.ActStatus == FiasActualStatusEnum.Actual);

                return new BaseDataResult(new
                {
                    entity.Id,
                    entity.Municipality,
                    FiasGuid = fias
                });
            }

            return base.Get(domainService, baseParams);
        }
    }
}
