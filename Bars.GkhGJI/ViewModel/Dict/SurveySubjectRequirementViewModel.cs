namespace Bars.GkhGji.ViewModel.Dict
{
	using System.Linq;

	using Bars.B4;
	using Bars.B4.Utils;
	using Bars.GkhGji.Entities.Dict;

	public class SurveySubjectRequirementViewModel : BaseViewModel<SurveySubjectRequirement>
    {
        /// <summary>
        ///     Получить список
        /// </summary>
        /// <param name="domainService">Домен</param>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns>
        ///     Результат получения списка
        /// </returns>
        public override IDataResult List(IDomainService<SurveySubjectRequirement> domainService, BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var ids =
                baseParams.Params.GetAs<string>("Id")
                          .Return(x => x.Split(',').Select(y => y.Trim().ToLong()).ToArray(), new long[0]);

            var data = domainService.GetAll()
                                    .WhereIf(ids.Length > 0, x => ids.Contains(x.Id))
                                    .Filter(loadParams, this.Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams), data.Count());
        }
    }
}