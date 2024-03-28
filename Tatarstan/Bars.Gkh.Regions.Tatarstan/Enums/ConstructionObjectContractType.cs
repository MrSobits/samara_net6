namespace Bars.Gkh.Regions.Tatarstan.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип контракта объекта строительства
    /// </summary>
    public enum ConstructionObjectContractType
    {
		/// <summary>
		/// Не задано
		/// </summary>
		[Display("Не задано")]
		NotSet = 10,

		/// <summary>
		/// Договор на СМР
		/// </summary>
		[Display("Договор на СМР")]
        Smr = 20,

		/// <summary>
		/// Договор на ПСД
		/// </summary>
		[Display("Договор на ПСД")]
        Psd = 30,

		/// <summary>
		/// Технадзор
		/// </summary>
		[Display("Технадзор")]
        Supervision = 40,

		/// <summary>
		/// Доп. соглашение
		/// </summary>
		[Display("Доп. соглашение")]
        Additional = 50
    }
}
