namespace Bars.GkhGji.Regions.Khakasia.DomainService
{
    using Bars.B4;

	/// <summary>
	/// Сервис для Предмет проверки для приказа 
	/// </summary>
	public interface IDisposalSurveySubjectService
    {
		/// <summary>
		/// Добавить предмет проверки для приказа 
		/// </summary>
		/// <param name="baseParams">Базовые параметры</param>
		/// <returns>Результат выполнения запроса</returns>
		IDataResult AddDisposalSurveySubject(BaseParams baseParams);
    }
}