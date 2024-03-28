namespace Bars.GkhGji.Regions.BaseChelyabinsk.DomainService
{
    using Bars.B4;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.ActCheck;

    /// <summary>
	/// Сервис для работы с Акт проверки
	/// </summary>
	public interface IChelyabinskActCheckService
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
		bool IsAnyHasViolation(ChelyabinskActCheck act);

		/// <summary>
		/// Объединить акты проверки
		/// </summary>
		/// <param name="baseParams">Базовые параметры</param>
		/// <returns>Результат выполнения запроса</returns>
		IDataResult MergeActs(BaseParams baseParams);
    }
}