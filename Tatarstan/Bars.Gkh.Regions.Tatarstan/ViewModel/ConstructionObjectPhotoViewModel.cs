namespace Bars.Gkh.Regions.Tatarstan.ViewModel
{
	using System.Linq;

	using Bars.B4;
	using Bars.Gkh.Domain;
	using Bars.Gkh.Regions.Tatarstan.Entities;

	/// <summary>
	/// ViewModel для <see cref="ConstructionObjectPhoto"/>
	/// </summary>
	public class ConstructionObjectPhotoViewModel : BaseViewModel<ConstructionObjectPhoto>
	{
		/// <summary>
		/// Получить список
		/// </summary>
		/// <param name="domain">Домен-сервис</param>
		/// <param name="baseParams">Базовые параметры</param>
		/// <returns>Результат выполнения запроса</returns>
		public override IDataResult List(IDomainService<ConstructionObjectPhoto> domain, BaseParams baseParams)
		{
			var loadParams = this.GetLoadParam(baseParams);
			var objectId = baseParams.Params.GetAsId("objectId");

			var data = domain.GetAll()
				.Where(x => x.ConstructionObject.Id == objectId)
				.Filter(loadParams, this.Container)
				.Order(loadParams);

			var totalCount = data.Count();

			return new ListDataResult(data.Paging(loadParams).ToList(), totalCount);
		}
	}
}