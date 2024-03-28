namespace Bars.GkhGji.Regions.Khakasia.DomainService
{
    using Bars.B4;

	/// <summary>
	/// Сервис для Группа нарушений для Документа ГЖИ
	/// </summary>
	public interface IViolationGroupService
    {
		/// <summary>
		/// Сохранить группы нарушений
		/// </summary>
		/// <param name="baseParams">Базовые параметры</param>
		/// <returns>Результат выполнения запроса</returns>
		IDataResult SavePoints(BaseParams baseParams);
    }
}