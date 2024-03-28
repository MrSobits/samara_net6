namespace Bars.Gkh.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Entities;

    public class RealityObjectServiceOrgViewModel : BaseViewModel<RealityObjectServiceOrg>
    {
        public override IDataResult List(IDomainService<RealityObjectServiceOrg> domain, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);
            var objectId = baseParams.Params.GetAs<long>("objectId");

            var data = domain.GetAll()
                .Where(x => x.RealityObject.Id == objectId)
                .Select(x => new
                {
                    x.Id,
                    x.TypeServiceOrg,
                    ContragentName = x.Organization.Name,
                    x.DocumentNum,
                    x.DocumentName,
                    x.DocumentDate,
                    x.File
                })
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}