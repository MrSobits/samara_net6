namespace Bars.Gkh.Regions.Tatarstan.DomainService
{
	using Bars.B4;
	using Bars.Gkh.Regions.Tatarstan.Entities;

	/// <summary>
	/// Интерфейс сервис для <see cref="ConstructionObject"/>
	/// </summary>
	public interface IConstructionObjectService
	{
		/// <summary>
		/// Массово сменить статусы объектов строительства
		/// </summary>
		/// <param name="baseParams">Базовые параметры</param>
		/// <returns>Результат выполнения запроса</returns>
		IDataResult MassChangeState(BaseParams baseParams);
	}
}
