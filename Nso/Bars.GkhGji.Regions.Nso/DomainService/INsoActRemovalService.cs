namespace Bars.GkhGji.Regions.Nso.DomainService
{
    using B4;

	/// <summary>
	/// Сервис для работы с Акт проверки предписания
	/// </summary>
	public interface INsoActRemovalService
    {
		/// <summary>
		/// Объединить акты проверки
		/// </summary>
		/// <param name="baseParams">Базовые параметры</param>
		/// <returns>Результат выполнения запроса</returns>
		IDataResult MergeActs(BaseParams baseParams);
    }
}