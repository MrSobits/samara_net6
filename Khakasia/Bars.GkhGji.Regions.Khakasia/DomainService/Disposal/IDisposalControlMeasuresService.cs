namespace Bars.GkhGji.Regions.Khakasia.DomainService
{
    using Bars.B4;

	/// <summary>
	/// Сервис для Мероприятия по контролю распоряжения ГЖИ
	/// </summary>
	public interface IDisposalControlMeasuresService
    {
		/// <summary>
		/// Добавить Мероприятия по контролю распоряжения ГЖИ
		/// </summary>
		/// <param name="baseParams">Базовые параметры</param>
		/// <returns>Результат выполнения запроса</returns>
		IDataResult AddDisposalControlMeasures(BaseParams baseParams);
    }
}