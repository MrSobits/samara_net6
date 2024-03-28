namespace Bars.GkhGji.DomainService.Dict
{
    using Bars.B4;

	/// <summary>
	/// Сервис для Типы контрагента
	/// </summary>
	public interface ITypeSurveyContragentTypeService
    {
		/// <summary>
		/// Добавить типы контрагента
		/// </summary>
		/// <param name="baseParams">Базовые параметры</param>
		/// <returns>Результат выполнения запроса</returns>
		IDataResult AddContragentTypes(BaseParams baseParams);
    }
}