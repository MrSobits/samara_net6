namespace Bars.Gkh.RegOperator.DomainService
{
    using B4;

	/// <summary>
	/// Сервис для Информация по начисленным льготам
	/// </summary>
	public interface IPersonalAccountBenefitsService
	{
		/// <summary>
		/// Сменить значение Сумма
		/// </summary>
		/// <param name="baseParams">Базовые параметры</param>
		/// <returns>Результат выполнения запроса</returns>
        IDataResult UpdateSum(BaseParams baseParams);
    }
}