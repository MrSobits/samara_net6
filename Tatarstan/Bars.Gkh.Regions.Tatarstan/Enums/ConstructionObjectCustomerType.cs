namespace Bars.Gkh.Regions.Tatarstan.Enums
{
    using Bars.B4.Utils;

	/// <summary>
	/// Тип заказчика объекта строительства
	/// </summary>
	public enum ConstructionObjectCustomerType
	{
		/// <summary>
		/// Не задано
		/// </summary>
		[Display("Не задано")]
		NotSet = 10,

		/// <summary>
		/// ГЖФ
		/// </summary>
		[Display("ГЖФ")]
		Gzhf = 20,

		/// <summary>
		/// Исполком
		/// </summary>
		[Display("Исполком")]
		Ispolkom = 30,

		/// <summary>
		/// Минстрой
		/// </summary>
		[Display("Минстрой")]
		Minstroj = 40,

		/// <summary>
		/// ГИСУ
		/// </summary>
		[Display("ГИСУ")]
		Gisu = 50,

		/// <summary>
		/// Организация технического надзора
		/// </summary>
		[Display("Организация технического надзора")]
		TechnicalSupervisionOrganization = 60,

		/// <summary>
		/// Разработчики ПСД
		/// </summary>
		[Display("Разработчики ПСД")]
		PsdDevelopers = 70
	}
}
