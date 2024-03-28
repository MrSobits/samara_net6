namespace Bars.Gkh.DomainService
{
	using B4;

	/// <summary>
	/// Сервис для Документы
	/// </summary>
	public interface IEmergencyObjectDocumentsService
	{
		/// <summary>
		/// Получить идентификатор документов
		/// </summary>
		/// <param name="baseParams">Базовые параметры</param>
		/// <returns>Результат выполнения запроса</returns>
		IDataResult GetDocumentsIdByEmergencyObject(BaseParams baseParams);
    }
}