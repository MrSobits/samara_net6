namespace Bars.GkhGji.ViewModel.Dict
{
	using System.Linq;

	using Bars.B4;
	using Bars.B4.Utils;
	using Bars.GkhGji.Entities.Dict;

	public class TypeFactViolationViewModel : BaseViewModel<TypeFactViolation>
    {
        public override IDataResult List(IDomainService<TypeFactViolation> domainService, BaseParams baseParams)
        {
            var loadParams = this.GetLoadParam(baseParams);

            var ids = baseParams.Params.GetAs("Id", string.Empty);

            var listIds = !string.IsNullOrEmpty(ids) ? ids.Split(',').Select(id => id.ToLong()).ToArray() : new long[0];

            var data = domainService.GetAll()
                .WhereIf(listIds.Length > 0, x => listIds.Contains(x.Id))
                .Filter(loadParams, this.Container);

            var totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
        }
    }
}