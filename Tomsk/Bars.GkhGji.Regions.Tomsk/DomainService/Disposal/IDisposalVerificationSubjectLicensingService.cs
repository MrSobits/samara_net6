namespace Bars.GkhGji.Regions.Tomsk.DomainService
{
    using Bars.B4;

	/// <summary>
	/// Сервис для Предмет проверки для приказа лицензирование
	/// </summary>
	public interface IDisposalVerificationSubjectLicensingService
	{
		/// <summary>
		/// Добавить предмет проверки для приказа лицензирование
		/// </summary>
		/// <param name="baseParams">Базовые параметры</param>
		/// <returns>Результат выполнения запроса</returns>
		IDataResult AddDisposalVerificationSubjectLicensing(BaseParams baseParams);
    }
}