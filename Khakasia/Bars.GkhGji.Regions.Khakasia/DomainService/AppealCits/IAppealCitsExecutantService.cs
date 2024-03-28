namespace Bars.GkhGji.Regions.Khakasia.DomainService
{
    using Bars.B4;

	/// <summary>
	/// Сервис для Исполнитель обращения
	/// </summary>
    public interface IAppealCitsExecutantService
    {
		/// <summary>
		/// Добавить исполнителей
		/// </summary>
		/// <param name="baseParams">Базовые параметры</param>
		/// <returns>Результат выполнения запроса</returns>
        IDataResult AddExecutants(BaseParams baseParams);

		/// <summary>
		/// Перенаправить исполнителя
		/// </summary>
		/// <param name="baseParams">Базовые параметры</param>
		/// <returns>Результат выполнения запроса</returns>
        IDataResult RedirectExecutant(BaseParams baseParams);
    }
}