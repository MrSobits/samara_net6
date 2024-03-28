namespace Bars.GkhGji.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Domain;
    using Bars.GkhGji.Entities;

    public class HeatInputPeriodViewModel : BaseViewModel<HeatInputPeriod>
    {
        public override IDataResult Get(IDomainService<HeatInputPeriod> domainService, BaseParams baseParams)
        {
            var id = baseParams.Params.GetAsId();
            var record = domainService.Get(id);

            if (record.IsNull())
            {
                return BaseDataResult.Error("Не удалось получить период подачи тепла");
            }

            var result = new
            {
                record.Id,
                record.Month,
                record.Year,
                Municipality = record.Municipality.Name,
                MunicipalityId = record.Municipality.Id
            };

            return new BaseDataResult(result);
        }


        public override IDataResult List(IDomainService<HeatInputPeriod> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var userManager = this.Container.Resolve<IGkhUserManager>();
            var municipalityList = userManager.GetMunicipalityIds();

            var data =
                domainService.GetAll()
                    .WhereIf(municipalityList.Count > 0, x => municipalityList.Contains(x.Municipality.Id))
                    .Select(x => new
                    {
                        x.Id,
                        x.Month,
                        x.Year,
                        Municipality = x.Municipality.Name
                    })
                    .OrderIf(loadParams.Order.Length == 0, true, x => x.Municipality)
                    .Filter(loadParams, Container);
            
            var totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
        }
    }
}