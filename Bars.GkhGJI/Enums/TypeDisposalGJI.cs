namespace Bars.GkhGji.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип распоряжения ГЖИ
    /// </summary>
    public enum TypeDisposalGji
    {
        /// <summary>
        /// По основанию
        /// </summary>
        [Display("По основанию")]
        Base = 10,

        /// <summary>
        /// На проверку документа ГЖИ
        /// </summary>
        [Display("Проверка исполнения предписания")]
        DocumentGji = 20,

        /// <summary>
        /// Без основания
        /// </summary>
        [Display("Без основания")]
        NullInspection = 30,

		/// <summary>
		/// Лицензирование
		/// </summary>
		[Display("Лицензирование")]
		Licensing = 40
	}
}