namespace Bars.GkhGji.DomainService.Dict
{
    using Bars.B4;

	/// <summary>
	/// Сервис для Правовые основания
	/// </summary>
	public interface ITypeSurveyLegalReasonService
    {
		/// <summary>
		/// Добавить правовые основания
		/// </summary>
		/// <param name="baseParams">Базовые параметры</param>
		/// <returns>Результат выполнения запроса</returns>
		IDataResult AddLegalReasons(BaseParams baseParams);
    }
}