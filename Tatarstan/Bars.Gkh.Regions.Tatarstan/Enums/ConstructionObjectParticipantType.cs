namespace Bars.Gkh.Regions.Tatarstan.Enums
{
    using Bars.B4.Utils;

	/// <summary>
	/// Тип участника объекта строительства
	/// </summary>
	public enum ConstructionObjectParticipantType
	{
		/// <summary>
		/// Не задано
		/// </summary>
		[Display("Не задано")]
		NotSet = 10,

		/// <summary>
		/// Заказчик
		/// </summary>
		[Display("Заказчик")]
        Customer = 20,

		/// <summary>
		/// Подрядная организация
		/// </summary>
		[Display("Подрядная организация")]
        Contractor = 30
    }
}
