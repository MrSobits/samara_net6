namespace Bars.GkhGji.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.GkhGji.Entities;

    /// <summary>
	/// Вьюмодель для "НПА требований"
	/// </summary>
    public class DisposalInspFoundationCheckViewModel : BaseViewModel<DisposalInspFoundationCheck>
    {
		/// <summary>
		/// Получить объект
		/// </summary>
		/// <param name="domain">Домен</param>
		/// <param name="baseParams">Базовые параметры</param>
		/// <returns>Результат выполнения запроса</returns>
		public override IDataResult List(IDomainService<DisposalInspFoundationCheck> domain, BaseParams baseParams)
        {
			var loadParam = baseParams.GetLoadParam();
			var documentId = baseParams.Params.GetAs<long>("documentId");

	        var data = domain.GetAll()
		        .Where(x => x.Disposal.Id == documentId)
		        .Select(x => new
		        {
			        x.Id,
                    NormDocId = x.InspFoundationCheck.Id,
			        x.InspFoundationCheck.Code,
			        x.InspFoundationCheck.Name
		        })
		        .Filter(loadParam, this.Container)
		        .Order(loadParam);

			var totalCount = data.Count();

			return new ListDataResult(data.Paging(loadParam).ToList(), totalCount);
		}
    }
}
