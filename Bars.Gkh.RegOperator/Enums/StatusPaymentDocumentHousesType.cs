namespace Bars.Gkh.RegOperator.Enums
{
    using B4.Utils;

	/// <summary>
	/// Фильтр Наличие начислений в карточке ЛС
	/// </summary>
    public enum StatusPaymentDocumentHousesType
    {
		/// <summary>
		/// Начисление по базовому тарифу > 0
		/// </summary>
		[Display("Направленно")]
        Directed = 0,

		/// <summary>
		/// Начисление по тарифу решения > 0
		/// </summary>
		[Display("размещено")]
        Posted = 10
    }
}
