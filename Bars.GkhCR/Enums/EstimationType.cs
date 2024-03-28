namespace Bars.GkhCr.Enums
{
    using B4.Utils;

    /// <summary>
    /// Тип сметы
    /// </summary>
    public enum EstimationType
    {
		/// <summary>
		/// Не задано
		/// </summary>
		[Display("Не задано")]
        None = 0,

		/// <summary>
		/// Смета заказчика
		/// </summary>
		[Display("Смета заказчика")]
        Customer = 10,

		/// <summary>
		/// Смета подрядчика
		/// </summary>
		[Display("Смета подрядчика")]
		Сontractor = 20,

		/// <summary>
		/// Смета на непредвиденные расходы
		/// </summary>
		[Display("Смета на непредвиденные расходы")]
		Contingencies = 30
    }
}
