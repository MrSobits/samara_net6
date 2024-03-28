namespace Bars.GkhGji.ViewModel
{
    using System.Linq;

    using B4;
    using B4.Utils;
    using Entities;

    public class AppealCitsRealObjectViewModel : BaseViewModel<AppealCitsRealityObject>
    {
        public override IDataResult List(IDomainService<AppealCitsRealityObject> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var appealCitizensId = baseParams.Params.GetAs<long>("appealCitizensId");

            var data = domainService.GetAll()
                .WhereIf(appealCitizensId > 0, x => x.AppealCits.Id == appealCitizensId)
                .Select(x => new
                    {
                        x.Id,
                        AppealCitizensGji = x.AppealCits,
                        x.RealityObject,
                        RealityObjectId = x.RealityObject.Id,
                        Municipality = x.RealityObject.Municipality.Name,
                        x.RealityObject.Address,
                    })
                .OrderIf(loadParams.Order.Length == 0, true, x => x.Municipality)
                .OrderThenIf(loadParams.Order.Length == 0, true, x => x.Address)
                .Filter(loadParams, Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
        }
    }
}