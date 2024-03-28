namespace Bars.Gkh.Regions.Tatarstan.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип документа объекта строительства
    /// </summary>
    public enum ConstructionObjectDocumentType
    {
		/// <summary>
		/// Не задано
		/// </summary>
		[Display("Не задано")]
		NotSet = 10,

		/// <summary>
		/// Лист согласования технических условий
		/// </summary>
		[Display("Лист согласования технических условий")]
        MatchingSheet = 20,

		/// <summary>
		/// ПСД на строительство и инженерные сети
		/// </summary>
		[Display("ПСД на строительство и инженерные сети")]
        Psd = 30,

		/// <summary>
		/// Экспертиза ПСД
		/// </summary>
		[Display("Экспертиза ПСД")]
        PsdExpertise = 40,

		/// <summary>
		/// Акт ввода дома в эксплуатацию
		/// </summary>
		[Display("Акт ввода дома в эксплуатацию")]
        CommissioningAct = 50,

		/// <summary>
		/// Иные документы
		/// </summary>
		[Display("Иные документы")]
        Other = 60
    }
}
