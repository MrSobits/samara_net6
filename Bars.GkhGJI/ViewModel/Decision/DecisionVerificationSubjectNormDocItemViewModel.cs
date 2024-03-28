namespace Bars.GkhGji.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Domain;
    using Entities;
	/// <summary>
	/// Представление <see cref="DecisionVerificationSubjectNormDocItem"/>
	/// </summary>
	public class DecisionVerificationSubjectNormDocItemViewModel : BaseViewModel<DecisionVerificationSubjectNormDocItem>
    {
		/// <summary>
		/// Получить список
		/// </summary>
		/// <param name="domain">Домен</param>
		/// <param name="baseParams">Базовые параметры</param>
		/// <returns>Результат выполнения запроса</returns>
		public override IDataResult List(IDomainService<DecisionVerificationSubjectNormDocItem> domain, BaseParams baseParams)
        {
			var loadParam = baseParams.GetLoadParam();
			var subjId = baseParams.Params.GetAsId("subjId");

	        var data = domain.GetAll()
		        .Where(x => x.DecisionVerificationSubject.Id == subjId)
		        .Select(x => new
		        {
			        x.Id,
			        x.NormativeDocItem.Number,
                    x.NormativeDocItem.Text,
					NormativeDoc = x.NormativeDocItem.NormativeDoc.Name
				})
		        .Filter(loadParam, this.Container)
		        .Order(loadParam);

			var totalCount = data.Count();

			return new ListDataResult(data.Paging(loadParam).ToList(), totalCount);
		}
    }
}
