namespace Bars.Gkh.RegOperator.DomainService
{
	using B4;

	/// <summary>
	/// Сервис для работы с "Импортируемая оплата"
	/// </summary>
    public interface IImportedPaymentService
    {
		/// <summary>
		/// Сопоставить ЛС
		/// </summary>
		/// <param name="baseParams">Базовые параметры</param>
		/// <returns>Результат</returns>
		IDataResult ComparePersonalAccount(BaseParams baseParams);

		/// <summary>
		/// Сопоставить ЛС
		/// </summary>
		/// <param name="paymentId">Идентификатор оплаты</param>
		/// <param name="paId">Идентификатор лицевого счета</param>
		/// <returns>Результат</returns>
		IDataResult ComparePersonalAccount(long paymentId, long paId);
    }
}