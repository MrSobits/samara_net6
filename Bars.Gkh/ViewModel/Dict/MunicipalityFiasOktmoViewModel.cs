namespace Bars.Gkh.ViewModel.Dict
{
    using Bars.B4;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities.Dicts;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FIAS;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Utils;

    public class MunicipalityFiasOktmoViewModel : BaseViewModel<MunicipalityFiasOktmo>
    {
		public IDomainService<Fias> FiasDomainService { get; set; }
		public IRepository<Municipality> MunicipalityRepository { get; set; }
		public IFiasRepository FiasRepository { get; set; }
        public IGkhUserManager UserManager { get; set; }

        public override IDataResult List(IDomainService<MunicipalityFiasOktmo> domainService, BaseParams baseParams)
        {
			var municipalityId = baseParams.Params.GetAs<long>("municipalityId");
            var needMunicipalityFilter = baseParams.Params.GetAs<bool>("needMunicipalityFilter");

            var fiasGuids = domainService.GetAll()
                .Select(x => x.FiasGuid)
                .Distinct()
                .ToHashSet();

            var fiasOffNameDict = this.FiasDomainService.GetAll()
                .Where(x => fiasGuids.Contains(x.AOGuid))
                .Where(x => x.ActStatus == FiasActualStatusEnum.Actual)
                .Select(x => new
                    {
                        x.AOGuid,
                        x.OffName
                    })
                .AsEnumerable()
                .GroupBy(x => x.AOGuid)
                .ToDictionary(x => x.Key, x => x.First().OffName);

            var municipalities = this.UserManager.GetMunicipalityIds();

            return domainService.GetAll()
                .WhereIf(municipalityId > 0, x => x.Municipality.Id == municipalityId)
                .WhereIf(needMunicipalityFilter && municipalities.Any(), x => municipalities.Contains(x.Municipality.Id))
                .AsEnumerable()
                .Select(x => new
				{
					x.Id,
					x.FiasGuid,
					x.Oktmo,
					OffName = fiasOffNameDict.ContainsKey(x.FiasGuid) ? fiasOffNameDict[x.FiasGuid] : "",
                    Municipality = x.Municipality.Name
				})
				.ToListDataResult(baseParams.GetLoadParam(), this.Container);
        }

        public override IDataResult Get(IDomainService<MunicipalityFiasOktmo> domainService, BaseParams baseParams)
        {
            var entity = domainService.Get(baseParams.Params.GetAsId());
			if (entity != null)
			{
				var fias = this.FiasDomainService.GetAll()
					.FirstOrDefault(x => x.AOGuid == entity.FiasGuid && x.ActStatus == FiasActualStatusEnum.Actual);

                return new BaseDataResult(new
                {
                    entity.Id,
					FiasGuid = fias,
                    entity.Oktmo,
					entity.Municipality
                });
            }

            return base.Get(domainService, baseParams);
        }
    }
}
