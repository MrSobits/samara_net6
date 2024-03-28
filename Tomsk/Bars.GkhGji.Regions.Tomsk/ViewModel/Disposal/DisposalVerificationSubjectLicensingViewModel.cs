namespace Bars.GkhGji.Regions.Tomsk.ViewModel
{
	using System.Linq;
	using B4;
	using Entities;
	using GkhGji.Entities.Dict;

	/// <summary>
	/// Вьюмодель для Предмет проверки для приказа лицензирование
	/// </summary>
	public class DisposalVerificationSubjectLicensingViewModel : BaseViewModel<DisposalVerificationSubjectLicensing>
    {
		/// <summary>
		/// Домен сервис для Предметы проверки Лицензирование
		/// </summary>
		public IDomainService<SurveySubjectLicensing> SurveySubjectDomain { get; set; } 

		/// <summary>
		/// Получить список
		/// </summary>
		/// <param name="domain">Домен сервис</param>
		/// <param name="baseParams">Базовые параметры</param>
		/// <returns>Результат выполнения запроса</returns>
        public override IDataResult List(IDomainService<DisposalVerificationSubjectLicensing> domain, BaseParams baseParams)
        {
            var dispId = baseParams.Params.GetAs<long>("documentId");

            var existingValues = domain.GetAll()
                .Where(x => x.Disposal.Id == dispId)
                .Select(x => x.SurveySubject.Id)
                .ToArray();

	        var data = this.SurveySubjectDomain.GetAll()
		        .Select(x => new
		        {
			        x.Id,
			        x.Code,
			        x.Name,
			        Active = existingValues.Contains(x.Id)
		        })
		        .ToList();

            return new ListDataResult(data.ToList(), data.Count);
        }
    }
}
