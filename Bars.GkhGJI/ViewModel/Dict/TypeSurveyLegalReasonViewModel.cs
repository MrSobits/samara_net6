namespace Bars.GkhGji.ViewModel.Dict
{
    using System.Linq;

    using Bars.B4;
    using Bars.GkhGji.Entities.Dict;

	/// <summary>
	/// Вьюмодель для Правовые основания типа обследования
	/// </summary>
    public class TypeSurveyLegalReasonViewModel : BaseViewModel<TypeSurveyLegalReason>
    {
		/// <summary>
		/// Получить список
		/// </summary>
		/// <param name="domainService">Домен</param><param name="baseParams">Базовые параметры</param>
		/// <returns>Результат получения списка</returns>
		public override IDataResult List(IDomainService<TypeSurveyLegalReason> domainService, BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();

            var typeSurveyId = baseParams.Params.GetAs<long>("typeSurveyId");

            var data = domainService.GetAll().Where(x => x.TypeSurveyGji.Id == typeSurveyId).Select(x => new { x.Id, x.LegalReason.Code, x.LegalReason.Name });

            var totalCount = data.Count();

            return new ListDataResult(data.Order(loadParam).Paging(loadParam).ToList(), totalCount);
        }
    }
}