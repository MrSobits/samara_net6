namespace Bars.GkhGji.ViewModel.Dict
{
    using System.Linq;

    using Bars.B4;
    using Bars.GkhGji.Entities.Dict;

	/// <summary>
	/// Вьюмодель для Типы конрагента
	/// </summary>
    public class TypeSurveyContragentTypeViewModel : BaseViewModel<TypeSurveyContragentType>
    {
		/// <summary>
		/// Получить список
		/// </summary>
		/// <param name="domainService">Домен</param><param name="baseParams">Базовые параметры</param>
		/// <returns>Результат получения списка</returns>
		public override IDataResult List(IDomainService<TypeSurveyContragentType> domainService, BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();

            var typeSurveyId = baseParams.Params.GetAs<long>("typeSurveyId");

            var data = domainService.GetAll().Where(x => x.TypeSurveyGji.Id == typeSurveyId).Select(x => new { x.Id, TypeSurveyGji = x.TypeSurveyGji.Id, x.TypeJurPerson });

            var totalCount = data.Count();

            return new ListDataResult(data.Order(loadParam).Paging(loadParam).ToList().Select(x => new { x.Id, x.TypeSurveyGji, TypeJurPerson = (int)x.TypeJurPerson }), totalCount);
        }
    }
}