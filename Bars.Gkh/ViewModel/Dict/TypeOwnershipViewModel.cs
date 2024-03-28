namespace Bars.Gkh.ViewModel
{
	using Bars.B4;
	using Bars.B4.Utils;
	using Bars.Gkh.Domain;
	using Bars.Gkh.Entities;
	using System.Linq;

	public class TypeOwnershipViewModel : BaseViewModel<TypeOwnership>
	{
		public override IDataResult List(IDomainService<TypeOwnership> domain, BaseParams baseParams)
		{
			var loadParams = GetLoadParam(baseParams);
			var ids = baseParams.Params.GetAs("Id", string.Empty).ToLongArray();

			var data = domain.GetAll()
					.WhereIf(ids.Any(y => y != 0), x => ids.Contains(x.Id))
					.Select(x => new { x.Id, x.Name })
					.Filter(loadParams, Container);;

			return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
		}
	}
}