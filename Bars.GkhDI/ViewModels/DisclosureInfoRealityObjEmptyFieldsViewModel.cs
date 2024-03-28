namespace Bars.GkhDi.ViewModels
{
    using System.Linq;
    using B4;
    using Bars.B4.IoC;
    using Entities;

    public class DisclosureInfoRealityObjEmptyFieldsViewModel : BaseViewModel<DisclosureInfoRealityObjEmptyFields>
    {
        public override IDataResult List(IDomainService<DisclosureInfoRealityObjEmptyFields> domainService, BaseParams baseParams)
        {
            var loadParams = this.GetLoadParam(baseParams);

            var disclosureInfoId = baseParams.Params.GetAs<long>("disclosureInfoId");

            var relationDomain = this.Container.Resolve<IDomainService<DisclosureInfoRelation>>();

            using (this.Container.Using(relationDomain))
            {
                var disInfoObj = relationDomain.GetAll()
                    .Where(x => x.DisclosureInfo.Id == disclosureInfoId)
                    .Select(x => x.DisclosureInfoRealityObj.Id);

                var data = domainService.GetAll()
                    .Where(x => disInfoObj.Any(id => id == x.RealityObj.Id))
                    .Select(x => new
                    {
                        x.Id,
                        x.FieldName,
                        x.PathId,
                        x.RealityObj.RealityObject.Address
                    })
                    .Filter(loadParams, this.Container);

                var totalCount = data.Count();

                return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
            }
        }
    }
}
