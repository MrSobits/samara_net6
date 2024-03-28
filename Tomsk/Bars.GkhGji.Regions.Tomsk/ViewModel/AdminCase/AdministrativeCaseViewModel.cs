namespace Bars.GkhGji.Regions.Tomsk.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.GkhGji.Regions.Tomsk.Entities;

    public class AdministrativeCaseViewModel : BaseViewModel<AdministrativeCase>
    {
        public override IDataResult List(IDomainService<AdministrativeCase> domainService, BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();

            var userManager = Container.Resolve<IGkhUserManager>();
            var municipalityList = userManager.GetMunicipalityIds();

            var data = domainService.GetAll()
                    .WhereIf(municipalityList.Count > 0, x => municipalityList.Contains(x.Contragent.Municipality.Id))
                    .Select(x => new
                    {
                        x.Id,
                        x.State,
                        x.DocumentNumber,
                        x.DocumentDate,
                        Municipality = x.RealityObject.Municipality.Name,
                        RealityObject = x.RealityObject.Address,
                        InspectionId = x.Inspection.Id,
                        Inspector = x.Inspector.Fio 
                        
                    })
                    .OrderIf(loadParam.Order.Length == 0, true, x => x.Municipality)
                    .Filter(loadParam, Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParam).Paging(loadParam).ToList(), totalCount);
        }

        public override IDataResult Get(IDomainService<AdministrativeCase> domainService, BaseParams baseParams)
        {
            var intId = baseParams.Params.GetAs<long>("id");
            var obj = domainService.GetAll().FirstOrDefault(x => x.Id == intId);

            obj.InspectionId = obj.Inspection.Id;

            return new BaseDataResult(obj);
        }
    }

}
