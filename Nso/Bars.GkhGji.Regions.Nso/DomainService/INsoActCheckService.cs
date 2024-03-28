namespace Bars.GkhGji.Regions.Nso.DomainService
{
    using B4;
    using Entities;

	/// <summary>
	/// Сервис для работы с Акт проверки
	/// </summary>
	public interface INsoActCheckService
    {
		/// <summary>
		/// Проверить, есть ли нарушения
		/// </summary>
		/// <param name="baseParams">Базовые параметры</param>
		/// <returns>Результат выполнения запроса</returns>
		IDataResult IsAnyHasViolation(BaseParams baseParams);

		/// <summary>
		/// Проверить, есть ли нарушения
		/// </summary>
		/// <param name="act">Акт проверки</param>
		/// <returns>Результат выполнения запроса</returns>
		bool IsAnyHasViolation(NsoActCheck act);

		/// <summary>
		/// Объединить акты проверки
		/// </summary>
		/// <param name="baseParams">Базовые параметры</param>
		/// <returns>Результат выполнения запроса</returns>
		IDataResult MergeActs(BaseParams baseParams);
    }
}