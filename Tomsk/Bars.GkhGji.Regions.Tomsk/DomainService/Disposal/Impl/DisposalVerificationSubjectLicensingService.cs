namespace Bars.GkhGji.Regions.Tomsk.DomainService
{
	using System.Linq;
	using B4;
	using B4.Utils;
	using Castle.Windsor;
	using Entities;
	using Gkh.Domain;
	using GkhGji.Entities;
	using GkhGji.Entities.Dict;

	/// <summary>
	/// Сервис для Предмет проверки для приказа лицензирование
	/// </summary>
	public class DisposalVerificationSubjectLicensingService : IDisposalVerificationSubjectLicensingService
	{
		/// <summary>
		/// Домен сервис для Предмет проверки для приказа лицензирование
		/// </summary>
		public IDomainService<DisposalVerificationSubjectLicensing> Domain { get; set; }

		public IWindsorContainer Container { get; set; }

		/// <summary>
		/// Добавить предмет проверки для приказа лицензирование
		/// </summary>
		/// <param name="baseParams">Базовые параметры</param>
		/// <returns>Результат выполнения запроса</returns>
		public IDataResult AddDisposalVerificationSubjectLicensing(BaseParams baseParams)
        {
            var documentId = baseParams.Params.GetAs<long>("documentId");
            var ids = baseParams.Params.GetAs<string>("ids").ToLongArray();

			this.Container.InTransaction(() =>
			{
				this.Domain.GetAll()
					.Where(x => x.Disposal.Id == documentId)
					.Select(x => x.Id)
					.ForEach(x => this.Domain.Delete(x));

				foreach (var id in ids)
				{
					var newObj = new DisposalVerificationSubjectLicensing
					{
						Disposal = new Disposal {Id = documentId},
						SurveySubject = new SurveySubjectLicensing {Id = id}
					};

					this.Domain.Save(newObj);
				}
			});

	        return new BaseDataResult();
        }
    }
}
