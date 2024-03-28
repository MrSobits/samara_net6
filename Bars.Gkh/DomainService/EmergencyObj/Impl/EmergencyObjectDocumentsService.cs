namespace Bars.Gkh.DomainService
{
	using System.Linq;
    
    using B4;
	using Entities;
	using Castle.Windsor;
	using Domain;

	/// <summary>
	/// Сервис для Документы
	/// </summary>
	public class EmergencyObjectDocumentsService : IEmergencyObjectDocumentsService
	{
		/// <summary>
		/// Контейнер
		/// </summary>
        public IWindsorContainer Container { get; set; }

		/// <summary>
		/// Домен сервися для Документы аварийного дома
		/// </summary>
        public IDomainService<EmergencyObjectDocuments> EmergencyObjectDocumentsDomain { get; set; }

		/// <summary>
		/// Получить идентификатор документов
		/// </summary>
		/// <param name="baseParams">Базовые параметры</param>
		/// <returns>Результат выполнения запроса</returns>
		public IDataResult GetDocumentsIdByEmergencyObject(BaseParams baseParams)
		{
			var emergencyObjectId = baseParams.Params.GetAsId("emergencyObjectId");

			var documents = this.EmergencyObjectDocumentsDomain.GetAll()
				.FirstOrDefault(x => x.EmergencyObject.Id == emergencyObjectId);

			var id = documents != null ? documents.Id : 0L;

			return new BaseDataResult(new { documentsId = id });
		}
	}
}