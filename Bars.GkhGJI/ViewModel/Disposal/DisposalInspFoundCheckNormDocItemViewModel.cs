namespace Bars.GkhGji.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Domain;
    using Entities;
    /// <summary>
	/// Представление <see cref="DisposalInspFoundCheckNormDocItem"/>
	/// </summary>
    public class DisposalInspFoundCheckNormDocItemViewModel : BaseViewModel<DisposalInspFoundCheckNormDocItem>
    {
		/// <summary>
		/// Получить список
		/// </summary>
		/// <param name="domain">Домен</param>
		/// <param name="baseParams">Базовые параметры</param>
		/// <returns>Результат выполнения запроса</returns>
		public override IDataResult List(IDomainService<DisposalInspFoundCheckNormDocItem> domain, BaseParams baseParams)
        {
			var loadParam = baseParams.GetLoadParam();
			var foundCheckId = baseParams.Params.GetAsId("foundCheckId");

	        var data = domain.GetAll()
		        .Where(x => x.DisposalInspFoundationCheck.Id == foundCheckId)
		        .Select(x => new
		        {
			        x.Id,
			        x.NormativeDocItem.Number,
                    x.NormativeDocItem.Text
		        })
		        .Filter(loadParam, this.Container)
		        .Order(loadParam);

			var totalCount = data.Count();

			return new ListDataResult(data.Paging(loadParam).ToList(), totalCount);
		}
    }
}
