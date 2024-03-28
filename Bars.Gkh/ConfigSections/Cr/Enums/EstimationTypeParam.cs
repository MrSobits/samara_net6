namespace Bars.Gkh.ConfigSections.Cr.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Параметр "Тип сметы"
    /// </summary>
    public enum EstimationTypeParam
    {
		/// <summary>
		/// Не использовать
		/// </summary>
		[Display("Не использовать")]
        DoNotUse = 0,

		/// <summary>
		/// Использовать
		/// </summary>
        [Display("Использовать")]
        Use = 10
    }
}
