namespace Bars.GkhGji.Regions.BaseChelyabinsk.DomainService
{
    using Bars.B4;

    /// <summary>
	/// Сервис для работы с Акт проверки предписания
	/// </summary>
	public interface IChelyabinskActRemovalService
    {
		/// <summary>
		/// Объединить акты проверки
		/// </summary>
		/// <param name="baseParams">Базовые параметры</param>
		/// <returns>Результат выполнения запроса</returns>
		IDataResult MergeActs(BaseParams baseParams);
    }
}